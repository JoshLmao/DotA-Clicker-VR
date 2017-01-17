using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AchievementsController : MonoBehaviour
{
    public List<AchievementDto> Achievements { get; set; }

    [SerializeField]
    GameObject UIItemPrefab; //For UI prefab

    RadiantSceneController m_sceneController;
    AchievementEvents m_achievementEvents;

    void Start()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_achievementEvents = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();

        Achievements = new List<AchievementDto>();
        AddDefaultItems();
        RefreshItemsList();

        //Todo: Listen to achievement events
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
        AchievementEvents.OnRefreshAchievementsList += RefreshItemsList;
    }

    void AddDefaultItems()
    {
        if (m_sceneController.CurrentSaveFile == null)
            Debug.Log("Current save file is null");

        //Add in any order ince they get sorted by name later
        Achievements.Add(new AchievementDto()
        {
            Name = "Click once",
            Description = "Wow. Such a hard achievement to get",
            Image = Resources.Load<Sprite>("Images/Achievements/Click1"),
            IsUnlocked = m_achievementEvents.ClickOnceStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Click 500 times",
            Description = "Oh, so you're serious about this clicking thing?",
            Image = Resources.Load<Sprite>("Images/Achievements/Click2"),
            IsUnlocked = m_achievementEvents.ClickFiveHundredStatus,
            IsSecretAchievement = false,
        }); Achievements.Add(new AchievementDto()
        {
            Name = "Click 1,000 times",
            Description = "Alright, ok. ",
            Image = Resources.Load<Sprite>("Images/Achievements/Click3"),
            IsUnlocked = m_achievementEvents.ClickThousandStatus,
            IsSecretAchievement = false,
        }); Achievements.Add(new AchievementDto()
        {
            Name = "Click 15,000 times",
            Description = "Now that right there is some dedication.",
            Image = Resources.Load<Sprite>("Images/Achievements/Click4"),
            IsUnlocked = m_achievementEvents.ClickFifteenThousandStatus,
            IsSecretAchievement = false,
        }); Achievements.Add(new AchievementDto()
        {
            Name = "Click 50,000 times",
            Description = "I'm speechless",
            Image = Resources.Load<Sprite>("Images/Achievements/Click5"),
            IsUnlocked = m_achievementEvents.ClickFiftyThousandStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 625 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/Achievements/Gold1"),
            IsUnlocked = m_achievementEvents.Earn625GoldStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 6200 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/Achievements/Gold2"),
            IsUnlocked = m_achievementEvents.Earn6200GoldStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 15,000 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/Achievements/Gold3"),
            IsUnlocked = m_achievementEvents.Earn15000GoldStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 100,000 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/Achievements/Gold4"),
            IsUnlocked = m_achievementEvents.Earn100000GoldStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 1,000,000 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/Achievements/Gold5"),
            IsUnlocked = m_achievementEvents.EarnMillionGoldStatus,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "Buy A Manager",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.BuyAManagerStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy All Managers",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.BuyAllManagersStatus,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "Buy An Ability",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.BuyAnAbilityStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy All Abilities for a Hero",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.BuyAllAbilitiesForAHeroStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy An Item",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.BuyAnItemStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy Each Item Once",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.BuyEachItenOnceStatus,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "Defeat Roshan Once",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.DefeatRoshanStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Defeat Roshan 10 Times",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.DefeatRoshanTenTimesStatus,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "The Aegis is Mine",
            Description = "Use the aegis",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.AegisIsMineStatus,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Cheese, Gromit!",
            Description = "You actually tried to eat the cheese. I congratulate you on your efforts",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.CheeseGromitStatus,
            IsSecretAchievement = false,
        });

        /*Secret Achievements*/
        Achievements.Add(new AchievementDto()
        {
            Name = "The Closest You'll Get To A Battle Cup",
            Description = "It's sad but true. Find all 4 battle cups that are hidden around the map",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.ClosestYoullGetStatus,
            IsSecretAchievement = true,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "When Did EG Throw Last?",
            Description = "Throw the EG logo off the map",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.EGThrowLastStatus,
            IsSecretAchievement = true,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "The Man, The Myth, The Legend",
            Description = "Name yourself 420BootyWizard",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_achievementEvents.ManMythLegendStatus,
            IsSecretAchievement = true,
        });
    }

    void RefreshItemsList()
    { 
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        //Achievements.Sort((x, y) => string.Compare(x.Name, y.Name)); //Sort list into alphabetical Order

        int addY = -30; //Hack to get positioning of List UI
        foreach (AchievementDto achievement in Achievements)
        {
            GameObject newUpgrade = GameObject.Instantiate(UIItemPrefab);
            newUpgrade.transform.SetParent(this.transform, false);
            newUpgrade.name = achievement.Name;

            newUpgrade.GetComponent<RectTransform>().localPosition = new Vector3(newUpgrade.transform.localPosition.x, addY, newUpgrade.transform.localPosition.z);
            addY -= 150; //Space between each UI

            Image icon = newUpgrade.transform.Find("Image").GetComponent<Image>();
            icon.sprite = achievement.Image;
            Image locked = newUpgrade.transform.Find("ImageLocked").GetComponent<Image>();
            locked.fillAmount = achievement.IsUnlocked == true ? 0 : 1;

            Text upgradeName = newUpgrade.transform.Find("Name").GetComponent<Text>();
            upgradeName.text = achievement.IsSecretAchievement ? "???" : achievement.Name;
            upgradeName.color = achievement.IsUnlocked == true ? Color.white : Color.gray;

            Text description = newUpgrade.transform.Find("Desc").GetComponent<Text>();
            description.text = achievement.IsUnlocked ? achievement.Description : "???";
            description.color = achievement.IsUnlocked == true ? Color.white : Color.gray;
        }
    }

    void OnLoadedSaveFile()
    {
        AddDefaultItems();
        RefreshItemsList();
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {

    }
}
