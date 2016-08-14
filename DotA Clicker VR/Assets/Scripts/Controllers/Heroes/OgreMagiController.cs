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
    public GameObject OgreMagi;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] FireblastResponses;

    [SerializeField]
    AudioClip[] BloodlustResponses;

    GameObject m_fireblastButton;
    GameObject m_bloodlustButton;
    Image m_fireblastImage;
    Image m_bloodlustImage;
    public Animator m_ogreMagiAnimator;
    AudioSource m_audioSource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        OgreMagi = transform.Find("OgreMagi").gameObject;
        m_ogreMagiAnimator = OgreMagi.GetComponent<Animator>();
        m_audioSource = OgreMagi.GetComponent<AudioSource>();

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

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useFireblast");
        RadiantClickerController.PlayRandomClip(m_audioSource, FireblastResponses);

        AbilityCooldown(180);

        m_fireblastImage.color = new Color(1f, 1f, 1f);
        FireblastActive = false;
    }

    public void ActivateBloodlust()
    {
        Debug.Log("Activated Mana Void");
        m_bloodlustImage.color = new Color(0.275f, 0.275f, 0.275f);
        BloodlustActive = true;

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useBloodlust");
        RadiantClickerController.PlayRandomClip(m_audioSource, BloodlustResponses);


        AbilityCooldown(180);

        m_bloodlustImage.color = new Color(1f, 1f, 1f);
        BloodlustActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void ClickedButton(string name)
    {
        if (name == "OgreMagiBuyStand")
        {
            m_ogreMagiAnimator.SetBool("isAttacking", true);
            RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "OgreMagiBuyStand")
        {
            m_ogreMagiAnimator.SetBool("isAttacking", false);
        }
    }
}
