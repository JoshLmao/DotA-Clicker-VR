using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Generic class aimed at being used by each clicker. Controls times and multipliers
/// </summary>
public class RadiantClickerController : MonoBehaviour
{
    /**  Clicker Details & Start Values**/
    public string HeroName;
    public int StartClickAmount = 1;
    public float UpgradeCost = 10;
    public float UpgradeMultiplier = 1.15f;
    /****************************/
    /// <summary>
    /// Current amount the clicker will give if clicked
    /// </summary>
    public int ClickAmount { get { return StartClickAmount * ClickerMultiplier; } }
    /// <summary>
    /// Amount of times the clicker has been bought
    /// </summary>
    public int ClickerMultiplier = 0;
    /// <summary>
    /// Start time it takes to complete one click. 
    /// </summary>
    public TimeSpan TimeBetweenClicks = new TimeSpan(0, 0, 0, 1); //5 seconds
    /// <summary>
    /// Current time the timer is at. Used in displays
    /// </summary>
    public DateTime CurrentClickerTime;
    /// <summary>
    /// Bool to determine if Click button has been clicked
    /// </summary>
    public bool IsClicked = false;

    private RadiantSceneController m_sceneController;

    private DateTime m_lastClickedTime;
    private Text m_timeRemainingText;
    private Text m_amountBoughtText;
    private Text m_clickButtonGoldText;
    private Text m_upgradeCostText;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_timeRemainingText = transform.FindChild("Buttons/StandBack/StandUI/TimeRemaining").GetComponent<Text>();
        m_amountBoughtText = transform.Find("Buttons/StandBack/StandUI/AmountCanvas/AmountText").GetComponent<Text>();
        m_clickButtonGoldText = transform.Find("Buttons/ClickButtonBack/ClickButton/ClickUI/ClickWorthText").GetComponent<Text>();
        m_upgradeCostText = transform.Find("Buttons/UpgradeCostBack/UpgradeCostCanvas/UpCostText").GetComponent<Text>(); ;

        ButtonManager.OnBuyClickerPressed += OnBuyClickerButtonPressed;
        ButtonManager.OnHeroClickButtonPress += OnClickButtonPressed;
	}
	
	void Update ()
    {
        UpdateCountdownTimer();
        UpdateUIText();

        if(IsClicked)
        {
            TimeSpan yes = DateTime.Now - m_lastClickedTime;
            if( yes >= new TimeSpan(0,0,0,0,500)) //0.5s
            {
                Debug.Log("Completed Click");
                IsClicked = false;
            }
        }
	}

    public void UpdateCountdownTimer()
    {
        
    }

    void OnBuyClickerButtonPressed()
    {
        UpgradeCost = Mathf.Round(UpgradeCost * UpgradeMultiplier);
    }

    void UpdateUIText()
    {
        m_clickButtonGoldText.text = ClickAmount + " gold";
        m_amountBoughtText.text = ClickerMultiplier.ToString();
        m_upgradeCostText.text = UpgradeCost.ToString() + " gold";
    }

    void OnClickButtonPressed()
    {
        m_sceneController.TotalGold += ClickAmount; //Add gold

        m_lastClickedTime = DateTime.Now;
        IsClicked = true;
    }
}
