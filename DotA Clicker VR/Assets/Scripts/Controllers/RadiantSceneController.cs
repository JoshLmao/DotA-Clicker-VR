using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RadiantSceneController : MonoBehaviour
{    
    public List<HeroClicker> SceneHeroes { get; set; }

    public int TotalGold = 0;

    Text m_goldUI;

	void Start ()
    {
        LoadProgress();
        m_goldUI = GameObject.Find("TotalGoldText").GetComponent<Text>();
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

    void CreateHeroes()
    {
        //SceneHeroes.Add(new HeroClicker()
        //{
        //    Name = "Io",
        //    GoldPerClick
        //});
    }
}
