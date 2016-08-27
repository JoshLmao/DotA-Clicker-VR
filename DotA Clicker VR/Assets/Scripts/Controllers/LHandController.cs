using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LHandController : HandController {

    public GameObject GoldUI;

    RadiantSceneController m_sceneController;
    Text m_goldCountText;

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
	}
	
	void Update ()
    {
        m_goldCountText.text = m_sceneController.TotalGold.ToString();
    }

    void OnMenuButtonClicked(object sender, ClickedEventArgs e)
    {
        GoldUI.SetActive(!GoldUI.activeInHierarchy);
    }

    void OnMenuButtonUnclicked(object sender, ClickedEventArgs e)
    {
        //GoldUI.SetActive(false);
    }
}
