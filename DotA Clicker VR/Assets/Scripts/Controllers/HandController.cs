using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;
using System;
using System.Linq;

/// <summary>
/// Controller for both Vive Controllers
/// </summary>
public class HandController : MonoBehaviour
{
    public delegate void OnIronBranchModifier(string hero, string item, int duration);
    public static event OnIronBranchModifier IronBranchModifierAdded;
    public delegate void OnClarityModifier(string hero, string item, int duration);
    public static event OnClarityModifier ClarityModifierAdded;
    public delegate void OnMagicStickModifier(string hero, string item, int duration);
    public static event OnMagicStickModifier MagicStickModifierAdded;
    public delegate void OnQuellingBladeModifier(string hero, string item, int duration);
    public static event OnQuellingBladeModifier QuellingBladeModifierAdded;
    public delegate void OnMangoModifier(string hero, string item, int duration);
    public static event OnMangoModifier MangoModifierAdded;
    public delegate void OnPowerTreadsModifier(string hero, string item, int duration);
    public static event OnPowerTreadsModifier PowerTreadsModifierAdded;
    public delegate void OnBottleModifier(string hero, string item, int duration);
    public static event OnBottleModifier BottleModifierAdded;
    public delegate void OnBlinkDaggerModifier(string hero, string item, int duration);
    public static event OnBlinkDaggerModifier BlinkDaggerModifierAdded;
    public delegate void OnHyperstoneModifier(string hero, string item, int duration);
    public static event OnHyperstoneModifier HyperstoneModifierAdded;
    public delegate void OnBloodstoneModifier(string hero, string item, int duration);
    public static event OnBloodstoneModifier BloodstoneModifierAdded;
    public delegate void OnReaverModifier(string hero, string item, int duration);
    public static event OnReaverModifier ReaverModifierAdded;
    public delegate void OnDivineRapierModifier(string hero, string item, int duration);
    public static event OnDivineRapierModifier DivineRapierModifierAdded;
    public delegate void OnRecipeModifier(string hero, string item, int duration);
    public static event OnRecipeModifier RecipeModifierAdded;

    public GameObject CurrentObject = null;
    public Transform PriorTranform = null;
    public bool IsHoldingObj = false;
    public Transform CurrentAimTranform = null;

    protected SteamVR_TrackedController m_controller { get; set; }
    SteamVR_LaserPointer m_laserPointer { get; set; }

    bool m_canPickupObj = false;
    bool m_enablePointer = true;
    bool m_moveSliderHandle;

    public bool AimingAtUI = false;
    Button m_activeButtonUI;
    Slider m_activeSliderUI;
    Transform m_sliderTranform;
    Toggle m_activeToggleUI;
    Scrollbar m_activeScrollerUI;
    GameObject m_scrollableMenu;

    Vector3 m_holdingPreviousFrame;
    Vector3 m_holdingCurrentFrame;

    //Sliders
    Ray sliderRaycast;

    public Rigidbody attachPoint;
    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;

    GameObject LeftHandCanvas;

    bool m_gotVRTKInteract = false;

    public virtual void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    public virtual void Start()
    {
        m_controller = this.GetComponent<SteamVR_TrackedController>();
        m_laserPointer = GetComponent<SteamVR_LaserPointer>();

        m_controller.TriggerClicked += OnTriggerClicked;
        m_controller.TriggerUnclicked += OnTriggerUnclicked;

        if(m_laserPointer != null)
        {
            m_laserPointer.PointerIn += OnPointerIn;
            m_laserPointer.PointerOut += OnPointerOut;
        }
    }

    private void OnUngrabObject(object sender, ObjectInteractEventArgs e)
    {
        CurrentObject = null;
    }

    private void OnGrabObject(object sender, ObjectInteractEventArgs e)
    {
        CurrentObject = e.target;
    }

