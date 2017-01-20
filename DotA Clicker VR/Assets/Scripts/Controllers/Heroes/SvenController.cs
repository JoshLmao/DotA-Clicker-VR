using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

public class SvenController : MonoBehaviour
{
    public bool WarCryUpgrade = false;
    public bool WarCryActive = false;

    public bool GodsStrengthUpgrade = false;
    public bool GodsStrengthActive = false;

    public bool SvenManager = false;
    public GameObject Sven;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] WarCryResponses;

    [SerializeField]
    AudioClip[] GodsStrengthResponses;

    public int WarCryCooldown;
    public int WarCryActiveDuration;
    [SerializeField]
    AudioClip WarCryAbilitySound;

    public int GodsStrengthCooldown;
    public int GodsStrengthActiveDuration;
    [SerializeField]
    AudioClip GodsStrengthAbilitySound;

    GameObject m_warCryButton;
    GameObject m_godsStrengthButton;
    Animator m_svenAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Countdown
    Image m_warCryCooldown, m_godsStrengthCooldown;
    float m_warCryCurrentTime, m_godsStrengthCurrentTime;
    float m_warCryCDImageCount, m_godsStrengthCDImageCount;
    bool m_warCryCountdown, m_godsStrengthCountdown;
    Text m_warCryCooldownTxt, m_godsStrengthCooldownTxt;
    Image m_warCryActiveFade, m_godsStrengthActiveFade;
    int m_warCryCountModifier;

    void Awake()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Sven = transform.Find("Sven").gameObject;
        m_svenAnimator = Sven.GetComponent<Animator>();
        m_audioSource = Sven.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Sven/AbilitySound").GetComponent<AudioSource>();

        m_warCryButton = transform.Find("Buttons/StandBack/UpgradesCanvas/WarCryBack/WarCryBtn").gameObject;
        m_godsStrengthButton = transform.Find("Buttons/StandBack/UpgradesCanvas/GodsStrengthBack/GodsStrengthBtn").gameObject;
        m_warCryCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/WarCryBack/CDImg").GetComponent<Image>();
        m_godsStrengthCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/GodsStrengthBack/CDImg").GetComponent<Image>();
        m_warCryCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/WarCryBack/CDText").GetComponent<Text>();
        m_godsStrengthCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/GodsStrengthBack/CDText").GetComponent<Text>();
        m_warCryCooldownTxt.gameObject.SetActive(false);
        m_godsStrengthCooldownTxt.gameObject.SetActive(false);

        m_warCryActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/WarCryBack/ActiveFade").GetComponent<Image>();
        m_godsStrengthActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/GodsStrengthBack/ActiveFade").GetComponent<Image>();
        m_warCryActiveFade.gameObject.SetActive(false);
        m_godsStrengthActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyWarCryUpgrade += BuyWarCryUpgrade;
        UpgradesController.BuyGodsStrengthUpgrade += BuyGodsStrengthUpgrade;
        ManagersController.BuySvenManager += BuySvenManager;
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        WarCryUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Sven").Ability1Level > 0;
        GodsStrengthUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Sven").Ability2Level > 0;
    }

    void Start()
    {
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }

    void Update()
    {
        if (WarCryActive && m_warCryCountdown)
        {
            m_warCryCurrentTime -= Time.deltaTime;
            m_warCryCDImageCount = m_warCryCurrentTime;

            float time = Mathf.Round(m_warCryCurrentTime);
            m_warCryCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_warCryCDImageCount - 0) / (WarCryCooldown - 0);
            m_warCryCooldown.fillAmount = scaledValue;
        }

        if (GodsStrengthActive && m_godsStrengthCountdown)
        {
            m_godsStrengthCurrentTime -= Time.deltaTime;
            m_godsStrengthCDImageCount = m_godsStrengthCurrentTime;

            float time = Mathf.Round(m_godsStrengthCurrentTime);
            m_godsStrengthCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_godsStrengthCDImageCount - 0) / (GodsStrengthCooldown - 0);
            m_godsStrengthCooldown.fillAmount = scaledValue;
        }
    }

    void BuyWarCryUpgrade(int level)
    {
        WarCryUpgrade = true;
        Debug.Log("Bought WarCry Upgrade");
        
        //Give white color to ability
        m_warCryCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyGodsStrengthUpgrade(int level)
    {
        GodsStrengthUpgrade = true;
        Debug.Log("Bought GodsStrength Upgrade");

        //Give white color to ability
        m_godsStrengthCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = level;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuySvenManager()
    {
        Debug.Log("Bought Sven Manager");
        SvenManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = SvenManager;
    }

    public void ActivateWarCry()
    {
        if (WarCryActive) return;
        WarCryActive = true;

        WarCryEffects();

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useWarCry");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, WarCryResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(WarCryAbilitySound);

        //StartCoroutine(AbilityCooldown(WarCryCooldown, "WarCry"));
    }

    public void ActivateWarCry(double remainingTime)
    {
        if (WarCryActive) return;
        WarCryActive = true;

        WarCryEffects((float)remainingTime);

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useWarCry");
        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, WarCryResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(WarCryAbilitySound);

        //StartCoroutine(AbilityCooldown(WarCryCooldown, "WarCry"));
    }

    public void ActivateGodsStrength()
    {
        if (GodsStrengthActive) return;
        GodsStrengthActive = true;

        GodsStrengthEffects();

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useGodsStrength");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, GodsStrengthResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(GodsStrengthAbilitySound);
    }

    public void ActivateGodsStrength(double remainingTime)
    {
        if (GodsStrengthActive) return;
        GodsStrengthActive = true;

        GodsStrengthEffects((float)remainingTime);

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useGodsStrength");
        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, GodsStrengthResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(GodsStrengthAbilitySound);
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "WarCry")
        {
            m_warCryCooldownTxt.gameObject.SetActive(false);
            m_warCryCooldown.fillAmount = 0;
            m_warCryCountdown = false;
            WarCryActive = false;
        }
        else if (ability == "GodsStrength")
        {
            m_godsStrengthCooldownTxt.gameObject.SetActive(false);
            m_godsStrengthCooldown.fillAmount = 0;
            m_godsStrengthCountdown = false;
            GodsStrengthActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "WarCryActiveFinish")
        {
            m_warCryActiveFade.gameObject.SetActive(false);

            RemoveWarCryEffects();

            //Start Cooldown clock once finished
            m_warCryCooldown.fillAmount = 1;
            m_warCryCooldownTxt.gameObject.SetActive(true);
            m_warCryCurrentTime = m_warCryCDImageCount = WarCryCooldown;
            m_warCryCountdown = true;

            StartCoroutine(AbilityCooldown(WarCryCooldown, "warCry"));
        }
        else if (ability == "GodsStrengthActiveFinish")
        {
            m_godsStrengthActiveFade.gameObject.SetActive(false);

            RemoveGodsStrengthEffects();

            //Cooldown clock
            m_godsStrengthCooldown.fillAmount = 1;
            m_godsStrengthCooldownTxt.gameObject.SetActive(true);
            m_godsStrengthCurrentTime = m_godsStrengthCDImageCount = GodsStrengthCooldown;
            m_godsStrengthCountdown = true;

            StartCoroutine(AbilityCooldown(GodsStrengthCooldown, "godsStrength"));
        }
    }

    void ClickedButton(string name)
    {
        if(name == "SvenBuyStand")
        {
            m_svenAnimator.SetBool("isAttacking", true);

            if (!m_clickerController.HasManager)
                RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "AlchemistBuyStand")
        {
            m_svenAnimator.SetBool("isAttacking", false);
        }
    }

    void WarCryEffects()
    {
        m_warCryActiveFade.gameObject.SetActive(true);

        //do effects

        StartCoroutine(AbilityCooldown(WarCryActiveDuration, "WarCryActiveFinish"));
    }

    void WarCryEffects(float remainingTime)
    {
        m_warCryActiveFade.gameObject.SetActive(true);

        //do effects

        StartCoroutine(AbilityCooldown(remainingTime, "WarCryActiveFinish"));
    }

    void RemoveWarCryEffects()
    {
        m_clickerController.m_ability1ClickTime = System.DateTime.MinValue;
    }

    void GodsStrengthEffects()
    {
        StartCoroutine(AbilityCooldown(GodsStrengthActiveDuration, "GodsStrengthActiveFinish"));
    }

    void GodsStrengthEffects(float remainingTime)
    {
        StartCoroutine(AbilityCooldown(remainingTime, "GodsStrengthActiveFinish"));
    }

    void RemoveGodsStrengthEffects()
    {
        m_godsStrengthActiveFade.gameObject.SetActive(true);
        m_clickerController.m_ability2ClickTime = System.DateTime.MinValue;

        //do effects

        StartCoroutine(AbilityCooldown(GodsStrengthActiveDuration, "GodsStrengthActiveFinish"));
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_svenAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
