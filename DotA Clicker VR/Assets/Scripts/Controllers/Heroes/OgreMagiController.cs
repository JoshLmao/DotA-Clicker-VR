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

    public int FireblastCooldown;
    [SerializeField]
    AudioClip FireblastAbilitySound;

    public int BloodlustCooldown;
    [SerializeField]
    AudioClip BloodlustAbilitySound;

    GameObject m_fireblastButton;
    GameObject m_bloodlustButton;
    Image m_fireblastImage;
    Image m_bloodlustImage;
    Animator m_ogreMagiAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        OgreMagi = transform.Find("OgreMagi").gameObject;
        m_ogreMagiAnimator = OgreMagi.GetComponent<Animator>();
        m_audioSource = OgreMagi.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("OgreMagi/AbilitySound").GetComponent<AudioSource>();

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
        if (FireblastActive) return;

        Debug.Log("Activated FireBlast");
        FireblastActive = true;
        m_fireblastImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useFireblast");
        if(m_audioSource.isPlaying)
        {
            RadiantClickerController.PlayRandomClip(m_audioSource, FireblastResponses);
        }

        if (!m_abilitySource.isPlaying)
        {
            m_abilitySource.PlayOneShot(FireblastAbilitySound);
        }

        StartCoroutine(AbilityCooldown(FireblastCooldown, "Fireblast"));

    }

    public void ActivateBloodlust()
    {
        if (BloodlustActive) return;

        Debug.Log("Activated Bloodlust");
        m_bloodlustImage.color = new Color(0.275f, 0.275f, 0.275f);
        BloodlustActive = true;

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useBloodlust");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, BloodlustResponses);

        if(!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(BloodlustAbilitySound);

        StartCoroutine(AbilityCooldown(BloodlustCooldown, "Bloodlust"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if(ability == "Fireblast")
        {
            m_fireblastImage.color = new Color(1f, 1f, 1f);
            FireblastActive = false;
        }
        else if(ability == "Bloodlust")
        {
            m_bloodlustImage.color = new Color(1f, 1f, 1f);
            BloodlustActive = false;
        }
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
