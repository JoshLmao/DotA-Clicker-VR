using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FPSPlayerController : MonoBehaviour {

    [SerializeField]
    GameObject m_collider;

    [SerializeField]
    Transform m_pickUpPosition;

    FirstPersonController m_unityFPSCont;
    RadiantSceneController m_sceneController;
    Camera m_camera;

    bool m_isHolding;

    void Awake()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_camera = GetComponent<Camera>();

        m_unityFPSCont = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnLeftClickUp();
            m_unityFPSCont.m_MouseLook.m_cursorIsLocked = false;
        }

        if (Input.GetButton("Fire1"))
        {
            OnLeftClickDown();
            m_unityFPSCont.m_MouseLook.m_cursorIsLocked = true;
        }
    }

    void OnLeftClickUp()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        //Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out hit, 2))
        {
            if (hit.collider.tag == "Player")
                Physics.IgnoreCollision(hit.collider, m_collider.GetComponent<Collider>());

            Debug.Log("hit " + hit.collider.gameObject.name);

            var name = hit.collider.gameObject.name;
            //On click to click on clicker
            if (name == "ClickButtonBack")
            {
                var btnManager = hit.collider.gameObject.GetComponent<ButtonManager>();
                btnManager.OnClickButton();
            }
            //On click to buy a multipler on clicker
            else if (name == "BuyButton")
            {
                var btnManager = hit.collider.gameObject.GetComponent<ButtonManager>();
                btnManager.OnBuyMultiplier();
            }
            //clicker ability buttons
            else if (name.Contains("Btn"))
            {
                var obj = FindController(hit.collider.transform.parent.gameObject);
                if (obj != null)
                {
                    var clicker = obj.GetComponent<RadiantClickerController>();
                    clicker.ActivateAbility(name);
                }
            }
        }

        if(m_isHolding)
        {
            m_isHolding = false;
        }
    }

    void OnLeftClickDown()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        //Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out hit, 2))
        {
            if(hit.collider.tag == "Interactable")
            {
                m_isHolding = true;
                var currentItemController = transform.Find("PickUpPosition").GetComponent<PickedUpItemController>();
                currentItemController.CurrentObject = hit.collider.gameObject;
            }
        }
    }

    GameObject FindController(GameObject currentObj)
    {
        if (currentObj == null) return null;

        var controller = currentObj.GetComponent<RadiantClickerController>();
        if (controller == null)
            return FindController(currentObj.transform.parent.gameObject);
        else
            return controller.gameObject;
    }
}
