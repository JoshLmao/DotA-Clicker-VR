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

    Text m_overchargeCount, m_relocateCount, m_telekinesisCount, m_spellStealCount, m_fireblastCount, m_bloodlustCount, m_snowballCount, m_walrusPunchCount;
    Text m_sunrayCount, m_supernovaCount, m_warCryCount, m_godsStrengthCount, m_blinkCount, m_manaVoidCount, m_greevilsGreedCount, m_chemicalRageCount;

    void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        m_totalTimePlayedText = transform.Find("StatsScrollable/StatsListCanvas/GeneralStats/TotalTime/TTText").GetComponent<Text>();
        m_totalClicksText = transform.Find("StatsScrollable/StatsListCanvas/GeneralStats/TotalClicks/TCText").GetComponent<Text>();

        m_ironBranch = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/IronBranchCount/Count").GetComponent<Text>();
        m_clarity = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/ClarityCount/Count").GetComponent<Text>();
        m_magicStick = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/MagicStickCount/Count").GetComponent<Text>();
        m_mangos = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/MangoCount/Count").GetComponent<Text>();
        m_quellingBlade = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/QuellingBladeCount/Count").GetComponent<Text>();
        m_powerTreads = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/PowerTreadsCount/Count").GetComponent<Text>();
        m_bottles = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/BottleCount/Count").GetComponent<Text>();
        m_blinkDaggers= transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/BlinkDaggerCount/Count").GetComponent<Text>();
        m_hyperstones = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/HyperstoneCount/Count").GetComponent<Text>();
        m_bloodstones = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/BloodstoneCount/Count").GetComponent<Text>();
        m_reavers = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/ReaverCount/Count").GetComponent<Text>();
        m_divineRapiers = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/DivineRapiersCount/Count").GetComponent<Text>();
        m_recipesUI = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/RecipesUI").gameObject;
        m_recipes = transform.Find("StatsScrollable/StatsListCanvas/ItemsBought/RecipesUI/RecipesCount").GetComponent<Text>();

        m_overchargeCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Io/Abil1/Count").GetComponent<Text>();
        m_relocateCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Io/Abil2/Count").GetComponent<Text>();
        m_telekinesisCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Rubick/Abil1/Count").GetComponent<Text>();
        m_spellStealCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Rubick/Abil2/Count").GetComponent<Text>();
        m_fireblastCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/OgreMagi/Abil1/Count").GetComponent<Text>();
        m_bloodlustCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/OgreMagi/Abil2/Count").GetComponent<Text>();
        m_snowballCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Tusk/Abil1/Count").GetComponent<Text>();
        m_walrusPunchCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Tusk/Abil2/Count").GetComponent<Text>();
        m_sunrayCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Phoenix/Abil1/Count").GetComponent<Text>();
        m_supernovaCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Phoenix/Abil2/Count").GetComponent<Text>();
        m_warCryCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Sven/Abil1/Count").GetComponent<Text>();
        m_godsStrengthCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Sven/Abil2/Count").GetComponent<Text>();
        m_blinkCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/AntiMage/Abil1/Count").GetComponent<Text>();
        m_manaVoidCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/AntiMage/Abil2/Count").GetComponent<Text>();
        m_greevilsGreedCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Alchemist/Abil1/Count").GetComponent<Text>();
        m_chemicalRageCount = transform.Find("StatsScrollable/StatsListCanvas/HeroStats/Alchemist/Abil2/Count").GetComponent<Text>();
    }
	
	void Update ()
    {
        //Live Play Time
        TimeSpan t = TimeSpan.FromSeconds(m_sceneController.CurrentSaveFile.SessionStats.TotalPlayTime + Time.realtimeSinceStartup);
        m_totalTimePlayedText.text = string.Format("{0:D1} days, \n{1:D2}:{2:D2}:{3:D2}", t.Days, t.Hours, t.Minutes, t.Seconds);

        //Live Click Count
        m_totalClicksText.text = m_sceneController.ClickCount.ToString() + " clicks";

        UpdateItemsCountUI();
        UpdateAbilityCountUI();
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

    void UpdateAbilityCountUI()
    {
        /* Hero Order in List: 0 = Alchemist, 1 = Ogre, 2 = Tusk, 3 = Io, 4 = AntiMagi, 5 = Sven, 6 = Phoenix, 7 = Rubick */
        m_overchargeCount.text = m_sceneController.SceneHeroes[3].Ability1UseCount.ToString();
        m_relocateCount.text = m_sceneController.SceneHeroes[3].Ability2UseCount.ToString();
        m_telekinesisCount.text = m_sceneController.SceneHeroes[7].Ability1UseCount.ToString();
        m_spellStealCount.text = m_sceneController.SceneHeroes[7].Ability2UseCount.ToString();
        m_fireblastCount.text = m_sceneController.SceneHeroes[2].Ability1UseCount.ToString();
        m_bloodlustCount.text = m_sceneController.SceneHeroes[2].Ability2UseCount.ToString();
        m_sunrayCount.text = m_sceneController.SceneHeroes[6].Ability1UseCount.ToString();
        m_supernovaCount.text = m_sceneController.SceneHeroes[6].Ability2UseCount.ToString();
        m_warCryCount.text = m_sceneController.SceneHeroes[5].Ability1UseCount.ToString();
        m_godsStrengthCount.text = m_sceneController.SceneHeroes[5].Ability2UseCount.ToString();
        m_blinkCount.text = m_sceneController.SceneHeroes[4].Ability1UseCount.ToString();
        m_manaVoidCount.text = m_sceneController.SceneHeroes[4].Ability2UseCount.ToString();
        m_greevilsGreedCount.text = m_sceneController.SceneHeroes[0].Ability1UseCount.ToString();
        m_chemicalRageCount.text = m_sceneController.SceneHeroes[0].Ability2UseCount.ToString();
    }
}
