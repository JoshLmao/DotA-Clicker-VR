using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AchievementEvents : MonoBehaviour
{
    RadiantSceneController m_sceneController;

    public delegate void RefreshAchievementsList();
    public static event RefreshAchievementsList OnRefreshAchievementsList;

    public UnityEvent Earn625Gold = new UnityEvent();
    public bool Earn625GoldStatus;
    public UnityEvent Earn6200Gold = new UnityEvent();
    public bool Earn6200GoldStatus;
    public UnityEvent Earn15000Gold = new UnityEvent();
    public bool Earn15000GoldStatus;
    public UnityEvent Earn100000Gold = new UnityEvent();
    public bool Earn100000GoldStatus;
    public UnityEvent EarnMillionGold = new UnityEvent();
    public bool EarnMillionGoldStatus;

    public UnityEvent ClickOnce = new UnityEvent();
    public bool ClickOnceStatus;
    public UnityEvent ClickFiveHundred = new UnityEvent();
    public bool ClickFiveHundredStatus;
    public UnityEvent ClickThousand = new UnityEvent();
    public bool ClickThousandStatus;
    public UnityEvent ClickFifteenThousand = new UnityEvent();
    public bool ClickFifteenThousandStatus;
    public UnityEvent ClickFiftyThousand = new UnityEvent();
    public bool ClickFiftyThousandStatus;

    public UnityEvent BuyAManager = new UnityEvent();
    public bool BuyAManagerStatus;
    public UnityEvent BuyAllManagers = new UnityEvent();
    public bool BuyAllManagersStatus;

    public UnityEvent BuyAnAbility = new UnityEvent();
    public bool BuyAnAbilityStatus;
    public UnityEvent BuyAllAbilitiesForAHero = new UnityEvent();
    public bool BuyAllAbilitiesForAHeroStatus;
    public UnityEvent BuyAllAbilities = new UnityEvent();
    public bool BuyAllAbilitiesStatus;

    public UnityEvent BuyAnItem = new UnityEvent();
    public bool BuyAnItemStatus;
    public UnityEvent BuyEachItemOnce = new UnityEvent();
    public bool BuyEachItenOnceStatus;

    public UnityEvent DefeatRoshan = new UnityEvent();
    public bool DefeatRoshanStatus;
    public UnityEvent DefeatRoshanTenTimes = new UnityEvent();
    public bool DefeatRoshanTenTimesStatus;

    public UnityEvent TheAegisIsMine = new UnityEvent();
    public bool AegisIsMineStatus;
    public UnityEvent CheeseGromit = new UnityEvent();
    public bool CheeseGromitStatus;

    /*Secret Achievements*/
    public UnityEvent TheClosestYoullGetToABattleCup = new UnityEvent();
    public bool ClosestYoullGetStatus;
    public UnityEvent WhenDidEGThrowLast = new UnityEvent();
    public bool EGThrowLastStatus;
    public UnityEvent TheManTheMythTheLegend = new UnityEvent();
    public bool ManMythLegendStatus;
    public UnityEvent DongsOutForBulldog = new UnityEvent();
    public bool DongsOutStatus;

    void Start()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        Earn625Gold.AddListener(Earn625GoldMethod);
        Earn6200Gold.AddListener(Earn6200GoldMethod);
        Earn15000Gold.AddListener(Earn15000GoldMethod);
        Earn100000Gold.AddListener(Earn100000GoldMethod);
        EarnMillionGold.AddListener(EarnMillionGoldMethod);
        ClickOnce.AddListener(ClickOnceMethod);
        ClickFiveHundred.AddListener(ClickFiveHundredMethod);
        ClickThousand.AddListener(ClickThousandMethod);
        ClickFifteenThousand.AddListener(ClickFifteenThousandMethod);
        ClickFiftyThousand.AddListener(ClickFiftyThousandMethod);
        BuyAnAbility.AddListener(BuyAnAbilityMethod);
        BuyAllAbilitiesForAHero.AddListener(BuyAllAbilitiesForHeroMethod);
        BuyAllAbilities.AddListener(BuyAllAbilitiesMethod);
        BuyAManager.AddListener(BuyAManagerMethod);
        BuyAllManagers.AddListener(BuyAllManagersMethod);
        BuyAnItem.AddListener(BuyAnItemMethod);
        BuyEachItemOnce.AddListener(BuyEachItemsOnceMethod);
        DefeatRoshan.AddListener(DefeatRoshanMethod);
        DefeatRoshanTenTimes.AddListener(DefeatRoshanTenTimesMethod);
        TheAegisIsMine.AddListener(TheAegisIsMineMethod);
        CheeseGromit.AddListener(CheeseGromitMethod);
        TheClosestYoullGetToABattleCup.AddListener(ClosestYoullGetMethod);
        WhenDidEGThrowLast.AddListener(EGThrowLastMethod);
        TheManTheMythTheLegend.AddListener(ManMythLegendMethod);
        DongsOutForBulldog.AddListener(DongsOutMethod);

        /*This code is the noise sheep make, with a 'd' on the end. Buy I dont care since no one will read this, right?*/
        if (m_sceneController.CurrentSaveFile != null)
        {
            Earn625GoldStatus = m_sceneController.CurrentSaveFile.Achievements.Earn625Gold;
            Earn6200GoldStatus = m_sceneController.CurrentSaveFile.Achievements.Earn625Gold;
            Earn15000GoldStatus = m_sceneController.CurrentSaveFile.Achievements.Earn15000Gold;
            Earn100000GoldStatus = m_sceneController.CurrentSaveFile.Achievements.Earn100000Gold;
            EarnMillionGoldStatus = m_sceneController.CurrentSaveFile.Achievements.EarnMillionGold;
            ClickOnceStatus = m_sceneController.CurrentSaveFile.Achievements.ClickOnce;
            ClickFiveHundredStatus = m_sceneController.CurrentSaveFile.Achievements.ClickFiveHundred;
            ClickThousandStatus = m_sceneController.CurrentSaveFile.Achievements.ClickThousand;
            ClickFifteenThousandStatus = m_sceneController.CurrentSaveFile.Achievements.ClickFifteenThousand;
            ClickFiftyThousandStatus = m_sceneController.CurrentSaveFile.Achievements.ClickFiftyThousand;
            BuyAnAbilityStatus = m_sceneController.CurrentSaveFile.Achievements.BuyAnAbility;
            BuyAllAbilitiesStatus = m_sceneController.CurrentSaveFile.Achievements.BuyAllManagers;
            BuyAllAbilitiesForAHeroStatus = m_sceneController.CurrentSaveFile.Achievements.BuyAllAbilitiesForAHero;
            BuyAManagerStatus = m_sceneController.CurrentSaveFile.Achievements.BuyAManager;
            BuyAllManagersStatus = m_sceneController.CurrentSaveFile.Achievements.BuyAllManagers;
            BuyAnItemStatus = m_sceneController.CurrentSaveFile.Achievements.BuyAnItem;
            BuyEachItenOnceStatus = m_sceneController.CurrentSaveFile.Achievements.BuyEachItemOnce;
            DefeatRoshanStatus = m_sceneController.CurrentSaveFile.Achievements.DefeatRoshan;
            DefeatRoshanTenTimesStatus = m_sceneController.CurrentSaveFile.Achievements.DefeatRoshanTenTimes;
            AegisIsMineStatus = m_sceneController.CurrentSaveFile.Achievements.TheAegisIsMine;
            CheeseGromitStatus = m_sceneController.CurrentSaveFile.Achievements.CheeseGromit;
            ClosestYoullGetStatus = m_sceneController.CurrentSaveFile.Achievements.TheClosestYoullGetToABattleCup;
            EGThrowLastStatus = m_sceneController.CurrentSaveFile.Achievements.WhenDidEGThrowLast;
            ManMythLegendStatus = m_sceneController.CurrentSaveFile.Achievements.TheManTheMythTheLegend;
            DongsOutStatus = m_sceneController.CurrentSaveFile.Achievements.DongsOutForBulldog;
        }
        else
        {
            Earn625GoldStatus = false;
            Earn6200GoldStatus = false;
            Earn15000GoldStatus = false;
            Earn100000GoldStatus = false;
            EarnMillionGoldStatus = false;
            ClickOnceStatus = false;
            ClickFiveHundredStatus = false;
            ClickThousandStatus = false;
            ClickFifteenThousandStatus = false;
            ClickFiftyThousandStatus = false;
            BuyAnAbilityStatus = false;
            BuyAllAbilitiesStatus = false;
            BuyAllAbilitiesForAHeroStatus = false;
            BuyAManagerStatus = false;
            BuyAllManagersStatus = false;
            BuyAnItemStatus = false;
            BuyEachItenOnceStatus = false;
            DefeatRoshanStatus = false;
            DefeatRoshanTenTimesStatus = false;
            AegisIsMineStatus = false;
            CheeseGromitStatus = false;
            ClosestYoullGetStatus = false;
            EGThrowLastStatus = false;
            ManMythLegendStatus = false;
            DongsOutStatus = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Earn625GoldMethod();
        }
    }

    void Earn625GoldMethod()
    {
        Earn625GoldStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void Earn6200GoldMethod()
    {
        Earn6200GoldStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void Earn15000GoldMethod()
    {
        Earn15000GoldStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void Earn100000GoldMethod()
    {
        Earn100000GoldStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void EarnMillionGoldMethod()
    {
        EarnMillionGoldStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ClickOnceMethod()
    {
        ClickOnceStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ClickFiveHundredMethod()
    {
        ClickFiveHundredStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ClickThousandMethod()
    {
        ClickThousandStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ClickFifteenThousandMethod()
    {
        ClickFifteenThousandStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ClickFiftyThousandMethod()
    {
        ClickFiftyThousandStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyAnAbilityMethod()
    {
        BuyAnAbilityStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyAllAbilitiesMethod()
    {
        BuyAllAbilitiesStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyAllAbilitiesForHeroMethod()
    {
        BuyAllAbilitiesForAHeroStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyAManagerMethod()
    {
        BuyAManagerStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyAllManagersMethod()
    {
        BuyAllManagersStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyAnItemMethod()
    {
        BuyAnItemStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void BuyEachItemsOnceMethod()
    {
        BuyEachItenOnceStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void DefeatRoshanMethod()
    {
        DefeatRoshanStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void DefeatRoshanTenTimesMethod()
    {
        DefeatRoshanTenTimesStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void TheAegisIsMineMethod()
    {
        AegisIsMineStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void CheeseGromitMethod()
    {
        CheeseGromitStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ClosestYoullGetMethod()
    {
        ClosestYoullGetStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void EGThrowLastMethod()
    {
        EGThrowLastStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void ManMythLegendMethod()
    {
        ManMythLegendStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
    void DongsOutMethod()
    {
        DongsOutStatus = true;
        OnRefreshAchievementsList.Invoke();
    }
}
