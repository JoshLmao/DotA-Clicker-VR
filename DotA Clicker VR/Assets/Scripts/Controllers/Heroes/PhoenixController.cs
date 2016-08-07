using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhoenixController : MonoBehaviour
{
    public bool SunrayUpgrade = false;
    public bool SupernovaUpgrade = false;
    public bool PhoenixManager = false;

    GameObject m_sunrayButton;
    GameObject m_supernovaButton;
    Image m_sunrayImage;
    Image m_supernovaImage;

    void Start()
    {
        m_sunrayButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SunrayBack/SunrayBtn").gameObject;
        m_supernovaButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SupernovaBack/SupernovaBtn").gameObject;
        m_sunrayImage = m_sunrayButton.GetComponent<Image>();
        m_supernovaImage = m_supernovaButton.GetComponent<Image>();

        UpgradesController.BuySunrayUpgrade += BuySunrayUpgrade;
        UpgradesController.BuySupernovaUpgrade += BuySupernovaUpgrade;
        ManagersController.BuyPhoenixManager += BuyPhoenixManager;

        //turn to grey
        m_sunrayImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_supernovaImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuySunrayUpgrade()
    {
        SunrayUpgrade = true;
        Debug.Log("Bought Sunray Upgrade");
        //turn to white
        m_sunrayImage.color = new Color(1f, 1f, 1f);
    }

    void BuySupernovaUpgrade()
    {
        SupernovaUpgrade = true;
        Debug.Log("Bought Supernova Upgrade");
        //turn to white
        m_supernovaImage.color = new Color(1f, 1f, 1f);
    }

    void BuyPhoenixManager()
    {
        Debug.Log("Bought Phoenix Manager");
        PhoenixManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = PhoenixManager;
    }
}
