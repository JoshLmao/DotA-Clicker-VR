using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RadiantSceneController : MonoBehaviour
{
    public List<RadiantClickerController> SceneHeroes;

    public int TotalGold = 3000;

    public const string THOUSAND_FORMAT = "{0}, {1}, {2}";
    public const string MILLION_FORMAT = "{0} million";
    public const string BILLION_FORMAT = "{0} billion";
    public const string TRILLION_FORMAT = "{0} trillion";
    public const string QUADRILLION_FORMAT = "{0} quadrillion";

    Text m_goldUI;

	void Start ()
    {
        LoadProgress();
        m_goldUI = GameObject.Find("TotalGoldText").GetComponent<Text>();

        SceneHeroes = GetClickerHeroesInScene();
	}

	void Update ()
    {
        m_goldUI.text = TotalGold.ToString();
	}

    void FixedUpdate()
    {

    }

    public void LoadProgress()
    {
        
    }

    public List<RadiantClickerController> GetClickerHeroesInScene()
    {
        List<RadiantClickerController> listClickers = new List<RadiantClickerController>();
        RadiantClickerController[] clickers = FindObjectsOfType<RadiantClickerController>();
        foreach(RadiantClickerController clicker in clickers)
        {
            listClickers.Add(clicker);
        }
        return listClickers;
    }
}
