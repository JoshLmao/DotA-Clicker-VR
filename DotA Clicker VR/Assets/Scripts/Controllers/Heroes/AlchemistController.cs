using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AlchemistController : MonoBehaviour
{
    public bool GreevilsGreedUpgrade = false;
    public bool GreevilsGreedActive = false;

    public bool ChemicalRageUpgrade = false;
    public bool ChemicalRageActive = false;

    public bool AlchemistManager = false;
    public GameObject Alchemist;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] GreevilsGreedResponses;

    [SerializeField]
    AudioClip[] ChemicalRageResponses;

    public int GreevilsGreedCooldown;
    public int GreevilsGreedActiveDuration;
    [SerializeField]
    AudioClip GreevilsGreedAbilitySound;

    public int ChemicalRageCooldown;
    public int ChemicalRageActiveDuration;
    [SerializeField]
    AudioClip ChemicalRageAbilitySound;

    GameObject m_greevilsGreedButton;
    GameObject m_chemicalRageButton;
    Animator m_alcAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Countdown
    Image m_greevilsGreedCooldown, m_chemicalRageCooldown;
    float m_greevilsGreedCurrentTime, m_chemicalRageCurrentTime;
    float m_greevilsGreedCDImageCount, m_chemicalRageCDImageCount;
    bool m_greevilsGreedCountdown, m_chemicalRageCountdown;
    Text m_greevilsGreedCooldownTxt, m_chemicalRageCooldownTxt;
    Image m_greevilsGreedActiveFade, m_chemicalRageActiveFade;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Alchemist = transform.Find("Alchemist").gameObject;
        m_alcAnimator = Alchemist.GetComponent<Animator>();
        m_audioSource = Alchemist.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Alchemist/AbilitySound").GetComponent<AudioSource>();

        m_greevilsGreedButton = transform.Find("Buttons/StandBack/UpgradesCanvas/GreevilsGreedBack/GreevilsGreedBtn").gameObject;
        m_chemicalRageButton = transform.Find("Buttons/StandBack/UpgradesCanvas/ChemicalRageBack/ChemicalRageBtn").gameObject;
        m_greevilsGreedCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/GreevilsGreedBack/CDImg").GetComponent<Image>();
        m_chemicalRageCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/ChemicalRageBack/CDImg").GetComponent<Image>();
        m_greevilsGreedCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/GreevilsGreedBack/CDText").GetComponent<Text>();
        m_chemicalRageCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/ChemicalRageBack/CDText").GetComponent<Text>();
        m_greevilsGreedCooldownTxt.gameObject.SetActive(false);
        m_chemicalRageCooldownTxt.gameObject.SetActive(false);

        m_greevilsGreedActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/GreevilsGreedBack/ActiveFade").GetComponent<Image>();
        m_chemicalRageActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/ChemicalRageBack/ActiveFade").GetComponent<Image>();
        m_greevilsGreedActiveFade.gameObject.SetActive(false);
        m_chemicalRageActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyGreevilsGreedUpgrade += BuyGreevilsGreedUpgrade;
        UpgradesController.BuyChemicalRageUpgrade += BuyChemicalRageUpgrade;
        ManagersController.BuyAlchemistManager += BuyAlchemistManager;
    }

    void Update()
    {
        if (GreevilsGreedActive && m_greevilsGreedCountdown)
        {
            m_greevilsGreedCurrentTime -= Time.deltaTime;
            m_greevilsGreedCDImageCount = m_greevilsGreedCurrentTime;

            float time = Mathf.Round(m_greevilsGreedCurrentTime);
            m_greevilsGreedCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_greevilsGreedCDImageCount - 0) / (GreevilsGreedCooldown - 0);
            m_greevilsGreedCooldown.fillAmount = scaledValue;
        }

        if (ChemicalRageActive && m_chemicalRageCountdown)
        {
            m_chemicalRageCurrentTime -= Time.deltaTime;
            m_chemicalRageCDImageCount = m_chemicalRageCurrentTime;

            float time = Mathf.Round(m_chemicalRageCurrentTime);
            m_chemicalRageCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_chemicalRageCDImageCount - 0) / (ChemicalRageCooldown - 0);
            m_chemicalRageCooldown.fillAmount = scaledValue;
        }
    }

    void BuyGreevilsGreedUpgrade(int level)
    {
        GreevilsGreedUpgrade = true;
        Debug.Log("Bought GreevilsGreed Upgrade");

        //Give white color to ability
        m_greevilsGreedCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = level;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyChemicalRageUpgrade(int level)
    {
        ChemicalRageUpgrade = true;
        Debug.Log("Bought ChemicalRage Upgrade");

        //Give white color to ability
        m_chemicalRageCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyAlchemistManager()
    {
        Debug.Log("Bought Alchemist Manager");
        AlchemistManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = AlchemistManager;
    }

    public void ActivateGreevilsGreed()
    {
        if (GreevilsGreedActive) return;
        GreevilsGreedActive = true;

        GreevilsGreedEffects();

        //Do animation and voice line
        m_alcAnimator.SetTrigger("useGreevilsGreed");
        RadiantClickerController.PlayRandomClip(m_audioSource, GreevilsGreedResponses);
    }

    public void ActivateGreevilsGreed(double remainingTime)
    {
        if (GreevilsGreedActive) return;
        GreevilsGreedActive = true;

        GreevilsGreedEffects((float)remainingTime);

        //Do animation and voice line
        m_alcAnimator.SetTrigger("useGreevilsGreed");
        RadiantClickerController.PlayRandomClip(m_audioSource, GreevilsGreedResponses);
    }

    public void ActivateChemicalRage()
    {
        if (ChemicalRageActive) return;
        ChemicalRageActive = true;

        ChemicalRageEffects();

        //Do animation and voice line
        m_alcAnimator.SetBool("useChemicalRage", true);
        RadiantClickerController.PlayRandomClip(m_audioSource, ChemicalRageResponses);
    }


    public void ActivateChemicalRage(double remainingTime)
    {
        if (ChemicalRageActive) return;
        ChemicalRageActive = true;

        ChemicalRageEffects((float)remainingTime);

        //Do animation and voice line
        m_alcAnimator.SetBool("useChemicalRage", true);
        //RadiantClickerController.PlayRandomClip(m_audioSource, ChemicalRageResponses);
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "GreevilsGreed")
        {
            m_greevilsGreedCooldownTxt.gameObject.SetActive(false);
            m_greevilsGreedCooldown.fillAmount = 0;
            m_greevilsGreedCountdown = false;
            GreevilsGreedActive = false;
        }
        else if (ability == "ChemicalRage")
        {
            m_chemicalRageCooldownTxt.gameObject.SetActive(false);
            m_chemicalRageCooldown.fillAmount = 0;
            m_chemicalRageCountdown = false;
            ChemicalRageActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "GreevilsGreedActiveFinish")
        {
            m_greevilsGreedActiveFade.gameObject.SetActive(false);

            RemoveGreevilsGreedEffects();

            //Start Cooldown clock once finished
            m_greevilsGreedCooldown.fillAmount = 1;
            m_greevilsGreedCooldownTxt.gameObject.SetActive(true);
            m_greevilsGreedCurrentTime = m_greevilsGreedCDImageCount = GreevilsGreedCooldown;
            m_greevilsGreedCountdown = true;

            StartCoroutine(AbilityCooldown(GreevilsGreedCooldown, "GreevilsGreed"));
        }
        else if (ability == "ChemicalRageActiveFinish")
        {
            m_chemicalRageActiveFade.gameObject.SetActive(false);

            RemoveChemicalRageEffects();

            //Cooldown clock
            m_chemicalRageCooldown.fillAmount = 1;
            m_chemicalRageCooldownTxt.gameObject.SetActive(true);
            m_chemicalRageCurrentTime = m_chemicalRageCDImageCount = ChemicalRageCooldown;
            m_chemicalRageCountdown = true;

            StartCoroutine(AbilityCooldown(ChemicalRageCooldown, "ChemicalRage"));
        }

    }

    void ClickedButton(string name)
    {
        if(name == "AlchemistBuyStand")
        {
            m_alcAnimator.SetBool("isAttacking", true);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "AlchemistBuyStand")
        {
            m_alcAnimator.SetBool("isAttacking", false);
        }
    }

    void GreevilsGreedEffects()
    {
        m_greevilsGreedActiveFade.gameObject.SetActive(true);

        //do effects
        GreevilGreedAttackReduce();
        InvokeRepeating("GreevilsGreedAttackReduce", GreevilsGreedActiveDuration, 1.07f);

        StartCoroutine(AbilityCooldown(GreevilsGreedActiveDuration, "GreevilsGreedActiveFinish"));
    }

    void GreevilsGreedEffects(float remainingTime)
    {
        m_greevilsGreedActiveFade.gameObject.SetActive(true);

        //do effects
        GreevilGreedAttackReduce();
        InvokeRepeating("GreevilsGreedAttackReduce", remainingTime, 1.07f);

        StartCoroutine(AbilityCooldown(remainingTime, "GreevilsGreedActiveFinish"));
    }

    void GreevilGreedAttackReduce()
    {
        m_clickerController.CurrentClickerTime -= new TimeSpan(0, 0, m_clickerController.CurrentClickerTime.Seconds - 20);
    }

    void RemoveGreevilsGreedEffects()
    {

        m_clickerController.m_ability1ClickTime = DateTime.MinValue;
    }

    void ChemicalRageEffects()
    {
        m_chemicalRageActiveFade.gameObject.SetActive(true);

        //do effects
        int quarter = m_clickerController.CurrentClickerTime.Seconds / 4;
        int seconds = (quarter * 4) - (quarter * 3);
        m_clickerController.CurrentClickerTime = new TimeSpan(0, 0, seconds);

        StartCoroutine(AbilityCooldown(ChemicalRageActiveDuration, "ChemicalRageActiveFinish"));
    }

    void ChemicalRageEffects(float remainingTime)
    {
        m_chemicalRageActiveFade.gameObject.SetActive(true);

        //do effects
        int quarter = m_clickerController.CurrentClickerTime.Seconds / 4;
        int seconds = (quarter * 4) - (quarter * 3);
        m_clickerController.CurrentClickerTime = new TimeSpan(0, 0, seconds);

        StartCoroutine(AbilityCooldown(remainingTime, "ChemicalRageActiveFinish"));
    }

    void RemoveChemicalRageEffects()
    {
        m_clickerController.m_ability2ClickTime = DateTime.MinValue;
    }

    IEnumerator RareIdleCount(float time)
    {
        yield return new WaitForSeconds(time);
        m_alcAnimator.SetTrigger("doRareIdle");
        int pick = UnityEngine.Random.Range(60, 300);
        StartCoroutine(RareIdleCount(pick));
    }
}
