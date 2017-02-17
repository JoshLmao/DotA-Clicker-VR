using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpItemController : MonoBehaviour
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

    FPSPlayerController m_fpsController;
    SphereCollider m_collider;

    void Awake()
    {
        m_fpsController = transform.parent.GetComponent<FPSPlayerController>();
        m_collider = GetComponent<SphereCollider>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (CurrentObject != null)
        {
            CurrentObject.GetComponent<Rigidbody>().useGravity = false;
            CurrentObject.transform.position = this.transform.position;

            m_collider.enabled = true;
        }
        else
        {
            m_collider.enabled = false;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ItemModifier" && CurrentObject != null) //Is holding an object & in item modifier trigger
        {
            string hero = col.transform.parent.parent.name; //Heirarchy is [ClickerName]BuyStand > ItemModifierStand > Collider
            if (CurrentObject.name.Contains("iron_branchPrefab"))
            {
                if (IronBranchModifierAdded != null)
                    IronBranchModifierAdded.Invoke(hero, "ironBranch", Constants.ModifierIronBranchDuration); //Main place to set Item Modifier Duration
            }
            else if (CurrentObject.name.Contains("clarityPrefab"))
            {
                if (ClarityModifierAdded != null)
                    ClarityModifierAdded.Invoke(hero, "clarity", Constants.ModifierClarityDuration);
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

    public void OnTriggerExit(Collider col)
    {

    }

    public void OnDropObject()
    {
        if (CurrentObject != null)
        {
            CurrentObject.GetComponent<Rigidbody>().useGravity = true;
            CurrentObject = null;
        }
    }
}
