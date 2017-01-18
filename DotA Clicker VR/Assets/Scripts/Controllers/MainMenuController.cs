using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

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

	void Start ()
    {
        DefaultHowToPlayMenu();
        OnShowToMainMenu();
    }
	
	void Update ()
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
        if (menusPage > HowToPlayMenus.Length)
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
}
