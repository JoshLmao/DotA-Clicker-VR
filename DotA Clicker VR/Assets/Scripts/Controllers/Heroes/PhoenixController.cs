using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

public class PhoenixController : MonoBehaviour
{
    public bool SunrayUpgrade = false;
    public bool SunrayActive = false;

    public bool SupernovaUpgrade = false;
    public bool SupernovaActive = false;

    public bool PhoenixManager = false;
    public GameObject Phoenix;
    GameObject PhoenixEgg;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] SunrayResponses;

    [SerializeField]
    AudioClip[] SupernovaResponses;

    public int SunrayCooldown;
    public int SunrayActiveDuration;
    [SerializeField]
    AudioClip SunrayAbilitySound;

    public int SupernovaCooldown;
    public int SupernovaActiveDuration;
    [SerializeField]
    AudioClip SupernovaAbilitySound;

    GameObject m_sunrayButton;
    GameObject m_supernovaButton;
    Animator m_phoenixAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;
    
    //Supernova Animation
    Transform phoenixOrig;
    Transform eggOriginal;
    bool m_rotateEgg;
    float m_rotateSpeed = 3f;
    float m_ySpeed = 0.1f;

    //Countdown
    Image m_sunrayCooldown, m_supernovaCooldown;
    float m_sunrayCurrentTime, m_supernovaCurrentTime;
    float m_sunrayCDImageCount, m_supernovaCDImageCount;
    bool m_sunrayCountdown, m_supernovaCountdown;
    Text m_sunrayCooldownTxt, m_supernovaCooldownTxt;
    Image m_sunrayActiveFade, m_supernovaActiveFade;
    int m_sunrayCountModifier;

    //Ability Effects
    public AudioClip GoldEarnedSound;

    void Awake()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Phoenix = transform.Find("Phoenix").gameObject;
        PhoenixEgg = transform.Find("Phoenix_Egg").gameObject;
        PhoenixEgg.SetActive(false);
        m_phoenixAnimator = Phoenix.GetComponent<Animator>();
        m_audioSource = Phoenix.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Phoenix/AbilitySound").GetComponent<AudioSource>();

        m_sunrayButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SunrayBack/SunrayBtn").gameObject;
        m_supernovaButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SupernovaBack/SupernovaBtn").gameObject;
        m_sunrayCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/SunrayBack/CDImg").GetComponent<Image>();
        m_supernovaCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/SupernovaBack/CDImg").GetComponent<Image>();
        m_sunrayCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/SunrayBack/CDText").GetComponent<Text>();
        m_supernovaCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/SupernovaBack/CDText").GetComponent<Text>();
        m_sunrayCooldownTxt.gameObject.SetActive(false);
        m_supernovaCooldownTxt.gameObject.SetActive(false);

        m_sunrayActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/SunrayBack/ActiveFade").GetComponent<Image>();
        m_supernovaActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/SupernovaBack/ActiveFade").GetComponent<Image>();
        m_sunrayActiveFade.gameObject.SetActive(false);
        m_supernovaActiveFade.gameObject.SetActive(false);

        UpgradesController.BuySunrayUpgrade += BuySunrayUpgrade;
        UpgradesController.BuySupernovaUpgrade += BuySupernovaUpgrade;
        ManagersController.BuyPhoenixManager += BuyPhoenixManager;
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void Start()
    {
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        SunrayUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Phoenix").Ability1Level > 0;
        SupernovaUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Phoenix").Ability2Level > 0;
    }

    void Update()
    {
        if(m_rotateEgg)
        {
            PhoenixEgg.transform.Rotate(Vector3.forward * Time.deltaTime * m_rotateSpeed);
            PhoenixEgg.transform.Translate(Vector3.down * Time.deltaTime * m_ySpeed, Space.World);
        }

        if (SunrayActive && m_sunrayCountdown)
        {
            m_sunrayCurrentTime -= Time.deltaTime;
            m_sunrayCDImageCount = m_sunrayCurrentTime;

            float time = Mathf.Round(m_sunrayCurrentTime);
            m_sunrayCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_sunrayCDImageCount - 0) / (SunrayCooldown - 0);
            m_sunrayCooldown.fillAmount = scaledValue;
        }

        if (SupernovaActive && m_supernovaCountdown)
        {
            m_supernovaCurrentTime -= Time.deltaTime;
            m_supernovaCDImageCount = m_supernovaCurrentTime;

            float time = Mathf.Round(m_supernovaCurrentTime);
            m_supernovaCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_supernovaCDImageCount - 0) / (SupernovaCooldown - 0);
            m_supernovaCooldown.fillAmount = scaledValue;
        }
    }

    void BuySunrayUpgrade(int level)
    {
        SunrayUpgrade = true;

        //Give white color to ability
        m_sunrayCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuySupernovaUpgrade(int level)
    {
        SupernovaUpgrade = true;

        //Give white color to ability
        m_supernovaCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = level;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyPhoenixManager()
    {
        PhoenixManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = PhoenixManager;
    }

    public void ActivateSunray()
    {
        ActivateSunray(SunrayActiveDuration, true);
    }

    public void ActivateSunray(double remainingTime, bool doSound)
    {
        if (SunrayActive) return;
        SunrayActive = true;

        SunrayEffects((float)remainingTime);

        //Do animation and voice line
        m_phoenixAnimator.SetTrigger("useSunray");

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, SunrayResponses);

            if (!m_abilitySource.isPlaying)
                m_abilitySource.PlayOneShot(SunrayAbilitySound);
        }
    }

    public void ActivateSupernova()
    {
        ActivateSupernova(SupernovaActiveDuration, true);
    }

    public void ActivateSupernova(double remainingTime, bool doSound)
    {
        if (SupernovaActive) return;
        SupernovaActive = true;

        SupernovaEffects((float)remainingTime);

        //Do animation and voice line
        m_phoenixAnimator.SetTrigger("useSupernova");

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, SupernovaResponses);
        }

        //Start Supernova Ability Wait
        StartCoroutine(SupernovaStartWait());
    }

    IEnumerator AbilityCooldown(float time, string ability, bool removeAbilityEffects)
    {
        yield return new WaitForSeconds(time);
        OnAbilityFinished(ability, removeAbilityEffects);
   }

    public void OnAbilityFinished(string ability, bool removeAbilityEffects)
    {
        if (ability == "Sunray")
        {
            m_sunrayCooldownTxt.gameObject.SetActive(false);
            m_sunrayCooldown.fillAmount = 0;
            m_sunrayCountdown = false;
            SunrayActive = false;
            m_clickerController.Ability1InUse = false;
        }
        else if (ability == "Supernova")
        {
            m_supernovaCooldownTxt.gameObject.SetActive(false);
            m_supernovaCooldown.fillAmount = 0;
            m_supernovaCountdown = false;
            SupernovaActive = false;
            m_clickerController.Ability2InUse = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "SunrayActiveFinish")
        {
            m_sunrayActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveSunrayEffects();

            //Start Cooldown clock once finished
            m_sunrayCooldown.fillAmount = 1;
            m_sunrayCooldownTxt.gameObject.SetActive(true);
            m_sunrayCurrentTime = m_sunrayCDImageCount = SunrayCooldown;
            m_sunrayCountdown = true;

            StartCoroutine(AbilityCooldown(SunrayCooldown, "sunray", false));
        }
        else if (ability == "SupernovaActiveFinish")
        {
            m_supernovaActiveFade.gameObject.SetActive(false);

            if(removeAbilityEffects)
                RemoveSupernovaEffects();

            //Cooldown clock
            m_supernovaCooldown.fillAmount = 1;
            m_supernovaCooldownTxt.gameObject.SetActive(true);
            m_supernovaCurrentTime = m_supernovaCDImageCount = SupernovaCooldown;
            m_supernovaCountdown = true;

            StartCoroutine(AbilityCooldown(SupernovaCooldown, "supernova", false));
        }
    }

    void ClickedButton(string name)
    {
        if (name == "PhoenixBuyStand")
        {
            m_phoenixAnimator.SetBool("isAttacking", true);

            if (!m_clickerController.HasManager)
                RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "PhoenixBuyStand")
        {
            m_phoenixAnimator.SetBool("isAttacking", false);
        }
    }


    IEnumerator SupernovaStartWait()
    {
        yield return new WaitForSeconds(0.5f); //Duration it takes to complete Supernova start anim

        //Once animation is done
        SupernovaAbility();
        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(SupernovaAbilitySound);
    }

    void SupernovaAbility()
    {
        phoenixOrig = Phoenix.transform;
        eggOriginal = PhoenixEgg.transform;

        //Change scale to fit inside
        Phoenix.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        //Setting parent reloads animation controller, set Y instead
        Vector3 pos = Phoenix.transform.localPosition;
        Phoenix.transform.localPosition = new Vector3(pos.x, 0.4f, pos.z);
        //Set active, 
        PhoenixEgg.SetActive(true);

        if (!m_audioSource.isPlaying)
        {
            m_audioSource.clip = SupernovaAbilitySound;
            m_audioSource.PlayDelayed(5f);
        }

        StartCoroutine(SupernovaMidWait());
        m_rotateEgg = true;
    }

    IEnumerator SupernovaMidWait()
    {
        yield return new WaitForSeconds(6f); //Duration inside the egg

        //Once animation is done
        SupernovaAbilityFinish();
    }

    void SupernovaAbilityFinish()
    {
        m_rotateEgg = false;

        //Set position back to normal
        Vector3 pos = Phoenix.transform.localPosition;
        Phoenix.transform.localPosition = new Vector3(pos.x, 0.002f, pos.z);
        Phoenix.transform.localScale = new Vector3(0.004f, 0.004f, 0.004f);
        Phoenix.transform.rotation = phoenixOrig.rotation;

        //Disable active and restore to original values for next
        PhoenixEgg.SetActive(false);
        PhoenixEgg.transform.position = eggOriginal.position;
        
        //Finally, the anim
        m_phoenixAnimator.SetTrigger("finishedSupernova");
        m_audioSource.clip = null;
    }

    void SunrayEffects(float remainingTime)
    {
        m_sunrayActiveFade.gameObject.SetActive(true);

        //do effect
        var sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        foreach (RadiantClickerController clicker in sceneController.SceneHeroes)
        {
            if ((clicker.TimeBetweenClicks.Seconds - 60) < 0)
            {
                clicker.TimeBetweenClicks -= new TimeSpan(0, 0, clicker.TimeBetweenClicks.Seconds - clicker.TimeBetweenClicks.Seconds);
                continue;
            }

            clicker.TimeBetweenClicks = new TimeSpan(0, 0, clicker.TimeBetweenClicks.Seconds - 60);
        }

        StartCoroutine(AbilityCooldown(remainingTime, "SunrayActiveFinish", true));
    }

    void RemoveSunrayEffects()
    {
        m_clickerController.m_ability1ClickTime = DateTime.MinValue;
    }

    void SupernovaEffects(float remainingTime)
    {
        m_sunrayActiveFade.gameObject.SetActive(true);

        //do effect
        InvokeRepeating("SupernovaRepeating", 6, 1);

        StartCoroutine(AbilityCooldown(remainingTime, "SupernovaActiveFinish", true));
    }

    void SupernovaRepeating()
    {
        var controller = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        controller.AddToTotal(m_clickerController.ClickAmount, m_clickerController.AbilityModifierMulitiplier, m_clickerController.ItemModifierMultiplier);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(GoldEarnedSound);
    }

    void RemoveSupernovaEffects()
    {
        m_clickerController.m_ability2ClickTime = DateTime.MinValue;
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_phoenixAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
