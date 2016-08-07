﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IoController : MonoBehaviour
{
    public bool OverchargeUpgrade = false;
    public bool RelocateUpgrade = false;
    public bool IoManager = false;

    GameObject m_overchargeButton;
    GameObject m_relocateButton;
    Image m_overchargeImage;
    Image m_relocateImage;

    void Start()
    {
        m_overchargeButton = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/OverchargeBtn").gameObject;
        m_relocateButton = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/RelocateBtn").gameObject;
        m_overchargeImage = m_overchargeButton.GetComponent<Image>();
        m_relocateImage = m_relocateButton.GetComponent<Image>();

        UpgradesController.BuyOverchargeUpgrade += BuyOverchargeUpgrade;
        UpgradesController.BuyRelocateUpgrade += BuyRelocateUpgrade;
        ManagersController.BuyIoManager += BuyIoManager;

        //turn to grey
        m_overchargeImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_relocateImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyOverchargeUpgrade()
    {
        OverchargeUpgrade = true;
        Debug.Log("Bought Overcharge Upgrade");
        //turn to white
        m_overchargeImage.color = new Color(1f, 1f, 1f);
    }

    void BuyRelocateUpgrade()
    {
        RelocateUpgrade = true;
        Debug.Log("Bought Relocate Upgrade");
        //turn to white
        m_relocateImage.color = new Color(1f, 1f, 1f);
    }

    void BuyIoManager()
    {
        Debug.Log("Bought Io Manager");
        IoManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = IoManager;
    }
}
