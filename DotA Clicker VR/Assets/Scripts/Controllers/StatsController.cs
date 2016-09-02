using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatsController : MonoBehaviour {

    RadiantSceneController m_sceneController;

    Text m_totalTimePlayedText;
    Text m_totalClicksText;

    Text m_ironBranch, m_clarity, m_magicStick, m_mangos, m_quellingBlade, m_powerTreads;
    Text m_bottles, m_blinkDaggers, m_hyperstones, m_bloodstones, m_reavers, m_divineRapiers, m_recipes;

    GameObject m_recipesUI;

    void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        m_totalTimePlayedText = transform.Find("GeneralStats/TotalTime/TTText").GetComponent<Text>();
        m_totalClicksText = transform.Find("GeneralStats/TotalClicks/TCText").GetComponent<Text>();

        m_ironBranch = transform.Find("ItemsBought/IronBranchCount/Count").GetComponent<Text>();
        m_clarity = transform.Find("ItemsBought/ClarityCount/Count").GetComponent<Text>();
        m_magicStick = transform.Find("ItemsBought/MagicStickCount/Count").GetComponent<Text>();
        m_mangos = transform.Find("ItemsBought/MangoCount/Count").GetComponent<Text>();
        m_quellingBlade = transform.Find("ItemsBought/QuellingBladeCount/Count").GetComponent<Text>();
        m_powerTreads = transform.Find("ItemsBought/PowerTreadsCount/Count").GetComponent<Text>();
        m_bottles = transform.Find("ItemsBought/BottleCount/Count").GetComponent<Text>();
        m_blinkDaggers= transform.Find("ItemsBought/BlinkDaggerCount/Count").GetComponent<Text>();
        m_hyperstones = transform.Find("ItemsBought/HyperstoneCount/Count").GetComponent<Text>();
        m_bloodstones = transform.Find("ItemsBought/BloodstoneCount/Count").GetComponent<Text>();
        m_reavers = transform.Find("ItemsBought/ReaverCount/Count").GetComponent<Text>();
        m_divineRapiers = transform.Find("ItemsBought/DivineRapiersCount/Count").GetComponent<Text>();
        m_recipesUI = transform.Find("ItemsBought/RecipesUI").gameObject;
        m_recipes = transform.Find("ItemsBought/RecipesUI/RecipesCount").GetComponent<Text>();
    }
	
	void Update ()
    {
        //Live Play Time
        TimeSpan t = TimeSpan.FromSeconds(m_sceneController.CurrentSaveFile.SessionStats.TotalPlayTime + Time.realtimeSinceStartup);
        m_totalTimePlayedText.text = string.Format("{0:D1} days, \n{1:D2}:{2:D2}:{3:D2}", t.Days, t.Hours, t.Minutes, t.Seconds);

        //Live Click Count
        m_totalClicksText.text = m_sceneController.ClickCount.ToString() + " clicks";

        UpdateItemsCountUI();
    }

    void UpdateItemsCountUI()
    {
        BuyableItemsController itemsController = GameObject.Find("SceneEnvironment/SideShop/ItemsCanvas/ItemsScrollable/ItemsListCanvas").GetComponent<BuyableItemsController>();

        m_ironBranch.text = itemsController.IronBranchCount.ToString();
        m_clarity.text = itemsController.ClarityCount.ToString();
        m_magicStick.text = itemsController.MagicStickCount.ToString();
        m_mangos.text = itemsController.MangoCount.ToString();
        m_quellingBlade.text = itemsController.QuellingBladeCount.ToString();
        m_powerTreads.text = itemsController.PowerTreadsCount.ToString();
        m_bottles.text = itemsController.BottleCount.ToString();
        m_blinkDaggers.text = itemsController.BlinkDaggerCount.ToString();
        m_hyperstones.text = itemsController.HyperstoneCount.ToString();
        m_bloodstones.text = itemsController.BloodstoneCount.ToString();
        m_reavers.text = itemsController.ReaverCount.ToString();
        m_divineRapiers.text = itemsController.DivineRapierCount.ToString();

        if(itemsController.IronBranchCount >= 1)
        {
            m_recipesUI.SetActive(true);
            m_reavers.text = itemsController.RecipeCount.ToString();
        }
        else
        {
            m_recipesUI.SetActive(false);
        }
    }

}
