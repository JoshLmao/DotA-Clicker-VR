using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SvenController : MonoBehaviour
{
    public bool WarCryUpgrade = false;
    public bool WarCryActive = false;

    public bool GodsStrengthUpgrade = false;
    public bool GodsStrengthActive = false;

    public bool SvenManager = false;
    public GameObject Sven;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] WarCryResponses;

    [SerializeField]
    AudioClip[] GodsStrengthResponses;

    public int WarCryCooldown;
    [SerializeField]
    AudioClip WarCryAbilitySound;

    public int GodsStrengthCooldown;
    [SerializeField]
    AudioClip GodsStrengthAbilitySound;

    GameObject m_warCryButton;
    GameObject m_godsStrengthButton;
    Image m_warCryImage;
    Image m_godsStrengthImage;
    Animator m_svenAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController; 

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Sven = transform.Find("Sven").gameObject;
        m_svenAnimator = Sven.GetComponent<Animator>();
        m_audioSource = Sven.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Sven/AbilitySound").GetComponent<AudioSource>();

        m_warCryButton = transform.Find("Buttons/StandBack/UpgradesCanvas/WarCryBack/WarCryBtn").gameObject;
        m_godsStrengthButton = transform.Find("Buttons/StandBack/UpgradesCanvas/GodsStrengthBack/GodsStrengthBtn").gameObject;
        m_warCryImage = m_warCryButton.GetComponent<Image>();
        m_godsStrengthImage = m_godsStrengthButton.GetComponent<Image>();

        UpgradesController.BuyWarCryUpgrade += BuyWarCryUpgrade;
        UpgradesController.BuyGodsStrengthUpgrade += BuyGodsStrengthUpgrade;
        ManagersController.BuySvenManager += BuySvenManager;

        //turn to grey
        m_warCryImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_godsStrengthImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyWarCryUpgrade()
    {
        WarCryUpgrade = true;
        Debug.Log("Bought WarCry Upgrade");
        //turn to white
        m_warCryImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyGodsStrengthUpgrade()
    {
        GodsStrengthUpgrade = true;
        Debug.Log("Bought GodsStrength Upgrade");
        //turn to white
        m_godsStrengthImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuySvenManager()
    {
        Debug.Log("Bought Sven Manager");
        SvenManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = SvenManager;
    }

    public void ActivateWarCry()
    {
        if (WarCryActive) return;
        Debug.Log("Activated War Cry");
        WarCryActive = true;
        m_warCryImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useWarCry");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, WarCryResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(WarCryAbilitySound);

        StartCoroutine(AbilityCooldown(WarCryCooldown, "WarCry"));
    }

    public void ActivateGodsStrength()
    {
        if (GodsStrengthActive) return;
        Debug.Log("Activated Gods Strength");
        m_godsStrengthImage.color = new Color(0.275f, 0.275f, 0.275f);
        GodsStrengthActive = true;

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useGodsStrength");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, GodsStrengthResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(GodsStrengthAbilitySound);

        StartCoroutine(AbilityCooldown(GodsStrengthCooldown, "GodsStrength"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "WarCry")
        {
            m_warCryImage.color = new Color(1f, 1f, 1f);
            WarCryActive = false;
        }
        else if (ability == "GodsStrength")
        {
            m_godsStrengthImage.color = new Color(1f, 1f, 1f);
            GodsStrengthActive = false;
        }
    }

    void ClickedButton(string name)
    {
        if(name == "SvenBuyStand")
        {
            m_svenAnimator.SetBool("isAttacking", true);
            RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "AlchemistBuyStand")
        {
            m_svenAnimator.SetBool("isAttacking", false);
        }
    }
}
