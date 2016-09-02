using UnityEngine;
using System.Collections;

public class AchievementEvents : MonoBehaviour
{
    public delegate void Earn625Gold();
    public static event Earn625Gold Earn625GoldAchievement;
    public delegate void Earn6200Gold();
    public static event Earn6200Gold Earn6200GoldAchievement;
    public delegate void Earn15000Gold();
    public static event Earn15000Gold Earn15000GoldAchievement;
    public delegate void Earn100000Gold();
    public static event Earn100000Gold Earn100000GoldAchievement;
    public delegate void EarnMillionGold();
    public static event EarnMillionGold EarnMillionGoldAchievement;

    public delegate void ClickOnce();
    public static event ClickOnce ClickOnceAchievement;
    public delegate void ClickFiveHundred();
    public static event ClickFiveHundred ClickFiveHundredAchievement;
    public delegate void ClickThousand();
    public static event ClickThousand ClickThousandAchievement;
    public delegate void ClickFifteenThousand();
    public static event ClickFifteenThousand ClickFifteenThousandAchievement;
    public delegate void ClickFiftyThousand();
    public static event ClickFiftyThousand ClickFiftyThousandAchievement;

    public delegate void BuyAManager();
    public static event BuyAManager BuyAManagerAchievement;
    public delegate void BuyAllManagers();
    public static event BuyAllManagers BuyAllManagersAchievement;

    public delegate void BuyAnAbility();
    public static event BuyAnAbility BuyAnAbilityAchievement;
    public delegate void BuyAllAbilitiesForAHero();
    public static event BuyAllAbilitiesForAHero BuyAllAbilitiesForAHeroAchievement;
    public delegate void BuyAllAbilities();
    public static event BuyAllAbilities BuyAllAbilitiesAchievement;

    public delegate void BuyAnItem();
    public static event BuyAnItem BuyAnItemAchievement;
    public delegate void BuyEachItemOnce();
    public static event BuyEachItemOnce BuyEachItemOnceAchievement;

    public delegate void DefeatRoshan();
    public static event DefeatRoshan DefeatRoshanAchievement;
    public delegate void DefeatRoshanTenTimes();
    public static event DefeatRoshanTenTimes DefeatRoshanTenTimesAchievement;

    public delegate void TheAegisIsMine();
    public static event TheAegisIsMine TheAegisIsMineAchievement;
    public delegate void CheeseGromit();
    public static event CheeseGromit CheeseGromitAchievement;

    /*Secret Achievements*/
    public delegate void TheClosestYoullGetToABattleCup();
    public static event TheClosestYoullGetToABattleCup TheClosestYoullGetToABattleCupAchievement;

    public delegate void WhenDidEGThrowLast();
    public static event WhenDidEGThrowLast WhenDidEGThrowLastAchievement;

    public delegate void TheManTheMythTheLegend();
    public static event TheManTheMythTheLegend TheManTheMythTheLegendAchievement;

    public delegate void DongsOutForBulldog();
    public static event DongsOutForBulldog DongsOutForBulldogAchievement;
}
