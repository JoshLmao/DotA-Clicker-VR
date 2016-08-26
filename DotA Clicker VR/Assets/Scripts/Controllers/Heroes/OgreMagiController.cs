using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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
    public int FireblastActiveDuration;
    [SerializeField]
    AudioClip FireblastAbilitySound;

    public int BloodlustCooldown;
    public int BloodlustActiveDuration;
    [SerializeField]
    AudioClip BloodlustAbilitySound;

    GameObject m_fireblastButton;
    GameObject m_bloodlustButton;
    Animator m_ogreMagiAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Countdown
    Image m_fireblastCooldown, m_bloodlustCooldown;
    float m_fireblastCurrentTime, m_bloodlustCurrentTime;
    float m_fireblastCDImageCount, m_bloodlustCDImageCount;
    bool m_fireblastCountdown, m_bloodlustCountdown;
    Text m_fireblastCooldownTxt, m_bloodlustCooldownTxt;
    Image m_fireblastActiveFade, m_bloodlustActiveFade;
    int m_fireblastCountModifier;

    //Effects
    int m_fireblastModifiedValue;
    int m_bloodlustModifiedValue;

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
        m_fireblastCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/FireblastBack/CDImg").GetComponent<Image>();
        m_bloodlustCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/BloodlustBack/CDImg").GetComponent<Image>();
        m_fireblastCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/FireblastBack/CDText").GetComponent<Text>();
        m_bloodlustCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/BloodlustBack/CDText").GetComponent<Text>();
        m_fireblastCooldownTxt.gameObject.SetActive(false);
        m_bloodlustCooldownTxt.gameObject.SetActive(false);

        m_fireblastActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/FireblastBack/ActiveFade").GetComponent<Image>();
        m_bloodlustActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/BloodlustBack/ActiveFade").GetComponent<Image>();
        m_fireblastActiveFade.gameObject.SetActive(false);
        m_bloodlustActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyFireblastUpgrade += BuyFireblastUpgrade;
        UpgradesController.BuyBloodlustUpgrade += BuyBloodlustUpgrade;
        ManagersController.BuyOgreMagiManager += BuyOgreMagiManager;
    }

    void Update()
    {
        if (FireblastActive && m_fireblastCountdown)
        {
            m_fireblastCurrentTime -= Time.deltaTime;
            m_fireblastCDImageCount = m_fireblastCurrentTime;

            float time = Mathf.Round(m_fireblastCurrentTime);
            m_fireblastCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_fireblastCDImageCount - 0) / (FireblastCooldown - 0);
            m_fireblastCooldown.fillAmount = scaledValue;
        }

        if (BloodlustActive && m_bloodlustCountdown)
        {
            m_bloodlustCurrentTime -= Time.deltaTime;
            m_bloodlustCDImageCount = m_bloodlustCurrentTime;

            float time = Mathf.Round(m_bloodlustCurrentTime);
            m_bloodlustCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_bloodlustCDImageCount - 0) / (BloodlustCooldown - 0);
            m_bloodlustCooldown.fillAmount = scaledValue;
        }
    }

    void BuyFireblastUpgrade()
    {
        FireblastUpgrade = true;
        Debug.Log("Bought Fireblast Upgrade");

        //Give white color to abiity
        m_fireblastCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyBloodlustUpgrade()
    {
        BloodlustUpgrade = true;
        Debug.Log("Bought Bloodlust Upgrade");

        //Give white color to abiity
        m_bloodlustCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
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
        FireblastActive = true;

        FireblastEffects();

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useFireblast");
        if(m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, FireblastResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(FireblastAbilitySound);
    }

    public void ActivateBloodlust()
    {
        if (BloodlustActive) return;
        BloodlustActive = true;

        BloodlustEffects();

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useBloodlust");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, BloodlustResponses);

        if(!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(BloodlustAbilitySound);
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if(ability == "Fireblast")
        {
            m_fireblastCooldownTxt.gameObject.SetActive(false);
            m_fireblastCooldown.fillAmount = 0;
            m_fireblastCountdown = false;
            FireblastActive = false;
        }
        else if(ability == "Bloodlust")
        {
            m_bloodlustCooldownTxt.gameObject.SetActive(false);
            m_bloodlustCooldown.fillAmount = 0;
            m_bloodlustCountdown = false;
            BloodlustActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "FireblastActiveFinish")
        {
            m_fireblastActiveFade.gameObject.SetActive(false);

            RemoveFireblastEffects();

            //Start Cooldown clock once finished
            m_fireblastCooldown.fillAmount = 1;
            m_fireblastCooldownTxt.gameObject.SetActive(true);
            m_fireblastCurrentTime = m_fireblastCDImageCount = FireblastCooldown;
            m_fireblastCountdown = true;

            StartCoroutine(AbilityCooldown(FireblastCooldown, "Fireblast"));
        }
        else if (ability == "BloodlustActiveFinish")
        {
            m_bloodlustActiveFade.gameObject.SetActive(false);

            RemoveBloodlustEffects();

            //Cooldown clock
            m_bloodlustCooldown.fillAmount = 1;
            m_bloodlustCooldownTxt.gameObject.SetActive(true);
            m_bloodlustCurrentTime = m_bloodlustCDImageCount = BloodlustCooldown;
            m_bloodlustCountdown = true;

            StartCoroutine(AbilityCooldown(BloodlustCooldown, "Bloodlust"));
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

    void FireblastEffects()
    {
        m_fireblastActiveFade.gameObject.SetActive(true);

        m_fireblastModifiedValue = m_clickerController.ClickAmount * 3;
        m_clickerController.ClickAmount = m_fireblastModifiedValue;


        StartCoroutine(AbilityCooldown(FireblastActiveDuration, "OverchargeActiveFinish"));
    }

    void RemoveFireblastEffects()
    {
        m_clickerController.ClickAmount -= (m_fireblastModifiedValue / 2);
    }
    int bloodlustOldValue;

    void BloodlustEffects()
    {
        m_bloodlustActiveFade.gameObject.SetActive(true);

        bloodlustOldValue = m_clickerController.TimeBetweenClicks.Seconds;
        m_bloodlustModifiedValue = (m_clickerController.TimeBetweenClicks.Seconds - 30) < 0 ? 1 : m_clickerController.TimeBetweenClicks.Seconds - 30;
        m_clickerController.TimeBetweenClicks = new TimeSpan(0, 0, m_bloodlustModifiedValue);

        StartCoroutine(AbilityCooldown(BloodlustActiveDuration, "BloodlustActiveFinish"));
    }

    void RemoveBloodlustEffects()
    {
        m_clickerController.TimeBetweenClicks = new TimeSpan(0, 0, bloodlustOldValue);
    }
}
