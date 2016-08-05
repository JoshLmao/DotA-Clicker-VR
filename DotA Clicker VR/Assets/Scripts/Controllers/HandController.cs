using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Controller for both Vive Controllers
/// </summary>
public class HandController : MonoBehaviour {

    public GameObject CurrentObject { get; set; }
    public bool IsHoldingObj { get; set; }

    SteamVR_TrackedController m_controller { get; set; }
    SteamVR_LaserPointer m_laserPointer { get; set; }

    bool m_canPickupObj = false;
    bool m_enablePointer = true;

    GameObject m_activeUI;
    bool m_canClickOnUI = false;

	void Start ()
    {
        m_controller = this.GetComponent<SteamVR_TrackedController>();
        m_laserPointer = GetComponent<SteamVR_LaserPointer>();

        m_controller.TriggerClicked += OnTriggerClicked;
        m_laserPointer.PointerIn += OnPointerIn;
        m_laserPointer.PointerOut += OnPointerOut;
	}
	
	void Update ()
    {
        //Toggle for LaserPointer
	    if(m_controller.triggerPressed && m_controller.gripped)
        {
            m_enablePointer = !m_enablePointer;
        }

        m_laserPointer.active = m_enablePointer;

        if(m_canClickOnUI && m_activeUI != null)
        {
            if(m_controller.triggerPressed)
            {
                Debug.Log("Clicking");
                var yes = m_activeUI.GetComponent<Button>();
                yes.onClick.Invoke(); //do button click
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
        if(e.target.gameObject.layer == 5 && e.target.gameObject.GetComponent<Button>() && e.target.gameObject.GetComponent<BoxCollider>())
        {
            Debug.Log("Aiming at clickable button");

            m_activeUI = e.target.gameObject;
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
        m_activeUI = null;
    }
}
