using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OgreMagiController : MonoBehaviour
{
    public bool FireblastUpgrade = false;
    public bool BloodlustUpgrade = false;

    GameObject m_fireblastButton;
    GameObject m_bloodlustButton;
    Image m_fireblastImage;
    Image m_bloodlustImage;

    void Start()
    {
        m_fireblastButton = transform.Find("Buttons/StandBack/UpgradesCanvas/FireblastBack/FireblastBtn").gameObject;
        m_bloodlustButton = transform.Find("Buttons/StandBack/UpgradesCanvas/BloodlustBack/BloodlustBtn").gameObject;
        m_fireblastImage = m_fireblastButton.GetComponent<Image>();
        m_bloodlustImage = m_bloodlustButton.GetComponent<Image>();

        UpgradesController.BuyFireblastUpgrade += BuyFireblastUpgrade;
        UpgradesController.BuyBloodlustUpgrade += BuyBloodlustUpgrade;

        //turn to grey
        m_fireblastImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_bloodlustImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyFireblastUpgrade()
    {
        FireblastUpgrade = true;
        Debug.Log("Bought Fireblast Upgrade");
        //turn to white
        m_fireblastImage.color = new Color(1f, 1f, 1f);
    }

    void BuyBloodlustUpgrade()
    {
        BloodlustUpgrade = true;
        Debug.Log("Bought Bloodlust Upgrade");
        //turn to white
        m_bloodlustImage.color = new Color(1f, 1f, 1f);
    }
}
