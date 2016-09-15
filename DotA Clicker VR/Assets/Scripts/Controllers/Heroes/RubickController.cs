using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

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

    void Start()
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
    }

    void Update()
    {
        if(TelekinesisActive)
        {
            m_clickerController.ClickAmount *= 2;
        }

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

    void BuyTelekinesisUpgrade()
    {
        TelekinesisUpgrade = true;
        Debug.Log("Bought Telekinesis Upgrade");

        //Give white color to abiity
        m_telekinesisCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuySpellStealUpgrade()
    {
        SpellStealUpgrade = true;
        Debug.Log("Bought Spell Steal Upgrade");

        //Give white color to abiity
        m_spellStealCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyRubickManager()
    {
        Debug.Log("Bought Rubick Manager");
        RubickManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = RubickManager;
    }

    public void ActivateTelekinesis()
    {
        if (TelekinesisActive) return;
        TelekinesisActive = true;

        TelekinesisEffects();

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useTelekinesis");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, TelekinesisResponses);

        if (!m_abilitySource.isPlaying)
            m_audioSource.PlayOneShot(TelekinesisAbilitySound);
    }

    public void ActivateSpellSteal()
    {
        if (SpellStealActive) return;
        SpellStealActive = true;

        SpellStealEffects();

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useSpellSteal");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SpellStealResponses);

        if (!m_abilitySource.isPlaying)
            m_audioSource.PlayOneShot(SpellStealAbilitySound);
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Telekinesis")
        {
            m_telekinesisCooldownTxt.gameObject.SetActive(false);
            m_telekinesisCooldown.fillAmount = 0;
            m_telekinesisCountdown = false;
            TelekinesisActive = false;
        }
        else if (ability == "SpellSteal")
        {
            m_spellStealCooldownTxt.gameObject.SetActive(false);
            m_spellStealCooldown.fillAmount = 0;
            m_spellStealCountdown = false;
            SpellStealActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "TelekinesisActiveFinish")
        {
            m_telekinesisActiveFade.gameObject.SetActive(false);

            RemoveTelekinesisEffects();

            //Start Cooldown clock once finished
            m_telekinesisCooldown.fillAmount = 1;
            m_telekinesisCooldownTxt.gameObject.SetActive(true);
            m_telekinesisCurrentTime = m_telekinesisCDImageCount = TelekinesisCooldown;
            m_telekinesisCountdown = true;

            StartCoroutine(AbilityCooldown(TelekinesisCooldown, "Telekinesis"));
        }
        else if (ability == "SpellStealActiveFinish")
        {
            m_spellStealActiveFade.gameObject.SetActive(false);

            RemoveSpellStealEffects();

            //Cooldown clock
            m_spellStealCooldown.fillAmount = 1;
            m_spellStealCooldownTxt.gameObject.SetActive(true);
            m_spellStealCurrentTime = m_spellStealCDImageCount = SpellStealCooldown;
            m_spellStealCountdown = true;

            StartCoroutine(AbilityCooldown(SpellStealCooldown, "SpellSteal"));
        }
    }

    void ClickedButton(string name)
    {
        if(name == "RubickBuyStand")
        {
            m_rubickAnimator.SetBool("isAttacking", true);
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

    void TelekinesisEffects()
    {
        m_telekinesisActiveFade.gameObject.SetActive(true);

        m_telekinesisModifiedValue = m_clickerController.ClickAmount * 2;
        m_clickerController.ClickAmount = m_telekinesisModifiedValue;

        StartCoroutine(AbilityCooldown(TelekinesisActiveDuration, "TelekinesisActiveFinish"));
    }

    void RemoveTelekinesisEffects()
    {
        m_clickerController.ClickAmount -= (m_telekinesisModifiedValue / 2);
    }

    void SpellStealEffects()
    {
        m_spellStealActiveFade.gameObject.SetActive(true);

        /*Rubick steals another heroes click amount for one click. Cooldown: 3 minutes*/

        StartCoroutine(AbilityCooldown(SpellStealActiveDuration, "SpellStealActiveFinish"));
    }

    void RemoveSpellStealEffects()
    {

    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_rubickAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
