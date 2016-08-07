﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlchemistController : MonoBehaviour
{
    public bool GreevilsGreedUpgrade = false;
    public bool ChemicalRageUpgrade = false;

    GameObject m_greevilsGreedButton;
    GameObject m_chemicalRageButton;
    Image m_greevilsGreedImage;
    Image m_chemicalRageImage;

    void Start()
    {
        m_greevilsGreedButton = transform.Find("Buttons/StandBack/UpgradesCanvas/GreevilsGreedBack/GreevilsGreedBtn").gameObject;
        m_chemicalRageButton = transform.Find("Buttons/StandBack/UpgradesCanvas/ChemicalRageBack/ChemicalRageBtn").gameObject;
        m_greevilsGreedImage = m_greevilsGreedButton.GetComponent<Image>();
        m_chemicalRageImage = m_chemicalRageButton.GetComponent<Image>();

        UpgradesController.BuyGreevilsGreedUpgrade += BuyGreevilsGreedUpgrade;
        UpgradesController.BuyChemicalRageUpgrade += BuyChemicalRageUpgrade;

        //turn to grey
        m_greevilsGreedImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_chemicalRageImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyGreevilsGreedUpgrade()
    {
        GreevilsGreedUpgrade = true;
        Debug.Log("Bought GreevilsGreed Upgrade");
        //turn to white
        m_greevilsGreedImage.color = new Color(1f, 1f, 1f);
    }

    void BuyChemicalRageUpgrade()
    {
        ChemicalRageUpgrade = true;
        Debug.Log("Bought ChemicalRage Upgrade");
        //turn to white
        m_chemicalRageImage.color = new Color(1f, 1f, 1f);
    }
}
