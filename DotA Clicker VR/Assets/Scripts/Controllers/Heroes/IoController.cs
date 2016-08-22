using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IoController : MonoBehaviour
{
    public bool OverchargeUpgrade = false;
    public bool OverchargeActive = false;

    public bool RelocateUpgrade = false;
    public bool RelocateActive = false;

    public bool IoManager = false;
    public GameObject Io;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] OverchargeResponses;

    [SerializeField]
    AudioClip[] RelocateResponses;

    public int OverchargeCooldown;
    public int OverchargeActiveDuration;
    [SerializeField]
    AudioClip OverchargeAbilitySound;

    public int RelocateCooldown;
    public int RelocateActiveDuration;
    [SerializeField]
    AudioClip RelocateAbilitySound;

    GameObject m_overchargeButton;
    GameObject m_relocateButton;
    Animator m_ioAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Countdown
    Image m_overchargeCooldown, m_relocateCooldown;
    float m_overchargeCurrentTime, m_relocateCurrentTime;
    float m_overchargeCDImageCount, m_relocateCDImageCount;
    bool m_overchargeCountdown, m_relocateCountdown;
    Text m_overchargeCooldownTxt, m_relocateCooldownTxt;
    Image m_overchargeActiveFade, m_relocateActiveFade;

    //Effects
    int m_overchargeModifiedValue;
    int m_relocateModifiedValue;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Io = transform.Find("Io").gameObject;
        m_ioAnimator = Io.GetComponent<Animator>();
        m_audioSource = Io.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Io/AbilitySound").GetComponent<AudioSource>();

        m_overchargeButton = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/OverchargeBtn").gameObject;
        m_relocateButton = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/RelocateBtn").gameObject;
        m_overchargeCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/CDImg").GetComponent<Image>();
        m_relocateCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/CDImg").GetComponent<Image>();
        m_overchargeCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/CDText").GetComponent<Text>();
        m_relocateCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/CDText").GetComponent<Text>();
        m_overchargeCooldownTxt.gameObject.SetActive(false);
        m_relocateCooldownTxt.gameObject.SetActive(false);

        m_overchargeActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/ActiveFade").GetComponent<Image>();
        m_relocateActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/ActiveFade").GetComponent<Image>();
        m_overchargeActiveFade.gameObject.SetActive(false);
        m_relocateActiveFade.gameObject.SetActive(false);

        UpgradesController.BuyOverchargeUpgrade += BuyOverchargeUpgrade;
        UpgradesController.BuyRelocateUpgrade += BuyRelocateUpgrade;
        ManagersController.BuyIoManager += BuyIoManager;
    }

    void Update()
    {
        if (OverchargeActive && m_overchargeCountdown)
        {
            m_overchargeCurrentTime -= Time.deltaTime;
            m_overchargeCDImageCount = m_overchargeCurrentTime;

            float time = Mathf.Round(m_overchargeCurrentTime);
            m_overchargeCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_overchargeCDImageCount - 0) / (OverchargeCooldown - 0);
            m_overchargeCooldown.fillAmount = scaledValue;
        }

        if (RelocateActive && m_relocateCountdown)
        {
            m_relocateCurrentTime -= Time.deltaTime;
            m_relocateCDImageCount = m_relocateCurrentTime;

            float time = Mathf.Round(m_relocateCurrentTime);
            m_relocateCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_relocateCDImageCount - 0) / (RelocateCooldown - 0);
            m_relocateCooldown.fillAmount = scaledValue;
        }
    }

    void BuyOverchargeUpgrade()
    {
        OverchargeUpgrade = true;
        Debug.Log("Bought Overcharge Upgrade");
        
        //Give white color to ability
        m_overchargeCooldown.fillAmount = 0;

        //Make hero have lvl 1 of ability
        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyRelocateUpgrade()
    {
        RelocateUpgrade = true;
        Debug.Log("Bought Relocate Upgrade");
        
        //Give white color to abiity
        m_relocateCooldown.fillAmount = 0;

        //Make hero have lvl 1 of ability
        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyIoManager()
    {
        Debug.Log("Bought Io Manager");
        IoManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = IoManager;
    }

    public void ActivateOvercharge()
    {
        if (OverchargeActive) return;
        OverchargeActive = true;

        OverchargeEffects();

        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, OverchargeResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(OverchargeAbilitySound);

        //StartCoroutine(AbilityCooldown(OverchargeCooldown, "OverchargeAbility"));
    }

    public void ActivateRelocate()
    {
        if (RelocateActive) return;
        RelocateActive = true;

        RelocateEffects();

        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, RelocateResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(RelocateAbilitySound);

        //StartCoroutine(AbilityCooldown(RelocateCooldown, "RelocateAbility"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        //Do to abilities once off cooldown
        if (ability == "Overcharge")
        {
            m_overchargeCooldownTxt.gameObject.SetActive(false);
            m_overchargeCooldown.fillAmount = 0;
            m_overchargeCountdown = false;
            OverchargeActive = false;
        }
        else if (ability == "Relocate")
        {
            m_relocateCooldownTxt.gameObject.SetActive(false);
            m_relocateCooldown.fillAmount = 0;
            m_relocateCountdown = false;
            RelocateActive = false;
        }
        /*Do after active effects have done their duration*/
        else if(ability == "OverchargeActiveFinish")
        {
            m_overchargeActiveFade.gameObject.SetActive(false);

            RemoveOverchargeEffects();

            //Start Cooldown clock once finished
            m_overchargeCooldown.fillAmount = 1;
            m_overchargeCooldownTxt.gameObject.SetActive(true);
            m_overchargeCurrentTime = m_overchargeCDImageCount = OverchargeCooldown;
            m_overchargeCountdown = true;

            StartCoroutine(AbilityCooldown(OverchargeCooldown, "Overcharge"));
        }
        else if(ability == "RelocateActiveFinish")
        {
            m_relocateActiveFade.gameObject.SetActive(false);

            RemoveRelocateEffects();

            //Cooldown clock
            m_relocateCooldown.fillAmount = 1;
            m_relocateCooldownTxt.gameObject.SetActive(true);
            m_relocateCurrentTime = m_relocateCDImageCount = RelocateCooldown;
            m_relocateCountdown = true;

            StartCoroutine(AbilityCooldown(RelocateCooldown, "Relocate"));
        }
    }

    void ClickedButton(string name)
    {
        if(name == "IoBuyStand")
        {
            m_ioAnimator.SetBool("isAttacking", true);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "IoBuyStand")
        {
            m_ioAnimator.SetBool("isAttacking", false);
        }
    }

    void OverchargeEffects()
    {
        m_overchargeActiveFade.gameObject.SetActive(true);

        m_overchargeModifiedValue = m_clickerController.ClickAmount * 2;
        m_clickerController.ClickAmount = m_overchargeModifiedValue;

        StartCoroutine(AbilityCooldown(OverchargeActiveDuration, "OverchargeActiveFinish"));
    }

    void RemoveOverchargeEffects()
    {
        m_clickerController.ClickAmount -= (m_overchargeModifiedValue / 2);
    }

    void RelocateEffects()
    {
        m_relocateActiveFade.gameObject.SetActive(true);

        m_relocateModifiedValue = m_clickerController.ClickAmount * 4; 
        m_clickerController.ClickAmount = m_relocateModifiedValue;

        StartCoroutine(AbilityCooldown(RelocateActiveDuration, "RelocateActiveFinish"));
    }

    void RemoveRelocateEffects()
    {
        m_clickerController.ClickAmount -= (m_relocateModifiedValue / 2);
    }
}
