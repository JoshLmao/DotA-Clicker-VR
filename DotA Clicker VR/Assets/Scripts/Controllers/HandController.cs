using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Controller for both Vive Controllers
/// </summary>
public class HandController : MonoBehaviour {

    public GameObject CurrentObject { get; set; }
    public bool IsHoldingObj { get; set; }
    public Transform CurrentAimPosition { get; set; }

    SteamVR_TrackedController m_controller { get; set; }
    SteamVR_LaserPointer m_laserPointer { get; set; }

    bool m_canPickupObj = false;
    bool m_enablePointer = true;

    Button m_activeButtonUI;
    Slider m_activeSliderUI;
    Transform m_sliderTranform;
    Toggle m_activeToggleUI;
    Scrollbar m_activeScrollerUI;
    bool m_canClickOnUI = false;

	void Start ()
    {
        m_controller = this.GetComponent<SteamVR_TrackedController>();
        m_laserPointer = GetComponent<SteamVR_LaserPointer>();

        m_controller.TriggerClicked += OnTriggerClicked;
        if(m_laserPointer != null)
        {
            m_laserPointer.PointerIn += OnPointerIn;
            m_laserPointer.PointerOut += OnPointerOut;
        }
    }
	
	void Update ()
    {
        //Toggle for LaserPointer
	    if(m_controller.triggerPressed && m_controller.gripped)
        {
            m_enablePointer = !m_enablePointer;
        }

        if(m_laserPointer != null)
        {
            m_laserPointer.active = m_enablePointer;
        }

        if (m_canClickOnUI)
        {
            if (m_activeButtonUI != null)
            {
                m_activeButtonUI.Select();
                if (m_controller.triggerPressed)
                {
                    Debug.Log("Clicked on Button");
                    var yes = m_activeButtonUI.GetComponent<Button>();
                    yes.onClick.Invoke(); //do button click
                }
            }
            else if(m_activeSliderUI != null)
            {
                if(m_controller.triggerPressed)
                {
                    Debug.Log("Clicked on Slider");
                    //m_sliderTranform = m_activeSliderUI.transform;
                }
            }
            else if(m_activeToggleUI != null)
            {
                m_activeToggleUI.Select();
                if(m_controller.triggerPressed)
                {
                    Debug.Log("Clicked on Toggle");
                    m_activeToggleUI.isOn = !m_activeToggleUI.isOn;
                }
            }
            else if(m_activeScrollerUI != null)
            {
                m_activeScrollerUI.Select();
                if(m_controller.triggerPressed)
                {
                    Debug.Log("Clicked on Slider");
                    m_activeScrollerUI.value = m_activeScrollerUI.value == 1 ? 0 : 1;
                }
            }
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Interactable")
        {
            CurrentObject = col.gameObject;
            m_canPickupObj = true;
        }
        else if(col.tag == "Upgrade")
        {

        }
    }

    void OnTriggerClicked(object sender, ClickedEventArgs e)
    {
        if(m_canPickupObj && CurrentObject != null)
        {
            IsHoldingObj = true;
        }
    }

    void OnPointerIn(object sender, PointerEventArgs e)
    {
        CurrentAimPosition = e.target.transform;

        if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<Button>() && e.target.gameObject.GetComponent<BoxCollider>())
        {
            Debug.Log("Aiming at clickable button");

            m_activeButtonUI = e.target.gameObject.GetComponent<Button>();
            m_canClickOnUI = true;
        }
        else if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<BoxCollider>() && e.target.gameObject.GetComponentInParent<Slider>())
        {
            Debug.Log("Aiming at Slider");

            m_activeSliderUI = e.target.gameObject.GetComponentInParent<Slider>();
            m_canClickOnUI = true;
        }
        else if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<Toggle>() && e.target.gameObject.GetComponent<BoxCollider>())
        {
            Debug.Log("Aiming at Toggle");

            m_activeToggleUI = e.target.gameObject.GetComponent<Toggle>();
            m_canClickOnUI = true;
        }
        else if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<BoxCollider>() && e.target.gameObject.GetComponentInParent<Scrollbar>())
        {
            Debug.Log("Aiming at Slider");

            m_activeScrollerUI = e.target.gameObject.GetComponentInParent<Scrollbar>();
            m_canClickOnUI = true;
        }
        else
        {
            m_canClickOnUI = false;
        }
    }

    void OnPointerOut(object sender, PointerEventArgs e)
    {
        m_canClickOnUI = false;

        m_activeToggleUI = null;
        m_activeSliderUI = null;
        m_activeButtonUI = null;
        m_activeScrollerUI = null;
    }
}
