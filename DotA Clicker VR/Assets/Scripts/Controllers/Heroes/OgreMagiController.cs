using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

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

    void Awake()
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
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void Start()
    {
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        FireblastUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Ogre Magi").Ability1Level > 0;
        BloodlustUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Ogre Magi").Ability2Level > 0;
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

    void BuyFireblastUpgrade(int level)
    {
        FireblastUpgrade = true;

        //Give white color to abiity
        m_fireblastCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyBloodlustUpgrade(int level)
    {
        BloodlustUpgrade = true;

        //Give white color to abiity
        m_bloodlustCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = level;
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
        ActivateFireblast(FireblastActiveDuration, true);
    }

    public void ActivateFireblast(double remainingTime, bool doSound)
    {
        if (FireblastActive || !FireblastUpgrade) return;
        FireblastActive = true;

        FireblastEffects((float)remainingTime);

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useFireblast");

        if(doSound)
        {
            if (m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, FireblastResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(FireblastAbilitySound);
        }
    }

    public void ActivateBloodlust()
    {
        ActivateBloodlust(BloodlustActiveDuration, true);
    }

    public void ActivateBloodlust(double remainingTime, bool doSound)
    {
        if (BloodlustActive || !BloodlustUpgrade) return;
        BloodlustActive = true;

        BloodlustEffects((float)remainingTime);

        //Do animation and voice line
        m_ogreMagiAnimator.SetTrigger("useBloodlust");

        if (doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, BloodlustResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(BloodlustAbilitySound);
        }
    }

    IEnumerator AbilityCooldown(float time, string ability, bool removeAbilityEffects)
    {
        yield return new WaitForSeconds(time);

        OnAbilityFinished(ability, removeAbilityEffects);
    }

    public void OnAbilityFinished(string ability, bool removeAbilityEffects)
    {
        if (ability == "Fireblast")
        {
            m_fireblastCooldownTxt.gameObject.SetActive(false);
            m_fireblastCooldown.fillAmount = 0;
            m_fireblastCountdown = false;
            FireblastActive = false;
            m_clickerController.Ability1InUse = false;
        }
        else if (ability == "Bloodlust")
        {
            m_bloodlustCooldownTxt.gameObject.SetActive(false);
            m_bloodlustCooldown.fillAmount = 0;
            m_bloodlustCountdown = false;
            BloodlustActive = false;
            m_clickerController.Ability2InUse= false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "FireblastActiveFinish")
        {
            m_fireblastActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveFireblastEffects();

            //Start Cooldown clock once finished
            m_fireblastCooldown.fillAmount = 1;
            m_fireblastCooldownTxt.gameObject.SetActive(true);
            m_fireblastCurrentTime = m_fireblastCDImageCount = FireblastCooldown;
            m_fireblastCountdown = true;

            StartCoroutine(AbilityCooldown(FireblastCooldown, "Fireblast", removeAbilityEffects));
        }
        else if (ability == "BloodlustActiveFinish")
        {
            m_bloodlustActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveBloodlustEffects();

            //Cooldown clock
            m_bloodlustCooldown.fillAmount = 1;
            m_bloodlustCooldownTxt.gameObject.SetActive(true);
            m_bloodlustCurrentTime = m_bloodlustCDImageCount = BloodlustCooldown;
            m_bloodlustCountdown = true;

            StartCoroutine(AbilityCooldown(BloodlustCooldown, "Bloodlust", removeAbilityEffects));
        }
    }

    void ClickedButton(string name)
    {
        if (name == "OgreMagiBuyStand")
        {
            m_ogreMagiAnimator.SetBool("isAttacking", true);

            if (!m_clickerController.HasManager)
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

    void FireblastEffects(float remainingTime)
    {
        m_fireblastActiveFade.gameObject.SetActive(true);

        m_clickerController.SetAbilityModifierAmount(Constants.FireblastMultiplier, 1);

        StartCoroutine(AbilityCooldown(remainingTime, "FireblastActiveFinish", true));
    }

    void RemoveFireblastEffects()
    {
        m_clickerController.ClickAmount -= (m_fireblastModifiedValue / 2);
        m_clickerController.m_ability1ClickTime = DateTime.MinValue;
    }

    int bloodlustOldValue;

    void BloodlustEffects(float remainingTime)
    {
        m_bloodlustActiveFade.gameObject.SetActive(true);

        bloodlustOldValue = m_clickerController.TimeBetweenClicks.Seconds;
        m_bloodlustModifiedValue = (m_clickerController.TimeBetweenClicks.Seconds - 30) < 0 ? 1 : m_clickerController.TimeBetweenClicks.Seconds - 30;
        m_clickerController.TimeBetweenClicks = new TimeSpan(0, 0, m_bloodlustModifiedValue);

        m_clickerController.SetAbilityModifierAmount(Constants.BloodlustMultiplier, 2);

        StartCoroutine(AbilityCooldown(remainingTime, "BloodlustActiveFinish", true));
    }

    void RemoveBloodlustEffects()
    {
        m_clickerController.TimeBetweenClicks = new TimeSpan(0, 0, bloodlustOldValue);
        m_clickerController.m_ability2ClickTime = DateTime.MinValue;
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_ogreMagiAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
