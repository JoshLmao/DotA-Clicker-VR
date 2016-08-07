using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OgreMagiController : MonoBehaviour
{
    public bool FireblastUpgrade = false;
    public bool FireblastActive = false;

    public bool BloodlustUpgrade = false;
    public bool BloodlustActive = false;

    public bool OgreMagiManager = false;

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
        ManagersController.BuyOgreMagiManager += BuyOgreMagiManager;

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

    void BuyOgreMagiManager()
    {
        Debug.Log("Bought Ogre Magi Manager");
        OgreMagiManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = OgreMagiManager;
    }

    public void ActivateFireblast()
    {
        Debug.Log("Activated Mana Void");
        m_fireblastImage.color = new Color(0.275f, 0.275f, 0.275f);
        FireblastActive = true;

        AbilityCooldown(180);

        m_fireblastImage.color = new Color(1f, 1f, 1f);
        FireblastActive = false;
    }

    public void ActivateBloodlust()
    {
        Debug.Log("Activated Mana Void");
        m_bloodlustImage.color = new Color(0.275f, 0.275f, 0.275f);
        BloodlustActive = true;

        AbilityCooldown(180);

        m_bloodlustImage.color = new Color(1f, 1f, 1f);
        BloodlustActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
