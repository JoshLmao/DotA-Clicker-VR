using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

public class CMController : MonoBehaviour
{
    public bool CrystalNovaUpgrade = false;
    public bool CrystalNovaActive = false;

    public bool FrostbiteUpgrade = false;
    public bool FrostbiteActive = false;

    public bool CMManager = false;
    public GameObject CrystalMaiden;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] CrystalNovaResponses;

    [SerializeField]
    AudioClip[] FrostbiteResponses;

    public int CrystalNovaCooldown;
    public int CrystalNovaActiveDuration;
    [SerializeField]
    AudioClip CrystalNovaAbilitySound;

    public int FrostbiteCooldown;
    public int FrostbiteActiveDuration;
    [SerializeField]
    AudioClip FrostbiteAbilitySound;

    GameObject m_crystalNovaButton;
    GameObject m_frostbiteButton;
    Animator m_cmAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Countdown
    Image m_crystalNovaCooldown, m_frostbiteCooldown;
    float m_crystalNovaCurrentTime, m_frostbiteCurrentTime;
    float m_crystalNovaCDImageCount, m_frostbiteCDImageCount;
    bool m_crystalNovaCountdown, m_frostbiteCountdown;
    Text m_crystalNovaCooldownTxt, m_frostbiteCooldownTxt;
    Image m_crystalNovaActiveFade, m_frostbiteActiveFade;

    //Effects
    int m_crystalNovaModifiedValue;
    int m_frostbiteModifiedValue;

    void Awake()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        CrystalMaiden = transform.Find("CrystalMaiden").gameObject;
        m_cmAnimator = CrystalMaiden.GetComponent<Animator>();
        m_audioSource = CrystalMaiden.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("CrystalMaiden/AbilitySound").GetComponent<AudioSource>();

        m_crystalNovaButton = transform.Find("Buttons/StandBack/UpgradesCanvas/CrystalNovaBack/CrystalNovaBtn").gameObject;
        m_frostbiteButton = transform.Find("Buttons/StandBack/UpgradesCanvas/FrostbiteBack/FrostbiteBtn").gameObject;
        m_crystalNovaCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/CrystalNovaBack/CDImg").GetComponent<Image>();
        m_frostbiteCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/FrostbiteBack/CDImg").GetComponent<Image>();
        m_crystalNovaCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/CrystalNovaBack/CDText").GetComponent<Text>();
        m_frostbiteCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/FrostbiteBack/CDText").GetComponent<Text>();
        m_crystalNovaCooldownTxt.gameObject.SetActive(false);
        m_frostbiteCooldownTxt.gameObject.SetActive(false);

        m_crystalNovaActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/CrystalNovaBack/ActiveFade").GetComponent<Image>();
        m_frostbiteActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/FrostbiteBack/ActiveFade").GetComponent<Image>();
        m_crystalNovaActiveFade.gameObject.SetActive(false);
        m_frostbiteActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyCrystalNovaUpgrade += BuyCrystalNovaUpgrade;
        UpgradesController.BuyFrostbiteUpgrade += BuyFrostbiteUpgrade;
        ManagersController.BuyCMManager += BuyCMManager;
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void Start()
    {
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        CrystalNovaUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Crystal Maiden").Ability1Level > 0;
        FrostbiteUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Crystal Maiden").Ability2Level > 0;
    }

    void Update()
    {
        if (CrystalNovaActive && m_crystalNovaCountdown)
        {
            m_crystalNovaCurrentTime -= Time.deltaTime;
            m_crystalNovaCDImageCount = m_crystalNovaCurrentTime;

            float time = Mathf.Round(m_crystalNovaCurrentTime);
            m_crystalNovaCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_crystalNovaCDImageCount - 0) / (CrystalNovaCooldown - 0);
            m_crystalNovaCooldown.fillAmount = scaledValue;
        }

        if (FrostbiteActive && m_frostbiteCountdown)
        {
            m_frostbiteCurrentTime -= Time.deltaTime;
            m_frostbiteCDImageCount = m_frostbiteCurrentTime;

            float time = Mathf.Round(m_frostbiteCurrentTime);
            m_frostbiteCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_frostbiteCDImageCount - 0) / (FrostbiteCooldown - 0);
            m_frostbiteCooldown.fillAmount = scaledValue;
        }
    }

    void BuyCrystalNovaUpgrade(int level)
    {
        CrystalNovaUpgrade = true;
        
        //Give white color to ability
        m_crystalNovaCooldown.fillAmount = 0;

        //Make hero have lvl 1 of ability
        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyFrostbiteUpgrade(int level)
    {
        FrostbiteUpgrade = true;
        
        //Give white color to abiity
        m_frostbiteCooldown.fillAmount = 0;

        //Make hero have lvl 1 of ability
        m_clickerController.Ability2Level = level;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyCMManager()
    {
        Debug.Log("Bought CM Manager");
        CMManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = CMManager;
    }

    public void ActivateCrystalNova()
    {
        ActivateCrystalNova(CrystalNovaActiveDuration, true);
    }

    public void ActivateCrystalNova(double time, bool doSound)
    {
        if (CrystalNovaActive) return;
        CrystalNovaActive = true;

        m_cmAnimator.SetTrigger("useCrystalNova");
        CrystalNovaEffects((float)time);

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, CrystalNovaResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(CrystalNovaAbilitySound);
        }
    }

    public void ActivateFrostbite()
    {
        ActivateFrostbite(FrostbiteActiveDuration, true);
    }

    public void ActivateFrostbite(double time, bool doSound)
    {
        if (FrostbiteActive) return;
        FrostbiteActive = true;

        m_cmAnimator.SetTrigger("useFrostbite");
        FrostbiteEffects((float)time);

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, FrostbiteResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(FrostbiteAbilitySound);
        }
    }

    IEnumerator AbilityCooldown(float time, string ability, bool removeAbilityEffects, int duration)
    {
        yield return new WaitForSeconds(time);

        OnAbilityFinished(ability, removeAbilityEffects, duration);
    }

    public void OnAbilityFinished(string ability, bool removeAbilityEffects, int duration)
    {
        //Do to abilities once off cooldown
        if (ability == "Crystal Nova")
        {
            m_crystalNovaCooldownTxt.gameObject.SetActive(false);
            m_crystalNovaCooldown.fillAmount = 0;
            m_crystalNovaCountdown = false;
            CrystalNovaActive = false;
            m_clickerController.Ability1InUse = false;
        }
        else if (ability == "Frostbite")
        {
            m_frostbiteCooldownTxt.gameObject.SetActive(false);
            m_frostbiteCooldown.fillAmount = 0;
            m_frostbiteCountdown = false;
            FrostbiteActive = false;
            m_clickerController.Ability2InUse = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "CrystalNovaActiveFinish")
        {
            m_crystalNovaActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveCrystalNovaEffects();

            //Start Cooldown clock once finished
            m_crystalNovaCooldown.fillAmount = 1;
            m_crystalNovaCooldownTxt.gameObject.SetActive(true);
            m_crystalNovaCurrentTime = m_crystalNovaCDImageCount = duration;
            m_crystalNovaCountdown = true;

            StartCoroutine(AbilityCooldown(duration, "Crystal Nova", removeAbilityEffects, duration));
        }
        else if (ability == "FrostbiteActiveFinish")
        {
            m_frostbiteActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveFrostbiteEffects();

            //Cooldown clock
            m_frostbiteCooldown.fillAmount = 1;
            m_frostbiteCooldownTxt.gameObject.SetActive(true);
            m_frostbiteCurrentTime = m_frostbiteCDImageCount = duration;
            m_frostbiteCountdown = true;

            StartCoroutine(AbilityCooldown(duration, "Frostbite", removeAbilityEffects, duration));
        }
    }

    void ClickedButton(string name)
    {
        if(name == "CMBuyStand")
        {
            m_cmAnimator.SetBool("isAttacking", true);

            if(!m_clickerController.HasManager)
                RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "CMBuyStand")
        {
            m_cmAnimator.SetBool("isAttacking", false);
        }
    }

    void CrystalNovaEffects(float remainingTime)
    {
        m_crystalNovaActiveFade.gameObject.SetActive(true);

        m_clickerController.SetAbilityModifierAmount(Constants.CrystalNovaMultiplier, 1);

        StartCoroutine(AbilityCooldown(remainingTime, "CrystalNovaActiveFinish", true, CrystalNovaActiveDuration));
    }

    void RemoveCrystalNovaEffects()
    {
        m_clickerController.RemoveAbilityModifierAmount(Constants.CrystalNovaMultiplier);
        m_clickerController.m_ability1ClickTime = DateTime.MinValue;
    }

    void FrostbiteEffects(float secondsRemaining)
    {
        m_frostbiteActiveFade.gameObject.SetActive(true);

        m_clickerController.SetAbilityModifierAmount(Constants.FrostbiteMultiplier, 2);

        StartCoroutine(AbilityCooldown(secondsRemaining, "FrostbiteActiveFinish", true, FrostbiteActiveDuration));
    }

    void RemoveFrostbiteEffects()
    {
        m_clickerController.RemoveAbilityModifierAmount(Constants.FrostbiteMultiplier);
        m_clickerController.m_ability2ClickTime = DateTime.MinValue;
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_cmAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
