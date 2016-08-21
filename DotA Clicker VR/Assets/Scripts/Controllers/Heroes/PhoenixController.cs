using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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

    void Start()
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

    void BuySunrayUpgrade()
    {
        SunrayUpgrade = true;
        Debug.Log("Bought Sunray Upgrade");

        //Give white color to ability
        m_sunrayCooldown.fillAmount = 0;

        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuySupernovaUpgrade()
    {
        SupernovaUpgrade = true;
        Debug.Log("Bought Supernova Upgrade");

        //Give white color to ability
        m_supernovaCooldown.fillAmount = 0;

        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyPhoenixManager()
    {
        Debug.Log("Bought Phoenix Manager");
        PhoenixManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = PhoenixManager;
    }

    public void ActivateSunray()
    {
        if (SunrayActive) return;
        SunrayActive = true;

        SunrayEffects();

        //Do animation and voice line
        m_phoenixAnimator.SetTrigger("useSunray");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SunrayResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(SunrayAbilitySound);
    }

    public void ActivateSupernova()
    {
        if (SupernovaActive) return;
        SupernovaActive = true;

        SupernovaEffects();

        //Do animation and voice line
        m_phoenixAnimator.SetTrigger("useSupernova");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SupernovaResponses);

        //Start Supernova Ability Wait
        StartCoroutine(SupernovaStartWait());
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Sunray")
        {
            m_sunrayCooldownTxt.gameObject.SetActive(false);
            m_sunrayCooldown.fillAmount = 0;
            m_sunrayCountdown = false;
            SunrayActive = false;
        }
        else if (ability == "Supernova")
        {
            m_supernovaCooldownTxt.gameObject.SetActive(false);
            m_supernovaCooldown.fillAmount = 0;
            m_supernovaCountdown = false;
            SupernovaActive = false;
        }
        /*Do after active effects have done their duration*/
        else if (ability == "SunrayActiveFinish")
        {
            m_sunrayActiveFade.gameObject.SetActive(false);

            RemoveSunrayEffects();

            //Start Cooldown clock once finished
            m_sunrayCooldown.fillAmount = 1;
            m_sunrayCooldownTxt.gameObject.SetActive(true);
            m_sunrayCurrentTime = m_sunrayCDImageCount = SunrayCooldown;
            m_sunrayCountdown = true;

            StartCoroutine(AbilityCooldown(SunrayCooldown, "sunray"));
        }
        else if (ability == "SupernovaActiveFinish")
        {
            m_supernovaActiveFade.gameObject.SetActive(false);

            RemoveSupernovaEffects();

            //Cooldown clock
            m_supernovaCooldown.fillAmount = 1;
            m_supernovaCooldownTxt.gameObject.SetActive(true);
            m_supernovaCurrentTime = m_supernovaCDImageCount = SupernovaCooldown;
            m_supernovaCountdown = true;

            StartCoroutine(AbilityCooldown(SupernovaCooldown, "supernova"));
        }

    }

    void ClickedButton(string name)
    {
        if (name == "PhoenixBuyStand")
        {
            m_phoenixAnimator.SetBool("isAttacking", true);
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

    void SunrayEffects()
    {
        m_sunrayActiveFade.gameObject.SetActive(true);

        //do effct

        StartCoroutine(AbilityCooldown(SunrayActiveDuration, "SunrayActiveFinish"));

    }

    void RemoveSunrayEffects()
    {

    }

    void SupernovaEffects()
    {
        m_sunrayActiveFade.gameObject.SetActive(true);

        //do effect

        StartCoroutine(AbilityCooldown(SupernovaActiveDuration, "SupernovaActiveFinish"));
    }

    void RemoveSupernovaEffects()
    {

    }
}
