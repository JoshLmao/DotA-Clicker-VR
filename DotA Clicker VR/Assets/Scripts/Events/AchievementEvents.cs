using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AchievementEvents : MonoBehaviour
{
    public UnityEvent Earn625Gold = new UnityEvent();
    public UnityEvent Earn6200Gold = new UnityEvent();
    public UnityEvent Earn15000Gold = new UnityEvent();
    public UnityEvent Earn100000Gold = new UnityEvent();
    public UnityEvent EarnMillionGold = new UnityEvent();

    public UnityEvent ClickOnce = new UnityEvent();
    public UnityEvent ClickFiveHundred = new UnityEvent();
    public UnityEvent ClickThousand = new UnityEvent();
    public UnityEvent ClickFifteenThousand = new UnityEvent();
    public UnityEvent ClickFiftyThousand = new UnityEvent();

    public UnityEvent BuyAManager = new UnityEvent();
    public UnityEvent BuyAllManagers = new UnityEvent();

    public UnityEvent BuyAnAbility = new UnityEvent();
    public UnityEvent BuyAllAbilitiesForAHero = new UnityEvent();
    public UnityEvent BuyAllAbilities = new UnityEvent();

    public UnityEvent BuyAnItem = new UnityEvent();
    public UnityEvent BuyEachItemOnce = new UnityEvent();

    public UnityEvent DefeatRoshan = new UnityEvent();
    public UnityEvent DefeatRoshanTenTimes = new UnityEvent();

    public UnityEvent TheAegisIsMine = new UnityEvent();
    public UnityEvent CheeseGromit = new UnityEvent();

    /*Secret Achievements*/
    public UnityEvent TheClosestYoullGetToABattleCup = new UnityEvent();
    public UnityEvent WhenDidEGThrowLast = new UnityEvent();
    public UnityEvent TheManTheMythTheLegend = new UnityEvent();
    public UnityEvent DongsOutForBulldog = new UnityEvent();
}
