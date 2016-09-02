using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StatsController : MonoBehaviour {

    RadiantSceneController m_sceneController;

    Text m_totalTimePlayedText;
    Text m_totalClicksText;

    void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        m_totalTimePlayedText = transform.Find("GeneralStats/TotalTime/TTText").GetComponent<Text>();
        m_totalClicksText = transform.Find("GeneralStats/TotalClicks/TCText").GetComponent<Text>();

    }
	
	void Update ()
    {
        //Live Play Time
        TimeSpan t = TimeSpan.FromSeconds(m_sceneController.CurrentSaveFile.SessionStats.TotalPlayTime + Time.realtimeSinceStartup);
        m_totalTimePlayedText.text = string.Format("{0:D1} days, \n{1:D2}:{2:D2}:{3:D2}", t.Days, t.Hours, t.Minutes, t.Seconds);

        //Live Click Count
        m_totalClicksText.text = m_sceneController.ClickCount.ToString() + " clicks";
    }

}
