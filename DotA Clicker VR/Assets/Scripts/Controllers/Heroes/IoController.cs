﻿using UnityEngine;
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
    [SerializeField]
    AudioClip OverchargeAbilitySound;

    public int RelocateCooldown;
    [SerializeField]
    AudioClip RelocateAbilitySound;

    GameObject m_overchargeButton;
    GameObject m_relocateButton;
    Image m_overchargeCooldown;
    Image m_relocateCooldown;
    Animator m_ioAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Io = transform.Find("Io").gameObject;
        m_ioAnimator = Io.GetComponent<Animator>();
        m_audioSource = Io.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Io/AbilitySound").GetComponent<AudioSource>();

        m_overchargeCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/CDImg").GetComponent<Image>();
        m_relocateCooldown = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/CDImg").GetComponent<Image>();
        m_overchargeButton = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/OverchargeBtn").gameObject;
        m_relocateButton = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/RelocateBtn").gameObject;

        UpgradesController.BuyOverchargeUpgrade += BuyOverchargeUpgrade;
        UpgradesController.BuyRelocateUpgrade += BuyRelocateUpgrade;
        ManagersController.BuyIoManager += BuyIoManager;

    }

    void Update()
    {
        if(OverchargeActive)
        {
            m_overchargeCooldown.fillAmount = 1;
        }
    }

    void BuyOverchargeUpgrade()
    {
        OverchargeUpgrade = true;
        Debug.Log("Bought Overcharge Upgrade");
        //Give white color to abiity
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
        Debug.Log("Activated Overcharge");
        OverchargeActive = true;
        //m_overchargeCD.fillamount = 1;

        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, OverchargeResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(OverchargeAbilitySound);

        StartCoroutine(AbilityCooldown(OverchargeCooldown, "Overcharge"));
    }

    public void ActivateRelocate()
    {
        if (RelocateActive) return;
        Debug.Log("Activated Relocate");
        OverchargeActive = true;
        //m_relocateCooldown.fillAmount = 1;

        if (!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, RelocateResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(RelocateAbilitySound);

        StartCoroutine(AbilityCooldown(RelocateCooldown, "Relocate"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Overcharge")
        {
            m_overchargeCooldown.fillAmount = 0;
            OverchargeActive = false;
        }
        else if (ability == "Relocate")
        {
            m_relocateCooldown.fillAmount = 0;
            OverchargeActive = false;
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
}
