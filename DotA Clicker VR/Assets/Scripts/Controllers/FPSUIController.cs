using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSUIController : MonoBehaviour {

    RadiantSceneController m_sceneController;
    Text m_goldText;

    void Awake()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_goldText = transform.Find("TotalGold/TotalGoldText").GetComponent<Text>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        m_goldText.text = m_sceneController.TotalGold.ToString("0");
    }
}
