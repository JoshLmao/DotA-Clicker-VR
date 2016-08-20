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
    [SerializeField]
    AudioClip SunrayAbilitySound;

    public int SupernovaCooldown;
    [SerializeField]
    AudioClip SupernovaAbilitySound;

    GameObject m_sunrayButton;
    GameObject m_supernovaButton;
    Image m_sunrayImage;
    Image m_supernovaImage;
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
        m_sunrayImage = m_sunrayButton.GetComponent<Image>();
        m_supernovaImage = m_supernovaButton.GetComponent<Image>();

        UpgradesController.BuySunrayUpgrade += BuySunrayUpgrade;
        UpgradesController.BuySupernovaUpgrade += BuySupernovaUpgrade;
        ManagersController.BuyPhoenixManager += BuyPhoenixManager;

        //turn to grey
        m_sunrayImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_supernovaImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void Update()
    {
        if(m_rotateEgg)
        {
            PhoenixEgg.transform.Rotate(Vector3.forward * Time.deltaTime * m_rotateSpeed);
            PhoenixEgg.transform.Translate(Vector3.down * Time.deltaTime * m_ySpeed, Space.World);
        }
    }

    void BuySunrayUpgrade()
    {
        SunrayUpgrade = true;
        Debug.Log("Bought Sunray Upgrade");
        //turn to white
        m_sunrayImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuySupernovaUpgrade()
    {
        SupernovaUpgrade = true;
        Debug.Log("Bought Supernova Upgrade");
        //turn to white
        m_supernovaImage.color = new Color(1f, 1f, 1f);
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
        Debug.Log("Activated Sunray");
        SunrayActive = true;
        m_sunrayImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_phoenixAnimator.SetTrigger("useSunray");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SunrayResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(SunrayAbilitySound);

        StartCoroutine(AbilityCooldown(SunrayCooldown, "Sunray"));
    }

    public void ActivateSupernova()
    {
        if (SupernovaActive) return;
        Debug.Log("Activated Supernova");
        SupernovaActive = true;
        m_supernovaImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_phoenixAnimator.SetTrigger("useSupernova");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SupernovaResponses);

        StartCoroutine(AbilityCooldown(SupernovaCooldown, "Relocate"));

        //Start Supernova Ability Wait
        StartCoroutine(SupernovaStartWait());
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Sunray")
        {
            m_sunrayImage.color = new Color(1f, 1f, 1f);
            SunrayActive = false;
        }
        else if (ability == "Supernova")
        {
            m_supernovaImage.color = new Color(1f, 1f, 1f);
            SupernovaActive = false;
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

}
