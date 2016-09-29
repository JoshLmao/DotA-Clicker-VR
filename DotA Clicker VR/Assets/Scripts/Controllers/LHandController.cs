using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LHandController : HandController {

    public GameObject GoldUI;

    bool m_menuIsOpen = false;
    bool m_canClickMenuButtons = false;
    bool m_onSettingsBtn = false;
    bool m_onMainMenuBtn = false;
    RadiantSceneController m_sceneController;
    Text m_goldCountText;

    SteamVR_Controller.Device m_device;

    void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        if (m_controller == null)
            m_controller = this.GetComponent<SteamVR_TrackedController>();

        m_goldCountText = transform.Find("TotalGoldMiniMenu/TotalGold/TotalGoldText").GetComponent<Text>();
        GoldUI = transform.Find("TotalGoldMiniMenu").gameObject;
        GoldUI.SetActive(false);

        m_controller.MenuButtonClicked += OnMenuButtonClicked;
        m_controller.MenuButtonUnclicked += OnMenuButtonUnclicked;

        m_controller.PadClicked += OnPadClicked;
	}

    void Update ()
    {
        m_goldCountText.text = m_sceneController.TotalGold.ToString();

        if(m_menuIsOpen)
        {
            m_device = SteamVR_Controller.Input((int)m_controller.controllerIndex);
            if(m_device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                var touchpad = m_device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

                if(touchpad.y < 0 && touchpad.x < 0) //Bottom Left
                {
                    m_onSettingsBtn = true;
                    m_onMainMenuBtn = false;

                    //Disable teleporter

                    m_canClickMenuButtons = true;
                    
                }
                else if(touchpad.y < 0 && touchpad.x > 0)//Buttom Right
                {
                    m_onSettingsBtn = false;
                    m_onMainMenuBtn = true;

                    //Disable teleporter

                    m_canClickMenuButtons = true;
                }
            }
        }
    }

    void OnMenuButtonClicked(object sender, ClickedEventArgs e)
    {
        GoldUI.SetActive(!GoldUI.activeInHierarchy);
        m_menuIsOpen = GoldUI.activeInHierarchy;
    }

    void OnMenuButtonUnclicked(object sender, ClickedEventArgs e)
    {
        //GoldUI.SetActive(false);
    }

    private void OnPadClicked(object sender, ClickedEventArgs e)
    {
        if(m_canClickMenuButtons)
        {
            if(m_onSettingsBtn)
            {

            }
            else if(m_onMainMenuBtn)
            {

            }
        }
    }
}
