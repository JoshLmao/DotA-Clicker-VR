using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.VR;
using VRTK;
using UnityEngine.EventSystems;
using System.IO;
using Newtonsoft.Json;
using System;

public class MainMenuController : MonoBehaviour
{
    public GameObject[] HowToPlayMenus;
    public GameObject Rubick;
    public GameObject SideShop;
    public GameObject SecretShop;
    public GameObject Roshan;
    int menusPage = 0;

    [SerializeField]
    GameObject MainMenuCanvas;
    [SerializeField]
    GameObject OptionsCanvas;

    [SerializeField]
    GameObject[] FPSItems;

    [SerializeField]
    GameObject[] VRItems;

    [SerializeField]
    GameObject m_vrInputSystem;
    [SerializeField]
    GameObject m_fpsInputSystem;
    [SerializeField]
    VRTK_UICanvas[] m_menuCanvases;

    SaveFileDto m_saveFile;

    void Awake()
    {
        VRSettings.enabled = SteamVR.enabled ? SteamVR.active : false;
        SetPlayerMode(VRSettings.enabled);
    }

    void Start()
    {
        OnShowToMainMenu();
        DefaultHowToPlayMenu();

        GetPlayerNameFromFile();
    }

    void GetPlayerNameFromFile()
    {
        if (Directory.Exists(RadiantSceneController.FILE_PATHS))
        {
            string saveFile = RadiantSceneController.FILE_PATHS + RadiantSceneController.SAVE_FILE;
            if (File.Exists(saveFile))
            {
                try
                {
                    string json = File.ReadAllText(saveFile);
                    m_saveFile = JsonConvert.DeserializeObject<SaveFileDto>(json);
                    var kboard = GameObject.Find("Keyboard").GetComponent<KeyboardController>();
                    kboard.Input = m_saveFile.PlayerName;
                }
                catch (Exception e)
                {
                    Debug.Log("exc + " + e);

                }
            }
        }
    }

    void Update()
    {

    }

    public void OnQuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Close Application");
#else
        Application.Quit();
#endif
    }

    void DefaultHowToPlayMenu()
    {
        foreach (GameObject obj in HowToPlayMenus)
        {
            obj.SetActive(false);
        }

        //Set 1st screen active
        HowToPlayMenus[0].SetActive(true);
        SetHelpObjects();
    }

    public void OnLoadRadiantLevel()
    {
        SceneManager.LoadScene("RadiantSide");
    }

    public void NextPage()
    {
        if (menusPage >= HowToPlayMenus.Length - 1)
            return;


        HowToPlayMenus[menusPage].SetActive(false);

        menusPage++;
        HowToPlayMenus[menusPage].SetActive(true);

        SetHelpObjects();
    }

    public void PreviousPage()
    {
        if (menusPage <= 0)
            return;

        HowToPlayMenus[menusPage].SetActive(false);
        menusPage--;
        HowToPlayMenus[menusPage].SetActive(true);

        SetHelpObjects();
    }

    void SetHelpObjects()
    {
        //Secret Shop - Screen 1 or 2
        if (menusPage == 0 || menusPage == 1)
            Rubick.SetActive(true);
        else
            Rubick.SetActive(false);

        //Secret Shop - Screen 3
        if (menusPage == 2)
            SideShop.SetActive(true);
        else
            SideShop.SetActive(false);

        //Secret Shop - Screen 4
        if (menusPage == 3)
            SecretShop.SetActive(true);
        else
            SecretShop.SetActive(false);

        //Roshan - Screen 6
        if (menusPage == 5)
            Roshan.SetActive(true);
        else
            Roshan.SetActive(false);
    }

    public void OnShowToMainMenu()
    {
        MainMenuCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
    }
    public void OnShowOptionsMenu()
    {
        MainMenuCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
    }

    void SetPlayerMode(bool vrEnabled)
    {
        for (int i = 0; i < FPSItems.Length; i++)
        {
            FPSItems[i].SetActive(!vrEnabled);
        }

        for (int i = 0; i < VRItems.Length; i++)
        {
            VRItems[i].SetActive(vrEnabled);
        }

        //if(!vrEnabled)
        //{
        //    VRTK_UICanvas[] allCanvas = Object.FindObjectsOfType<VRTK_UICanvas>();
        //    for (int i = 0; i < allCanvas.Length; i++)
        //    {
        //        allCanvas[i].enabled = false;
        //    }
        //}

        m_vrInputSystem.SetActive(vrEnabled);
        m_fpsInputSystem.SetActive(!vrEnabled);

        SetUISystemActive(vrEnabled);
    }

    void SetUISystemActive(bool vrEnabled)
    {
        //Disable UIPointers & UICanvas which set eventsystem to work with VR
        GameObject.Find("LeftController").GetComponent<VRTK_UIPointer>().enabled = vrEnabled;
        GameObject.Find("RightController").GetComponent<VRTK_UIPointer>().enabled = vrEnabled;
        var allPointers = GameObject.FindObjectsOfType<VRTK_UIPointer>();
        for (int i = 0; i < allPointers.Length; i++)
        {
            allPointers[i].GetComponent<VRTK_UIPointer>().enabled = vrEnabled;
        }

        var allVRCanvas = FindObjectsOfType<VRTK_UICanvas>();
        for (int i = 0; i < allVRCanvas.Length; i++)
        {
            allVRCanvas[i].GetComponent<VRTK_UICanvas>().enabled = vrEnabled;
        }

        for (int i = 0; i < m_menuCanvases.Length; i++)
        {
            m_menuCanvases[i].enabled = vrEnabled;
        }

        if(!VRSettings.enabled)
        {
            var components = m_fpsInputSystem.GetComponents(typeof(Component));
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].GetType() == typeof(EventSystem))
                {
                    var events = components[i] as EventSystem; ;
                    events.enabled = true;
                    continue;
                }

                if (components[i].GetType() == typeof(CustomInputModule))
                {
                    var customInputs = components[i] as CustomInputModule;
                    customInputs.enabled = true;
                }

                if (components[i].GetType() == typeof(VRTK_EventSystemVRInput))
                {
                    var system = components[i] as VRTK_EventSystemVRInput;
                    system.enabled = false;
                }
            }
        }
    }
}