    void Update()
    {
        if(LeftHandCanvas != null)
            LeftHandCanvas.transform.localPosition = new Vector3(0f, 0f, 0f);

        if (AimingAtUI)
        {
            //Detect for Scrollable UI
            if (m_scrollableMenu != null)
            {
                ScrollableMenuMethod();
            }
        }

        if(!m_gotVRTKInteract)
        {
            //Workaround for VRTK scripts not being moved on Start
            try
            {
                var events = this.GetComponentInChildren<VRTK_InteractGrab>();
                events.ControllerGrabInteractableObject += OnGrabObject;
                events.ControllerUngrabInteractableObject += OnUngrabObject;

                m_gotVRTKInteract = true;
            }
            catch (Exception e)
            {

            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Interactable")
        {
            CurrentObject = col.gameObject;
            m_canPickupObj = true;

            this.GetComponent<SphereCollider>().isTrigger = true;
            col.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        else if(col.tag == "Ability")
        {
            RadiantClickerController controller = col.gameObject.GetComponentInParent<RadiantClickerController>();
            controller.ActivateAbility(col.gameObject.name, m_controller.controllerIndex);
        }
        else if(col.tag == "Killable") //Creeps, Wards, etc. Anything that can do SetTrigger("isKilled")
        {
            Debug.Log("Killed 'Killable' tagged obj");
            col.gameObject.GetComponent<Animator>().SetTrigger("isKilled");
        }
        else if(col.tag == "VRTKInteractTag")
        {
            this.GetComponent<SphereCollider>().isTrigger = true;
        }
    }

    //MAIN PLACE TO TRIGGER COMPLETE MODIFIER
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "ItemModifier" && CurrentObject != null) //Is holding an object & in item modifier trigger
        {
            string hero = col.transform.parent.parent.name; //Heirarchy is [ClickerName]BuyStand > ItemModifierStand > Collider
            var heroController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().SceneHeroes.FirstOrDefault(x => x.name == hero);
            if (heroController.m_currentModifier != string.Empty)
                return; //return if theres a modifer active

            if (CurrentObject.name.Contains("iron_branchPrefab"))
            {
                if (IronBranchModifierAdded != null)
                    IronBranchModifierAdded.Invoke(hero, "ironBranch", Constants.ModifierIronBranchDuration); //Main place to set Item Modifier Duration
            }
            else if (CurrentObject.name.Contains("clarityPrefab"))
            {
                if (ClarityModifierAdded != null)
                    ClarityModifierAdded.Invoke(hero, "clairty", Constants.ModifierClarityDuration);
            }
            else if (CurrentObject.name.Contains("magic_stickPrefab"))
            {
                if (MagicStickModifierAdded != null)
                    MagicStickModifierAdded.Invoke(hero, "magicStick", Constants.ModifierMagicStickDuration);
            }
            else if (CurrentObject.name.Contains("quelling_bladePrefab"))
            {
                if (QuellingBladeModifierAdded != null)
                    QuellingBladeModifierAdded.Invoke(hero, "quellingBlade", Constants.ModifierQuellingBladeDuration);
            }
            else if (CurrentObject.name.Contains("mangoPrefab"))
            {
                if (MangoModifierAdded != null)
                    MangoModifierAdded.Invoke(hero, "mango", Constants.ModifierMangoDuration);
            }
            else if (CurrentObject.name.Contains("power_treadsPrefab"))
            {
                if (PowerTreadsModifierAdded != null)
                    PowerTreadsModifierAdded.Invoke(hero, "powerTreads", Constants.ModifierPowerTreadsDuration);
            }
            else if (CurrentObject.name.Contains("bottlePrefab"))
            {
                if (BottleModifierAdded != null)
                    BottleModifierAdded.Invoke(hero, "bottle", Constants.ModifierBottleDuration);
            }
            else if (CurrentObject.name.Contains("blink_daggerPrefab"))
            {
                if (BlinkDaggerModifierAdded != null)
                    BlinkDaggerModifierAdded.Invoke(hero, "blinkDagger", Constants.ModifierBlinkDaggerDuration);
            }
            else if (CurrentObject.name.Contains("hyperstonePrefab"))
            {
                if (HyperstoneModifierAdded != null)
                    HyperstoneModifierAdded.Invoke(hero, "hyperstone", Constants.ModifierHyperstoneDuration);
            }
            else if (CurrentObject.name.Contains("bloodstonePrefab"))
            {
                if (BloodstoneModifierAdded != null)
                    BloodstoneModifierAdded.Invoke(hero, "bloodstone", Constants.ModifierBloodstoneDuration);
            }
            else if (CurrentObject.name.Contains("reaverPrefab"))
            {
                if (ReaverModifierAdded != null)
                    ReaverModifierAdded.Invoke(hero, "reaver", Constants.ModifierReaverDuration);
            }
            else if (CurrentObject.name.Contains("divine_rapierPrefab"))
            {
                if (DivineRapierModifierAdded != null)
                    DivineRapierModifierAdded.Invoke(hero, "divineRapier", Constants.ModifierDivineRapierDuration);
            }
            else if (CurrentObject.name.Contains("recipePrefab"))
            {
                if (RecipeModifierAdded != null)
                    RecipeModifierAdded.Invoke(hero, "recipe", Constants.ModifierRecipeDuration);
            }
            GameObject.Destroy(CurrentObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        CurrentObject = null;

        if (col.tag == "VRTKInteractTag")
        {
            this.GetComponent<SphereCollider>().isTrigger = false;
        }
        else if(col.tag == "Interactable")
        {
            this.GetComponent<SphereCollider>().isTrigger = false;
            col.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    void OnTriggerClicked(object sender, ClickedEventArgs e)
    { 

    }

    void OnTriggerUnclicked(object sender, ClickedEventArgs e)
    {
        m_moveSliderHandle = false;
    }

    void OnPointerIn(object sender, PointerEventArgs e)
    {
        //Debug.Log("Aiming at '" + e.target.gameObject.name + "' and has layer '" + e.target.gameObject.layer + "'");
        if (e.target.gameObject.layer == 5)
        {
            AimingAtUI = true;
        }
        else
        {
            AimingAtUI = false;
        }
    }

    void OnPointerOut(object sender, PointerEventArgs e)
    {
        AimingAtUI = false;
    }

    /// <summary>
    /// Method for added haptics to controllers. Specify which controller and for how long. Keep under 5000ms for length
    /// </summary>
    /// <param name="index">Controller Index</param>
    /// <param name="length">Duration in Milliseconds</param>
    public static void RumbleController(uint index, ushort length)
    {
        SteamVR_Controller.Input((int)index).TriggerHapticPulse(length);
    }

    void ScrollableMenuMethod()
    {
        if (!m_controller.padTouched) return;

        Transform managersScrollable = m_scrollableMenu.transform.parent.transform.Find("ManagersScrollbar");
        Transform upgradesScrollable = m_scrollableMenu.transform.parent.transform.Find("UpgradesScrollbar");
        Scrollbar scr;
        if (managersScrollable != null && managersScrollable.GetComponent<Scrollbar>())
        {
            scr = managersScrollable.gameObject.GetComponent<Scrollbar>();
        }
        else if (upgradesScrollable != null && upgradesScrollable.GetComponent<Scrollbar>())
        {
            scr = upgradesScrollable.gameObject.GetComponent<Scrollbar>();
        }
        else
        {
            scr = null;
        }

        if (scr != null)
        {
            if (m_controller.controllerState.rAxis0.y > 0.5f)
            {
                scr.value += 0.005f;
            }
            else if (m_controller.controllerState.rAxis0.y < 0.5f)
            {
                scr.value -= 0.005f;
            }
        }
    }

}
