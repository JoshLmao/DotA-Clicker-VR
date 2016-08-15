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

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Phoenix = transform.Find("Phoenix").gameObject;
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

    void BuySunrayUpgrade()
    {
        SunrayUpgrade = true;
        Debug.Log("Bought Sunray Upgrade");
        //turn to white
        m_sunrayImage.color = new Color(1f, 1f, 1f);
    }

    void BuySupernovaUpgrade()
    {
        SupernovaUpgrade = true;
        Debug.Log("Bought Supernova Upgrade");
        //turn to white
        m_supernovaImage.color = new Color(1f, 1f, 1f);
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

        AbilityCooldown(SunrayCooldown);

        m_sunrayImage.color = new Color(1f, 1f, 1f);
        SunrayActive = false;
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

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(SupernovaAbilitySound);

        AbilityCooldown(SupernovaCooldown);

        m_supernovaImage.color = new Color(1f, 1f, 1f);
        SupernovaActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
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
}
