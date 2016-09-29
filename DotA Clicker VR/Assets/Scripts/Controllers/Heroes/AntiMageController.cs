using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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

    void Start()
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

    void BuyBlinkUpgrade()
    {
        BlinkUpgrade = true;
        Debug.Log("Bought Blink Upgrade");

        //Give white color to ability
        m_blinkCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyManaVoidUpgrade()
    {
        ManaVoidUpgrade = true;
        Debug.Log("Bought ManaVoid Upgrade");

        //Give white color to ability
        m_manaVoidCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = 1;
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
        if (BlinkActive) return;
        BlinkActive = true;

        BlinkEffects();

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useBlink");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, BlinkResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(BlinkAbilitySound);
    }

    public void ActivateManaVoid()
    {
        if (ManaVoidActive) return;
        ManaVoidActive = true;

        ManaVoidEffects();

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useManaVoid");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, ManaVoidResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(ManaVoidAbilitySound);
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Blink")
        {
            m_blinkCooldownTxt.gameObject.SetActive(false);
            m_blinkCooldown.fillAmount = 0;
            m_blinkCountdown = false;
            BlinkActive = false;
        }
        else if (ability == "ManaVoid")
        {
            m_manaVoidCooldownTxt.gameObject.SetActive(false);
            m_manaVoidCooldown.fillAmount = 0;
            m_manaVoidCountdown = false;
            ManaVoidActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "BlinkActiveFinish")
        {
            m_blinkActiveFade.gameObject.SetActive(false);

            RemoveBlinkEffects();

            //Start Cooldown clock once finished
            m_blinkCooldown.fillAmount = 1;
            m_blinkCooldownTxt.gameObject.SetActive(true);
            m_blinkCurrentTime = m_blinkCDImageCount = BlinkCooldown;
            m_blinkCountdown = true;

            StartCoroutine(AbilityCooldown(BlinkCooldown, "Blink"));
        }
        else if (ability == "ManaVoidActiveFinish")
        {
            m_manaVoidActiveFade.gameObject.SetActive(false);

            RemoveManaVoidEffects();

            //Cooldown clock
            m_manaVoidCooldown.fillAmount = 1;
            m_manaVoidCooldownTxt.gameObject.SetActive(true);
            m_manaVoidCurrentTime = m_manaVoidCDImageCount = ManaVoidCooldown;
            m_manaVoidCountdown = true;

            StartCoroutine(AbilityCooldown(ManaVoidCooldown, "ManaVoid"));
        }
    }

    void ClickedButton(string name)
    {
        if (name == "AntiMageBuyStand")
        {
            m_antiMageAnimator.SetBool("isAttacking", false);
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

    void BlinkEffects()
    {
        m_blinkActiveFade.gameObject.SetActive(true);

        //do effects
        var m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        m_sceneController.TotalGold += m_clickerController.ClickAmount;

        StartCoroutine(AbilityCooldown(BlinkActiveDuration, "BlinkActiveFinish"));
    }

    void RemoveBlinkEffects()
    {
        m_clickerController.m_ability1ClickTime = DateTime.MinValue;
    }

    void ManaVoidEffects()
    {
        m_manaVoidActiveFade.gameObject.SetActive(true);

        var durationLeft = m_clickerController.CurrentClickerTime.Seconds;
        RadiantSceneController scene = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        RadiantClickerController sven = scene.SceneHeroes[5];
        RadiantClickerController alchemist = scene.SceneHeroes[0];
        //Left of Anti Magi is Sven
        if (sven.CurrentClickerTime.Seconds - durationLeft < 0)
        {
            scene.TotalGold += sven.ClickAmount;
            sven.CurrentClickerTime = new TimeSpan(0, 0, sven.TimeBetweenClicks.Seconds - durationLeft);
        }
        else
        {
            sven.CurrentClickerTime = new TimeSpan(0, 0, sven.CurrentClickerTime.Seconds - durationLeft);
        }
        //Right of Anti Mage is Alchemist
        if (alchemist.CurrentClickerTime.Seconds - durationLeft < 0)
        {
            scene.TotalGold += sven.ClickAmount;
            alchemist.CurrentClickerTime = new TimeSpan(0, 0, alchemist.TimeBetweenClicks.Seconds - durationLeft);
        }
        else
        {
            alchemist.CurrentClickerTime = new TimeSpan(0, 0, alchemist.CurrentClickerTime.Seconds - durationLeft);
        }

        StartCoroutine(AbilityCooldown(ManaVoidActiveDuration, "ManaVoidActiveFinish"));
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
