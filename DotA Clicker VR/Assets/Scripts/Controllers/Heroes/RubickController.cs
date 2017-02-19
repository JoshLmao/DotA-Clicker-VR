using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RubickController : MonoBehaviour
{
    public bool TelekinesisUpgrade = false;
    public bool TelekinesisActive = false;

    public bool SpellStealUpgrade = false;
    public bool SpellStealActive = false;

    public bool RubickManager = false;
    public GameObject RubickModel;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] TelekinesisResponses;

    [SerializeField]
    AudioClip[] SpellStealResponses;

    public int TelekinesisCooldown;
    public int TelekinesisActiveDuration;
    [SerializeField]
    AudioClip TelekinesisAbilitySound;

    public int SpellStealCooldown;
    public int SpellStealActiveDuration;
    [SerializeField]
    AudioClip SpellStealAbilitySound;

    GameObject m_telekinesisButton;
    GameObject m_spellStealButton;
    Animator m_rubickAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController; //For Events

    //Countdown
    Image m_telekinesisCooldown, m_spellStealCooldown;
    float m_telekinesisCurrentTime, m_spellStealCurrentTime;
    float m_telekinesisCDImageCount, m_spellStealCDImageCount;
    bool m_telekinesisCountdown, m_spellStealCountdown;
    Text m_telekinesisCooldownTxt, m_spellStealCooldownTxt;
    Image m_telekinesisActiveFade, m_spellStealActiveFade;

    //Effects
    int m_telekinesisModifiedValue;

    void Awake()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        RubickModel = transform.Find("Rubick").gameObject;
        m_rubickAnimator = RubickModel.GetComponent<Animator>();
        m_audioSource = RubickModel.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Rubick/AbilitySound").GetComponent<AudioSource>();

        m_telekinesisButton = transform.Find("Buttons/StandBack/UpgradesCanvas/TelekinesisBack/TelekinesisBtn").gameObject;
        m_spellStealButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SpellStealBack/SpellStealBtn").gameObject;
        m_telekinesisCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/TelekinesisBack/CDImg").GetComponent<Image>();
        m_spellStealCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/SpellStealBack/CDImg").GetComponent<Image>();
        m_telekinesisCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/TelekinesisBack/CDText").GetComponent<Text>();
        m_spellStealCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/SpellStealBack/CDText").GetComponent<Text>();
        m_telekinesisCooldownTxt.gameObject.SetActive(false);
        m_spellStealCooldownTxt.gameObject.SetActive(false);

        m_telekinesisActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/TelekinesisBack/ActiveFade").GetComponent<Image>();
        m_spellStealActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/SpellStealBack/ActiveFade").GetComponent<Image>();
        m_telekinesisActiveFade.gameObject.SetActive(false);
        m_spellStealActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyTelekinesisUpgrade += BuyTelekinesisUpgrade;
        UpgradesController.BuySpellStealUpgrade += BuySpellStealUpgrade;
        ManagersController.BuyRubickManager += BuyRubickManager;
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void Start()
    {
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        TelekinesisUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Rubick").Ability1Level > 0;
        SpellStealUpgrade = saveFile.RadiantSide.Heroes.FirstOrDefault(x => x.HeroName == "Rubick").Ability2Level > 0;
    }

    void Update()
    {
        //if(TelekinesisActive)
        //{
        //    m_clickerController.AbilityModifierMulitiplier *= 2;
        //}

        if (TelekinesisActive && m_telekinesisCountdown)
        {
            m_telekinesisCurrentTime -= Time.deltaTime;
            m_telekinesisCDImageCount = m_telekinesisCurrentTime;

            float time = Mathf.Round(m_telekinesisCurrentTime);
            m_telekinesisCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_telekinesisCDImageCount - 0) / (TelekinesisCooldown - 0);
            m_telekinesisCooldown.fillAmount = scaledValue;
        }

        if (SpellStealActive && m_spellStealCountdown)
        {
            m_spellStealCurrentTime -= Time.deltaTime;
            m_spellStealCDImageCount = m_spellStealCurrentTime;

            float time = Mathf.Round(m_spellStealCurrentTime);
            m_spellStealCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_spellStealCDImageCount - 0) / (SpellStealCooldown - 0);
            m_spellStealCooldown.fillAmount = scaledValue;
        }
    }

    void BuyTelekinesisUpgrade(int level)
    {
        TelekinesisUpgrade = true;

        //Give white color to abiity
        m_telekinesisCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuySpellStealUpgrade(int level)
    {
        SpellStealUpgrade = true;

        //Give white color to abiity
        m_spellStealCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = level;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyRubickManager()
    {
        RubickManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = RubickManager;
    }

    public void ActivateTelekinesis()
    {
        ActivateTelekinesis(TelekinesisActiveDuration, true);
    }

    public void ActivateTelekinesis(double remainingTime, bool doSound)
    {
        if (TelekinesisActive) return;
        TelekinesisActive = true;

        TelekinesisEffects((float)remainingTime);

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useTelekinesis");

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, TelekinesisResponses);

            if (!m_abilitySource.isPlaying)
                m_audioSource.PlayOneShot(TelekinesisAbilitySound);
        }
    }

    public void ActivateSpellSteal()
    {
        ActivateSpellSteal(SpellStealActiveDuration, true);
    }

    public void ActivateSpellSteal(double remainingTime, bool doSound)
    {
        if (SpellStealActive) return;
        SpellStealActive = true;

        SpellStealEffects((float)remainingTime);

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useSpellSteal");

        if(doSound)
        {
            if (!m_audioSource.isPlaying)
                RadiantClickerController.PlayRandomClip(m_audioSource, SpellStealResponses);

            if (!m_abilitySource.isPlaying)
                m_audioSource.PlayOneShot(SpellStealAbilitySound);
        }
    }

    IEnumerator AbilityCooldown(float time, string ability, bool removeEffects)
    {
        yield return new WaitForSeconds(time);

        OnAbilityFinished(ability, removeEffects);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="removeEffects">Choose to remove effects or not. False if loading from save file</param>
    public void OnAbilityFinished(string ability, bool removeEffects)
    {
        if (ability == "Telekinesis")
        {
            m_telekinesisCooldownTxt.gameObject.SetActive(false);
            m_telekinesisCooldown.fillAmount = 0;
            m_telekinesisCountdown = false;
            TelekinesisActive = false;
            m_clickerController.Ability1InUse = false;
        }
        else if (ability == "SpellSteal")
        {
            m_spellStealCooldownTxt.gameObject.SetActive(false);
            m_spellStealCooldown.fillAmount = 0;
            m_spellStealCountdown = false;
            SpellStealActive = false;
            m_clickerController.Ability2InUse= false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "TelekinesisActiveFinish")
        {
            m_telekinesisActiveFade.gameObject.SetActive(false);

            if(removeEffects)
                RemoveTelekinesisEffects();

            //Start Cooldown clock once finished
            m_telekinesisCooldown.fillAmount = 1;
            m_telekinesisCooldownTxt.gameObject.SetActive(true);
            m_telekinesisCurrentTime = m_telekinesisCDImageCount = TelekinesisCooldown;
            m_telekinesisCountdown = true;

            StartCoroutine(AbilityCooldown(TelekinesisCooldown, "Telekinesis", true));
        }
        else if (ability == "SpellStealActiveFinish")
        {
            m_spellStealActiveFade.gameObject.SetActive(false);

            if(removeEffects)
                RemoveSpellStealEffects();

            //Cooldown clock
            m_spellStealCooldown.fillAmount = 1;
            m_spellStealCooldownTxt.gameObject.SetActive(true);
            m_spellStealCurrentTime = m_spellStealCDImageCount = SpellStealCooldown;
            m_spellStealCountdown = true;

            StartCoroutine(AbilityCooldown(SpellStealCooldown, "SpellSteal", true));
        }
    }

    void ClickedButton(string name)
    {
        if(name == "RubickBuyStand")
        {
            m_rubickAnimator.SetBool("isAttacking", true);

            if (!m_clickerController.HasManager)
                RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if(name == "RubickBuyStand")
        {
            m_rubickAnimator.SetBool("isAttacking", false);
        }
    }

    void TelekinesisEffects(float remainingTime)
    {
        m_telekinesisActiveFade.gameObject.SetActive(true);

        m_clickerController.SetAbilityModifierAmount(Constants.TelekinesisMultiplier, 1);

        StartCoroutine(AbilityCooldown(remainingTime, "TelekinesisActiveFinish", true));
    }

    void RemoveTelekinesisEffects()
    {
        m_clickerController.m_ability1ClickTime = System.DateTime.MinValue;
    }

    void SpellStealEffects(float remainingTime)
    {
        m_spellStealActiveFade.gameObject.SetActive(true);

        /*Rubick steals another heroes click amount for one click. Cooldown: 3 minutes*/
        m_clickerController.SetAbilityModifierAmount(Constants.SpellStealMultiplier, 2);

        StartCoroutine(AbilityCooldown(remainingTime, "SpellStealActiveFinish", true));
    }

    void RemoveSpellStealEffects()
    {
        m_clickerController.RemoveAbilityModifierAmount(Constants.SpellStealMultiplier);
        m_clickerController.m_ability2ClickTime = System.DateTime.MinValue;
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_rubickAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
