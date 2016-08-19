using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AntiMageController : MonoBehaviour
{
    public bool BlinkUpgrade = false;
    public bool BlinkActive = false;

    public bool ManaVoidUpgrade = false;
    public bool ManaVoidActive = false;

    public bool AntiMageManager = false;
    public GameObject AntiMage;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] BlinkResponses;

    [SerializeField]
    AudioClip[] ManaVoidResponses;

    public int BlinkCooldown;
    [SerializeField]
    AudioClip BlinkAbilitySound;

    public int ManaVoidCooldown;
    [SerializeField]
    AudioClip ManaVoidAbilitySound;

    GameObject m_blinkButton;
    GameObject m_manaVoidButton;
    Image m_blinkImage;
    Image m_manaVoidImage;
    Animator m_antiMageAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        AntiMage = transform.Find("AntiMage").gameObject;
        m_antiMageAnimator = AntiMage.GetComponent<Animator>();
        m_audioSource = AntiMage.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("AntiMage/AbilitySound").GetComponent<AudioSource>();

        m_blinkButton = transform.Find("Buttons/StandBack/UpgradesCanvas/BlinkBack/BlinkBtn").gameObject;
        m_manaVoidButton = transform.Find("Buttons/StandBack/UpgradesCanvas/ManaVoidBack/ManaVoidBtn").gameObject;
        m_blinkImage = m_blinkButton.GetComponent<Image>();
        m_manaVoidImage = m_manaVoidButton.GetComponent<Image>();

        UpgradesController.BuyBlinkUpgrade += BuyBlinkUpgrade;
        UpgradesController.BuyManaVoidUpgrade += BuyManaVoidUpgrade;
        ManagersController.BuyAntiMageManager += BuyAntiMageManager;

        //turn to grey
        m_blinkImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_manaVoidImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyBlinkUpgrade()
    {
        BlinkUpgrade = true;
        Debug.Log("Bought Blink Upgrade");
        //turn to white
        m_blinkImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyManaVoidUpgrade()
    {
        ManaVoidUpgrade = true;
        Debug.Log("Bought ManaVoid Upgrade");
        //turn to white
        m_manaVoidImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyAntiMageManager()
    {
        Debug.Log("Bought AntiMage Manager");
        AntiMageManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = AntiMageManager;
    }

    public void ActivateBlink()
    {
        if (BlinkActive) return;
        Debug.Log("Activated Mana Void");
        BlinkActive = true;
        m_blinkImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useBlink");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, BlinkResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(BlinkAbilitySound);

        StartCoroutine(AbilityCooldown(BlinkCooldown, "Blink"));
    }

    public void ActivateManaVoid()
    {
        if (ManaVoidActive) return;
        Debug.Log("Activated Mana Void");
        ManaVoidActive = true;
        m_manaVoidImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useManaVoid");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, ManaVoidResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(ManaVoidAbilitySound);

        StartCoroutine(AbilityCooldown(ManaVoidCooldown, "ManaVoid"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Blink")
        {
            m_blinkImage.color = new Color(1f, 1f, 1f);
            BlinkActive = false;
        }
        else if (ability == "ManaVoid")
        {
            m_manaVoidImage.color = new Color(1f, 1f, 1f);
            ManaVoidActive = false;
        }
    }

    void ClickedButton(string name)
    {
        if (name == "AntiMageBuyStand")
        {
            m_antiMageAnimator.SetBool("isAttacking", false);
            RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "AntiMageBuyStand")
        {
            m_antiMageAnimator.SetBool("isAttacking", false);
        }
    }
}
