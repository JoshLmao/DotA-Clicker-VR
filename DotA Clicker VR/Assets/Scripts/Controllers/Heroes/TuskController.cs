using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TuskController : MonoBehaviour
{
    public bool SnowballUpgrade = false;
    public bool SnowballActive = false;

    public bool WalrusPunchUpgrade = false;
    public bool WalrusPunchActive = false;

    public bool TuskManager = false;
    public GameObject Tusk;
    GameObject TuskSnowball;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] SnowballResponses;

    [SerializeField]
    AudioClip[] WalrusPunchResponses;

    public int SnowballCooldown;
    public int SnowballActiveDuration;
    [SerializeField]
    AudioClip SnowballAbilitySound;

    public int WalrusPunchCooldown;
    public int WalrusPunchActiveDuration;
    [SerializeField]
    AudioClip WalrusPunchAbilitySound;

    GameObject m_snowballButton;
    GameObject m_walrusPunchButton;
    Animator m_tuskAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    //Snowball animation
    bool m_rotateSnowball = false;
    float m_rotateSpeed = 56f;

    //Countdown
    Image m_snowballCooldown, m_walrusPunchCooldown;
    float m_snowballCurrentTime, m_walrusPunchCurrentTime;
    float m_snowballCDImageCount, m_walrusPunchCDImageCount;
    bool m_snowballCountdown, m_walrusPunchCountdown;
    Text m_snowballCooldownTxt, m_walrusPunchCooldownTxt;
    Image m_snowballActiveFade, m_walrusPunchActiveFade;
    int m_snowballCountModifier;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Tusk = transform.Find("Tusk").gameObject;
        TuskSnowball = transform.Find("Tusk_Snowball").gameObject;
        TuskSnowball.SetActive(false);

        m_tuskAnimator = Tusk.GetComponent<Animator>();
        m_audioSource = Tusk.GetComponent<AudioSource>();
        m_abilitySource = transform.Find("AbilitySound").GetComponent<AudioSource>();

        m_snowballButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SnowballBack/SnowballBtn").gameObject;
        m_walrusPunchButton = transform.Find("Buttons/StandBack/UpgradesCanvas/WalrusPunchBack/WalrusPunchBtn").gameObject;
        m_snowballCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/SnowballBack/CDImg").GetComponent<Image>();
        m_walrusPunchCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/WalrusPunchBack/CDImg").GetComponent<Image>();
        m_snowballCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/SnowballBack/CDText").GetComponent<Text>();
        m_walrusPunchCooldownTxt = transform.Find("Buttons/StandBack/UpgradesCanvas/WalrusPunchBack/CDText").GetComponent<Text>();
        m_snowballCooldownTxt.gameObject.SetActive(false);
        m_walrusPunchCooldownTxt.gameObject.SetActive(false);

        m_snowballActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/SnowballBack/ActiveFade").GetComponent<Image>();
        m_walrusPunchActiveFade = transform.Find("Buttons/StandBack/UpgradesCanvas/WalrusPunchBack/ActiveFade").GetComponent<Image>();
        m_snowballActiveFade.gameObject.SetActive(false);
        m_walrusPunchActiveFade.gameObject.SetActive(false);

        UpgradesController.BuySnowballUpgrade += BuySnowballUpgrade;
        UpgradesController.BuyWalrusPunchUpgrade += BuyWalrusPunchUpgrade;
        ManagersController.BuyTuskManager += BuyTuskManager;
    }

    void Update()
    {
        if (m_rotateSnowball)
        {
            TuskSnowball.transform.Rotate(Vector3.right * Time.deltaTime * m_rotateSpeed);
        }

        if (SnowballActive && m_snowballCountdown)
        {
            m_snowballCurrentTime -= Time.deltaTime;
            m_snowballCDImageCount = m_snowballCurrentTime;

            float time = Mathf.Round(m_snowballCurrentTime);
            m_snowballCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_snowballCDImageCount - 0) / (SnowballCooldown - 0);
            m_snowballCooldown.fillAmount = scaledValue;
        }

        if (WalrusPunchActive && m_walrusPunchCountdown)
        {
            m_walrusPunchCurrentTime -= Time.deltaTime;
            m_walrusPunchCDImageCount = m_walrusPunchCurrentTime;

            float time = Mathf.Round(m_walrusPunchCurrentTime);
            m_walrusPunchCooldownTxt.text = (time + 1).ToString();

            float scaledValue = (m_walrusPunchCDImageCount - 0) / (WalrusPunchCooldown - 0);
            m_walrusPunchCooldown.fillAmount = scaledValue;
        }
    }

    void BuySnowballUpgrade()
    {
        SnowballUpgrade = true;
        Debug.Log("Bought Snowball Upgrade");

        //Give white color to ability
        m_snowballCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyWalrusPunchUpgrade()
    {
        WalrusPunchUpgrade = true;
        Debug.Log("Bought WalrusPunch Upgrade");

        //Give white color to ability
        m_walrusPunchCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyTuskManager()
    {
        Debug.Log("Bought Tusk Manager");
        TuskManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = TuskManager;
    }

    public void ActivateSnowball()
    {
        if (SnowballActive) return;
        SnowballActive = true;

        SnowballEffects();

        //Do animation and voice line
        m_tuskAnimator.SetTrigger("useSnowball");
        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SnowballResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(SnowballAbilitySound);

        StartCoroutine(SnowballStartWait());
    }

    public void ActivateWalrusPunch()
    {
        if (WalrusPunchActive) return;
        WalrusPunchActive = true;

        WalrusPunchEffects();

        //Do animation and voice line
        m_tuskAnimator.SetTrigger("useWalrusPunch");
        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, WalrusPunchResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(WalrusPunchAbilitySound);
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Snowball")
        {
            m_snowballCooldownTxt.gameObject.SetActive(false);
            m_snowballCooldown.fillAmount = 0;
            m_snowballCountdown = false;
            SnowballActive = false;
        }
        else if (ability == "WalrusPunch")
        {
            m_walrusPunchCooldownTxt.gameObject.SetActive(false);
            m_walrusPunchCooldown.fillAmount = 0;
            m_walrusPunchCountdown = false;
            WalrusPunchActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "OverchargeActiveFinish")
        {
            m_snowballActiveFade.gameObject.SetActive(false);

            RemoveSnowballEffects();

            //Start Cooldown clock once finished
            m_snowballCooldown.fillAmount = 1;
            m_snowballCooldownTxt.gameObject.SetActive(true);
            m_snowballCurrentTime = m_snowballCDImageCount = SnowballCooldown;
            m_snowballCountdown = true;

            StartCoroutine(AbilityCooldown(SnowballCooldown, "Snowball"));
        }
        else if (ability == "RelocateActiveFinish")
        {
            m_walrusPunchActiveFade.gameObject.SetActive(false);

            RemoveWalrusPunchEffects();

            //Cooldown clock
            m_walrusPunchCooldown.fillAmount = 1;
            m_walrusPunchCooldownTxt.gameObject.SetActive(true);
            m_walrusPunchCurrentTime = m_walrusPunchCDImageCount = WalrusPunchCooldown;
            m_walrusPunchCountdown = true;

            StartCoroutine(AbilityCooldown(WalrusPunchCooldown, "WalrusPunch"));
        }
    }

    void ClickedButton(string name)
    {
        if (name == "TuskBuyStand")
        {
            m_tuskAnimator.SetBool("isAttacking", true);
            RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "TuskBuyStand")
        {
            m_tuskAnimator.SetBool("isAttacking", false);
        }
    }

    IEnumerator SnowballStartWait()
    {
        yield return new WaitForSeconds(0f); //Duration it takes to get into anim

        //Once animation is done
        SnowballAbility();
    }

    void SnowballAbility()
    {
        Tusk.SetActive(false);
        TuskSnowball.SetActive(true);

        StartCoroutine(SnowballMidWait());
        m_rotateSnowball = true;
    }

    IEnumerator SnowballMidWait()
    {
        yield return new WaitForSeconds(6f); //Duration for ability sound clip

        //Once animation is done
        SnowballAbilityFinish();
    }

    void SnowballAbilityFinish()
    {
        m_rotateSnowball = false;
        Tusk.SetActive(true);
        TuskSnowball.SetActive(false);
        TuskSnowball.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        m_tuskAnimator.SetTrigger("finishSnowball");

    }

    void SnowballEffects()
    {
        m_snowballActiveFade.gameObject.SetActive(true);

        //do effects

        StartCoroutine(AbilityCooldown(SnowballActiveDuration, "OverchargeActiveFinish"));
    }

    void RemoveSnowballEffects()
    {

    }

    void WalrusPunchEffects()
    {
        m_walrusPunchActiveFade.gameObject.SetActive(true);

        //do effects

        StartCoroutine(AbilityCooldown(WalrusPunchActiveDuration, "OverchargeActiveFinish"));
    }

    void RemoveWalrusPunchEffects()
    {

    }
}
