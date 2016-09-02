using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AchievementsController : MonoBehaviour {

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
        AssignAchievementEvents();
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
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Click 500 times",
            Description = "Oh, so you're serious about this clicking thing?",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        }); Achievements.Add(new AchievementDto()
        {
            Name = "Click 1,000 times",
            Description = "Alright, ok. ",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        }); Achievements.Add(new AchievementDto()
        {
            Name = "Click 15,000 times",
            Description = "Now that right there is some dedication.",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        }); Achievements.Add(new AchievementDto()
        {
            Name = "Click 50,000 times",
            Description = "I'm speechless",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 625 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 6200 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 15,000 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 100,000 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Earn 1,000,000 gold",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "Buy A Manager",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy All Managers",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "Buy An Ability",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy All Abilities for a Hero",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy An Item",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Buy Each Item Once",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "Defeat Roshan Once",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Defeat Roshan 10 Times",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });

        Achievements.Add(new AchievementDto()
        {
            Name = "The Aegis is Mine",
            Description = "Use the aegis",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Cheese, Gromit!",
            Description = "You actually tried to eat the cheese. I congratulate you on your efforts",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
            IsSecretAchievement = false,
        });

        /*Secret Achievements*/
        Achievements.Add(new AchievementDto()
        {
            Name = "The Closest You'll Get To A Battle Cup",
            Description = "It's sad but true. Find all 4 battle cups that are hidden around the map",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.TheManTheMythTheLegend,
            IsSecretAchievement = true,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "When Did EG Throw Last?",
            Description = "Throw the EG logo off the map",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.WhenDidEGThrowLast,
            IsSecretAchievement = true,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "The Man, The Myth, The Legend",
            Description = "Name yourself 420BootyWizard",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.TheManTheMythTheLegend,
            IsSecretAchievement = true,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Dongs Out For Bulldog",
            Description = "Visit AdmiralBulldog's Twitch stream",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.TheManTheMythTheLegend,
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

    void AssignAchievementEvents()
    {
        m_achievementEvents.Earn625Gold.AddListener(OnEarn625Gold);
        m_achievementEvents.Earn6200Gold.AddListener(OnEarn6200Gold);
        m_achievementEvents.Earn15000Gold.AddListener(OnEarn15000Gold);
        m_achievementEvents.Earn100000Gold.AddListener(OnEarn100000Gold);
        m_achievementEvents.EarnMillionGold.AddListener(OnEarnMillionGold);

        m_achievementEvents.ClickOnce.AddListener(OnClickOnceAchievement);
        m_achievementEvents.ClickFiveHundred.AddListener(OnClickFiveHundred);
        m_achievementEvents.ClickThousand.AddListener(OnClickThousand);
        m_achievementEvents.ClickFifteenThousand.AddListener(OnClickFifteenThousand);
        m_achievementEvents.ClickFiftyThousand.AddListener(OnClickFiftyThousand);

        m_achievementEvents.BuyAManager.AddListener(OnBuyAManager);
        m_achievementEvents.BuyAllManagers.AddListener(OnBuyAllManagers);
        
        m_achievementEvents.BuyAnAbility.AddListener(OnBuyAnAbility);
        m_achievementEvents.BuyAllAbilitiesForAHero.AddListener(OnBuyAllAbilitiesForAHero);
        m_achievementEvents.BuyAllAbilities.AddListener(OnBuyAllAbilities);
        
        m_achievementEvents.BuyAnItem.AddListener(OnBuyAnItem);
        m_achievementEvents.BuyEachItemOnce.AddListener(OnBuyEachItemOnce);
        
        m_achievementEvents.DefeatRoshan.AddListener(OnDefeatRoshan);
        m_achievementEvents.DefeatRoshanTenTimes.AddListener(OnDefeatRoshanTenTimes);
        
        m_achievementEvents.TheAegisIsMine.AddListener(OnTheAegisIsMine);
        m_achievementEvents.CheeseGromit.AddListener(OnCheeseGromit);

        /*Secret Achievements*/
        m_achievementEvents.TheClosestYoullGetToABattleCup.AddListener(OnDongsOutForBulldog);
        m_achievementEvents.WhenDidEGThrowLast.AddListener(OnDongsOutForBulldog);
        m_achievementEvents.TheManTheMythTheLegend.AddListener(OnDongsOutForBulldog);
        m_achievementEvents.DongsOutForBulldog.AddListener(OnDongsOutForBulldog);
    }

    private void OnEarn625Gold()
    {
        m_sceneController.CurrentSaveFile.Achievements.Earn625Gold = true;
    }

    private void OnEarn6200Gold()
    {
        m_sceneController.CurrentSaveFile.Achievements.Earn6200Gold = true;
    }

    private void OnEarn15000Gold()
    {
        m_sceneController.CurrentSaveFile.Achievements.Earn15000Gold = true;
    }

    private void OnEarn100000Gold()
    {
        m_sceneController.CurrentSaveFile.Achievements.Earn100000Gold = true;
    }

    private void OnEarnMillionGold()
    {
        m_sceneController.CurrentSaveFile.Achievements.EarnMillionGold = true;
    }

    private void OnClickOnceAchievement()
    {
        m_sceneController.CurrentSaveFile.Achievements.ClickOnce = true;
    }

    private void OnClickFiveHundred()
    {
        m_sceneController.CurrentSaveFile.Achievements.ClickFiveHundred = true;
    }

    private void OnClickThousand()
    {
        m_sceneController.CurrentSaveFile.Achievements.ClickThousand = true;
    }

    private void OnClickFifteenThousand()
    {
        m_sceneController.CurrentSaveFile.Achievements.ClickFifteenThousand = true;
    }

    private void OnClickFiftyThousand()
    {
        m_sceneController.CurrentSaveFile.Achievements.ClickFiftyThousand = true;
    }

    private void OnBuyAManager()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyAManager = true;
    }

    private void OnBuyAllManagers()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyAllManagers = true;
    }

    private void OnBuyAnAbility()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyAnAbility = true;
    }

    private void OnBuyAllAbilitiesForAHero()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyAllAbilitiesForAHero = true;
    }

    private void OnBuyAllAbilities()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyAllManagers = true;
    }

    private void OnBuyAnItem()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyAnItem = true;
    }

    private void OnBuyEachItemOnce()
    {
        m_sceneController.CurrentSaveFile.Achievements.BuyEachItemOnce = true;
    }

    private void OnDefeatRoshan()
    {
        m_sceneController.CurrentSaveFile.Achievements.DefeatRoshan = true;
    }

    private void OnDefeatRoshanTenTimes()
    {
        m_sceneController.CurrentSaveFile.Achievements.DefeatRoshanTenTimes = true;
    }

    private void OnTheAegisIsMine()
    {
        m_sceneController.CurrentSaveFile.Achievements.TheAegisIsMine = true;
    }

    private void OnCheeseGromit()
    {
        m_sceneController.CurrentSaveFile.Achievements.CheeseGromit = true;
    }

    private void OnTCYGTABCA()
    {
        m_sceneController.CurrentSaveFile.Achievements.TheClosestYoullGetToABattleCup = true;
    }

    private void OnWDEGTL()
    {
        m_sceneController.CurrentSaveFile.Achievements.WhenDidEGThrowLast = true;
    }

    private void OnTMTMTL()
    {
        m_sceneController.CurrentSaveFile.Achievements.TheManTheMythTheLegend = true;
    }

    private void OnDongsOutForBulldog()
    {
        m_sceneController.CurrentSaveFile.Achievements.DongsOutForBulldog = true;
    }
}
