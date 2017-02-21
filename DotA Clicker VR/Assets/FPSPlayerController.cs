using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class FPSPlayerController : MonoBehaviour {

    [SerializeField]
    GameObject m_collider;

    [SerializeField]
    Transform m_pickUpPosition;

    FirstPersonController m_unityFPSCont;
    RadiantSceneController m_sceneController;
    Camera m_camera;
    PickedUpItemController m_pickUpController;

    bool m_isHolding;
    bool m_menuOpen;
    [SerializeField]
    GameObject m_menuPrefab;
    GameObject m_currentMenu;
    Blur m_blurEffect;
    [SerializeField]
    Shader m_blurShader;
    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "RadiantSide")
        {
            m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        }
        m_camera = GetComponent<Camera>();

        m_unityFPSCont = GameObject.Find("FPSController").GetComponent<FirstPersonController>();
        m_pickUpController = transform.Find("PickUpPosition").GetComponent<PickedUpItemController>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnLeftClickUp();

            m_pickUpController.OnDropObject();

            //m_unityFPSCont.m_MouseLook.m_cursorIsLocked = true;
        }

        if (Input.GetButton("Fire1"))
        {
            OnLeftClickDown();
            //m_unityFPSCont.m_MouseLook.m_cursorIsLocked = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!m_menuOpen)
            {
                ShowMainMenu();
            }
            else
            {
                OnResume();
            }
        }
    }

    void OnLeftClickUp()
    {
        var forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, 2))
        {
            if (hit.collider.tag == "Player")
                Physics.IgnoreCollision(hit.collider, m_collider.GetComponent<Collider>());

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
            else if(name == "TestBtn")
            {
                var obj = hit.collider.transform.gameObject;
                var test = obj.GetComponent<MainMenuTestButton>();
                if (!test.IsPlayingSound())
                {
                    test.PlayResponse();
                }
                else
                {

                }
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
            if(hit.collider.tag == "Interactable" || hit.collider.tag == "VRTKInteractTag")
            {
                m_isHolding = true;
                m_pickUpController.CurrentObject = hit.collider.gameObject;
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

    void ShowMainMenu()
    {
        m_menuOpen = true;
        m_currentMenu = Instantiate(m_menuPrefab);
        m_currentMenu.transform.Find("BG/Exit").GetComponent<Button>().onClick.AddListener(OnExit);

        if (SceneManager.GetActiveScene().name == "RadiantSide")
        {
            m_currentMenu.transform.Find("BG/Options").GetComponent<Button>().onClick.AddListener(OnOptions);
            m_currentMenu.transform.Find("BG/ManualSave").GetComponent<Button>().onClick.AddListener(OnSave);
        }
        else
        {
            m_currentMenu.transform.Find("BG/Options").gameObject.SetActive(false);
            m_currentMenu.transform.Find("BG/ManualSave").gameObject.SetActive(false);
        }

        m_currentMenu.transform.Find("BG/Resume").GetComponent<Button>().onClick.AddListener(OnResume);

        m_blurEffect = GameObject.Find("FirstPersonCharacterCamera").AddComponent<Blur>();
        m_blurEffect.blurShader = m_blurShader;
        m_blurEffect.enabled = true;

        GameObject.Find("EventSystem").GetComponent<CustomInputModule>().MenuIsOpen = true;
    }

    void OnExit()
    {
        if(SceneManager.GetActiveScene().name == "RadiantSide")
        {
            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().OnApplicationQuit();
            Application.Quit();
        }
        else
        {
            Application.Quit();
        }
    }

    void OnOptions()
    {
        transform.parent.transform.position = new Vector3(13.72374f, 0.8716383f, -0.8631723f);
        transform.parent.transform.rotation = new Quaternion(0f,0f,0f,0f); //wont work. Look in MouseLook.cs
        OnResume();
    }

    void OnResume()
    {
        if(m_currentMenu != null)
        {
            DestroyImmediate(m_currentMenu);
            Destroy(m_blurEffect);

            m_blurEffect = null;
            m_currentMenu = null;
            m_menuOpen = false;
        }

        GameObject.Find("EventSystem").GetComponent<CustomInputModule>().MenuIsOpen = false;
        GameObject.Find("FPSController").GetComponent<FirstPersonController>().m_MouseLook.Resume();
    }

    private void OnSave()
    {
        GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().SaveFile();
    }
}
