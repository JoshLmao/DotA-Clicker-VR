using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

public class AntiMageController : MonoBehaviour
{
    public bool BlinkUpgrade = false;
    public bool BlinkActive = false;

    public bool ManaVoidUpgrade = false;
    public bool ManaVoidActive = false;

    public bool AntiMageManager = false;
    public GameObject AntiMage;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] BlinkResponses;

    [SerializeField]
    AudioClip[] ManaVoidResponses;

    public int BlinkCooldown;
    public int BlinkActiveDuration;
    [SerializeField]
    AudioClip BlinkAbilitySound;

    public int ManaVoidCooldown;
    public int ManaVoidActiveDuration;
    [SerializeField]
    AudioClip ManaVoidAbilitySound;

    GameObject m_blinkButton;
    GameObject m_manaVoidButton;
    Animator m_antiMageAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Countdown
    Image m_blinkCooldown, m_manaVoidCooldown;
    float m_blinkCurrentTime, m_manaVoidCurrentTime;
    float m_blinkCDImageCount, m_manaVoidCDImageCount;
    bool m_blinkCountdown, m_manaVoidCountdown;
    Text m_blinkCooldownTxt, m_manaVoidCooldownTxt;
    Image m_blinkActiveFade, m_manaVoidActiveFade;

    void Awake()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        AntiMage = transform.Find("AntiMage").gameObject;
        m_antiMageAnimator = AntiMage.GetComponent<Animator>();
        m_audioSource = AntiMage.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("AntiMage/AbilitySound").GetComponent<AudioSource>();

        m_blinkButton = transform.Find("Buttons/StandBack/UpgradesCanvas/BlinkBack/BlinkBtn").gameObject;
        m_manaVoidButton = transform.Find("Buttons/StandBack/UpgradesCanvas/ManaVoidBack/ManaVoidBtn").gameObject;
        m_blinkCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/BlinkBack/CDImg").GetComponent<Image>();
        m_manaVoidCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/ManaVoidBack/CDImg").GetComponent<Image>();
        m_blinkCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/BlinkBack/CDText").GetComponent<Text>();
        m_manaVoidCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/ManaVoidBack/CDText").GetComponent<Text>();
        m_blinkCooldownTxt.gameObject.SetActive(false);
        m_manaVoidCooldownTxt.gameObject.SetActive(false);

        m_blinkActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/BlinkBack/ActiveFade").GetComponent<Image>();
        m_manaVoidActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/ManaVoidBack/ActiveFade").GetComponent<Image>();
        m_blinkActiveFade.gameObject.SetActive(false);
        m_manaVoidActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyBlinkUpgrade += BuyBlinkUpgrade;
        UpgradesController.BuyManaVoidUpgrade += BuyManaVoidUpgrade;
        ManagersController.BuyAntiMageManager += BuyAntiMageManager;
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void Start()
    {
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        BlinkUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Anti Mage").Ability1Level > 0;
        ManaVoidUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Anti Mage").Ability2Level > 0;
    }

    void Update()
    {
        if (BlinkActive && m_blinkCountdown)
        {
            m_blinkCurrentTime -= Time.deltaTime;
            m_blinkCDImageCount = m_blinkCurrentTime;

            float time = Mathf.Round(m_blinkCurrentTime);
            m_blinkCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_blinkCDImageCount - 0) / (BlinkCooldown - 0);
            m_blinkCooldown.fillAmount = scaledValue;
        }

        if (ManaVoidActive && m_manaVoidCountdown)
        {
            m_manaVoidCurrentTime -= Time.deltaTime;
            m_manaVoidCDImageCount = m_manaVoidCurrentTime;

            float time = Mathf.Round(m_manaVoidCurrentTime);
            m_manaVoidCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_manaVoidCDImageCount - 0) / (ManaVoidCooldown - 0);
            m_manaVoidCooldown.fillAmount = scaledValue;
        }
    }

    void BuyBlinkUpgrade(int level)
    {
        BlinkUpgrade = true;

        //Give white color to ability
        m_blinkCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyManaVoidUpgrade(int level)
    {
        ManaVoidUpgrade = true;

        //Give white color to ability
        m_manaVoidCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = level;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyAntiMageManager()
    {
        Debug.Log("Bought AntiMage Manager");
        AntiMageManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = AntiMageManager;
    }

    public void ActivateBlink()
    {
        ActivateBlink(BlinkActiveDuration, true);
    }

    public void ActivateBlink(double remainingTime, bool doSound)
    {
        if (BlinkActive) return;
        BlinkActive = true;

        BlinkEffects((float)remainingTime);

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useBlink");

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, BlinkResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(BlinkAbilitySound);
        }
    }

    public void ActivateManaVoid()
    {
        ActivateManaVoid(ManaVoidActiveDuration, true);
    }

    public void ActivateManaVoid(double remainingTime, bool doSound)
    {
        if (ManaVoidActive) return;
        ManaVoidActive = true;

        ManaVoidEffects((float)remainingTime);

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useManaVoid");

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, ManaVoidResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(ManaVoidAbilitySound);
        }
    }

    IEnumerator AbilityCooldown(float time, string ability, bool removeAbilityEffects)
    {
        yield return new WaitForSeconds(time);

        OnAbilityFinished(ability, removeAbilityEffects);
    }

    public void OnAbilityFinished(string ability, bool removeAbilityEffects)
    {
        if (ability == "Blink")
        {
            m_blinkCooldownTxt.gameObject.SetActive(false);
            m_blinkCooldown.fillAmount = 0;
            m_blinkCountdown = false;
            BlinkActive = false;
            m_clickerController.Ability1InUse = false;
        }
        else if (ability == "ManaVoid")
        {
            m_manaVoidCooldownTxt.gameObject.SetActive(false);
            m_manaVoidCooldown.fillAmount = 0;
            m_manaVoidCountdown = false;
            ManaVoidActive = false;
            m_clickerController.Ability2InUse = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "BlinkActiveFinish")
        {
            m_blinkActiveFade.gameObject.SetActive(false);

            if (removeAbilityEffects)
                RemoveBlinkEffects();

            //Start Cooldown clock once finished
            m_blinkCooldown.fillAmount = 1;
            m_blinkCooldownTxt.gameObject.SetActive(true);
            m_blinkCurrentTime = m_blinkCDImageCount = BlinkCooldown;
            m_blinkCountdown = true;

            StartCoroutine(AbilityCooldown(BlinkCooldown, "Blink", false));
        }
        else if (ability == "ManaVoidActiveFinish")
        {
            m_manaVoidActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveManaVoidEffects();

            //Cooldown clock
            m_manaVoidCooldown.fillAmount = 1;
            m_manaVoidCooldownTxt.gameObject.SetActive(true);
            m_manaVoidCurrentTime = m_manaVoidCDImageCount = ManaVoidCooldown;
            m_manaVoidCountdown = true;

            StartCoroutine(AbilityCooldown(ManaVoidCooldown, "ManaVoid", false));
        }
    }

    void ClickedButton(string name)
    {
        if (name == "AntiMageBuyStand")
        {
            m_antiMageAnimator.SetBool("isAttacking", true);

            if (!m_clickerController.HasManager)
                RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "AntiMageBuyStand")
        {
            m_antiMageAnimator.SetBool("isAttacking", false);
        }
    }

    void BlinkEffects(float remainingTime)
    {
        m_blinkActiveFade.gameObject.SetActive(true);

        //do effects
        var m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        //m_sceneController.AddToTotal(m_clickerController.ClickAmount, m_clickerController.ItemModifierMultiplier);
        m_clickerController.SetAbilityModifierAmount(Constants.BlinkMultiplier, 1);

        StartCoroutine(AbilityCooldown(remainingTime, "BlinkActiveFinish", true));
    }

    void RemoveBlinkEffects()
    {
        m_clickerController.m_ability1ClickTime = DateTime.MinValue;
        m_clickerController.RemoveAbilityModifierAmount(Constants.BlinkMultiplier);
    }

    void ManaVoidEffects(float remainingTime)
    {
        m_manaVoidActiveFade.gameObject.SetActive(true);

        var durationLeft = m_clickerController.CurrentClickerTime.Seconds;
        RadiantSceneController scene = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        RadiantClickerController antiMage = scene.SceneHeroes[4];

        //Left of Anti Magi is Sven
        if (antiMage.CurrentClickerTime.Seconds - durationLeft < 0)
        {
            scene.AddToTotal(antiMage.ClickAmount, m_clickerController.AbilityModifierMulitiplier, m_clickerController.ItemModifierMultiplier);
            antiMage.CurrentClickerTime = new TimeSpan(0, 0, antiMage.TimeBetweenClicks.Seconds - durationLeft);
        }
        else
        {
            antiMage.CurrentClickerTime = new TimeSpan(0, 0, antiMage.CurrentClickerTime.Seconds - durationLeft);
        }
        //Right of Anti Mage is Alchemist
        if (antiMage.CurrentClickerTime.Seconds - durationLeft < 0)
        {
            scene.AddToTotal(antiMage.ClickAmount, m_clickerController.AbilityModifierMulitiplier, m_clickerController.ItemModifierMultiplier);
            antiMage.CurrentClickerTime = new TimeSpan(0, 0, antiMage.TimeBetweenClicks.Seconds - durationLeft);
        }
        else
        {
            antiMage.CurrentClickerTime = new TimeSpan(0, 0, antiMage.CurrentClickerTime.Seconds - durationLeft);
        }

        m_clickerController.SetAbilityModifierAmount(Constants.ManaVoidMultiplier, 2);
        StartCoroutine(AbilityCooldown(remainingTime, "ManaVoidActiveFinish", true));
    }

    void RemoveManaVoidEffects()
    {
        m_clickerController.m_ability2ClickTime = DateTime.MinValue;
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_antiMageAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
