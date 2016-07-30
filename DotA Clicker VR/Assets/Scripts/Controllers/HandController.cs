using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour {

    public GameObject CurrentObject { get; set; }
    public bool IsHoldingObj { get; set; }

    SteamVR_TrackedController m_controller { get; set; }

    bool m_canPickupObj = false;

	void Start ()
    {
        m_controller = this.GetComponent<SteamVR_TrackedController>();

        //m_controller.TriggerClicked += OnTriggerClicked;
	}
	
	void FixedUpdate ()
    {
	    
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Interactable")
        {
            CurrentObject = col.gameObject;
            m_canPickupObj = true;
        }
    }

    void OnTriggerClicked()
    {

    }
}
