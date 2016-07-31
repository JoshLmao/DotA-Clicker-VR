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
    /// Amount of times the clicker has been bought
    /// </summary>
    public int ClickerMultiplier = 0;


    private RadiantSceneController parentController;
    /// <summary>
    /// Current time the timer is at. Used in displays
    /// </summary>
    private DateTime m_currentTime;

    private Text m_timeRemainingText;
    private Text m_amountBoughtText;

	void Start ()
    {
        parentController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_timeRemainingText = transform.FindChild("Buttons/StandBack/StandUI/TimeRemaining").GetComponent<Text>();
        m_amountBoughtText = transform.Find("Buttons/StandBack/StandUI/AmountCanvas/AmountText").GetComponent<Text>();
        ButtonManager.OnPressed += OnButtonPressed;
	}
	
	void Update ()
    {
        UpdateCountdownTimer();
	}

    public void UpdateCountdownTimer()
    {
        //m_currentTime;
    }

    void OnButtonPressed()
    {
        //Cant buy next level
        if (parentController.TotalGold < BuyNextLevel)
            return;

        ClickerMultiplier += 1;
        m_amountBoughtText.text = ClickerMultiplier.ToString();
        parentController.TotalGold -= BuyNextLevel;
    }
}
