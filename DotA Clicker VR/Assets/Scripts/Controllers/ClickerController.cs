using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// Generic class aimed at being used by each clicker. Controls times and multipliers
/// </summary>
public class ClickerController : MonoBehaviour {

    public string ClickerHeroName;
    public int ClickAmount;
    public DateTime TimeBetweenClicks; //String.Format(TimeBetweenCLicks, HH/MM/SS);
    public int AmountOfClickers = 0;

    DateTime m_currentTime;
    Text m_timeRemainingText;

	void Start ()
    {
        m_timeRemainingText = transform.FindChild("StandBack/StandUI/TimeRemaining").GetComponent<Text>();
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
        AmountOfClickers += 1;
    }
}
