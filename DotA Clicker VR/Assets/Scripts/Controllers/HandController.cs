using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Controller for both Vive Controllers
/// </summary>
public class HandController : MonoBehaviour {

    public GameObject CurrentObject { get; set; }
    public Transform PriorTranform { get; set; }
    public bool IsHoldingObj { get; set; }
    public Transform CurrentAimTranform { get; set; }

    SteamVR_TrackedController m_controller { get; set; }
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

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start ()
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

        //LeftHandCanvas = transform.Find("WorldSpaceMenu").gameObject;
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
    }

    //Physics based stuffs
    void FixedUpdate()
    {
        //Hijacked from SteamVR_TestThrow
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && CurrentObject != null)
        {
            this.GetComponent<SphereCollider>().enabled = false;
            var go = CurrentObject;
            go.transform.position = attachPoint.transform.position;

            joint = go.AddComponent<FixedJoint>();
            joint.connectedBody = attachPoint;
        }
        else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            var go = joint.gameObject;
            var rigidbody = go.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            joint = null;
            Object.Destroy(go, 15.0f);

            // We should probably apply the offset between trackedObj.transform.position
            // and device.transform.pos to insert into the physics sim at the correct
            // location, however, we would then want to predict ahead the visual representation
            // by the same amount we are predicting our render poses.

            var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(device.velocity);
                rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
            }
            else
            {
                rigidbody.velocity = device.velocity;
                rigidbody.angularVelocity = device.angularVelocity;
            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }

        this.GetComponent<SphereCollider>().enabled = true;
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Interactable")
        {
            CurrentObject = col.gameObject;
            m_canPickupObj = true;
        }
        else if(col.tag == "Ability")
        {
            RadiantClickerController controller = col.gameObject.GetComponentInParent<RadiantClickerController>();
            controller.ActivateAbility(col.gameObject.name, m_controller.controllerIndex);
        }
        else if(col.tag == "Creep")
        {
            //Kill the creep
            Debug.Log("Killed creep");
            col.gameObject.GetComponent<Animator>().SetTrigger("isKilled");
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
        //CurrentAimTranform = e.target.transform;

        //if(e.target.gameObject.layer == 5)
        //{
        //    //Enable LaserPointer
        //    m_laserPointer.active = true;
        //}
        //else
        //{
        //    m_laserPointer.active = false;
        //}

        //if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<Button>() && e.target.gameObject.GetComponent<BoxCollider>())
        //{
        //    m_activeButtonUI = e.target.gameObject.GetComponent<Button>();
        //    m_canClickOnUI = true;
        //}
        //else if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<BoxCollider>() && e.target.gameObject.GetComponentInParent<Slider>())
        //{
        //    Debug.Log("Aiming at Slider");

        //    m_activeSliderUI = e.target.gameObject.GetComponentInParent<Slider>();
        //    m_canClickOnUI = true;
        //}
        //else if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<Toggle>() && e.target.gameObject.GetComponent<BoxCollider>())
        //{
        //    m_activeToggleUI = e.target.gameObject.GetComponent<Toggle>();
        //    m_canClickOnUI = true;
        //}
        //else if(e.target.gameObject.layer == 5 && e.target.GetComponent<BoxCollider>() && e.target.gameObject.GetComponent<ScrollRect>())
        //{
        //    m_canClickOnUI = true;
        //    m_scrollableMenu = e.target.gameObject;
        //}

        if(e.target.gameObject.layer == 5)
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
        //m_canClickOnUI = false;
        
        //m_activeToggleUI = null;
        //m_activeSliderUI = null;
        //m_activeButtonUI = null;
        //m_activeScrollerUI = null;

        ////Disable LaserPointer
        //m_laserPointer.active = false;
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
