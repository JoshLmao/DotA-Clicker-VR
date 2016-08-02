using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Generic class aimed at being used by each clicker. Controls times and multipliers
/// </summary>
public class RadiantClickerController : MonoBehaviour {

    public string ClickerHeroName;
    public int ClickAmount = 1;
    public int BuyNextLevel;
    /// <summary>
    /// Start time it takes to complete one click. 
    /// </summary>
    public DateTime TimeBetweenClicks; //String.Format(TimeBetweenCLicks, HH/MM/SS);
    /// <summary>
    /// Current time the timer is at. Used in displays
    /// </summary>
    public DateTime CurrentClickerTime;
    /// <summary>
    /// Amount of times the clicker has been bought
    /// </summary>
    public int ClickerMultiplier = 0;

    private RadiantSceneController m_sceneController;

    private Text m_timeRemainingText;
    private Text m_amountBoughtText;
    private Text m_clickButtonGoldText;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_timeRemainingText = transform.FindChild("Buttons/StandBack/StandUI/TimeRemaining").GetComponent<Text>();
        m_amountBoughtText = transform.Find("Buttons/StandBack/StandUI/AmountCanvas/AmountText").GetComponent<Text>();
        m_clickButtonGoldText = transform.Find("Buttons/ClickButtonBack/ClickButton/ClickUI/ClickWorthText").GetComponent<Text>();

        ButtonManager.OnBuyClickerPressed += OnBuyClickerButtonPressed;
        ButtonManager.OnHeroClickButtonPress += OnClickButtonPressed;
	}
	
	void Update ()
    {
        UpdateCountdownTimer();
        UpdateUIText();
	}

    public void UpdateCountdownTimer()
    {
        //m_currentTime;
    }

    void OnBuyClickerButtonPressed()
    {
        //Adding of multiplier is done by Button Manager
    }

    void UpdateUIText()
    {
        m_clickButtonGoldText.text = ClickAmount + " gold";
        m_amountBoughtText.text = ClickerMultiplier.ToString();
    }

    void OnClickButtonPressed()
    {
        //Adding of gold is done by Button Manager
    }

    public void AddGold()
    {
        m_sceneController.TotalGold += (ClickAmount * ClickerMultiplier);
    }
}
