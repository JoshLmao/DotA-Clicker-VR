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

    GameObject m_warCryButton;
    GameObject m_godsStrengthButton;
    Image m_warCryImage;
    Image m_godsStrengthImage;
    Animator m_svenAnimator;
    AudioSource m_audioSource;
    RadiantClickerController m_clickerController; 

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Sven = transform.Find("Sven").gameObject;
        m_svenAnimator = Sven.GetComponent<Animator>();
        m_audioSource = Sven.GetComponent<AudioSource>();

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
    }

    void BuyGodsStrengthUpgrade()
    {
        GodsStrengthUpgrade = true;
        Debug.Log("Bought GodsStrength Upgrade");
        //turn to white
        m_godsStrengthImage.color = new Color(1f, 1f, 1f);
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
        Debug.Log("Activated War Cry");
        m_warCryImage.color = new Color(0.275f, 0.275f, 0.275f);
        WarCryActive = true;

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useWarCry");
        RadiantClickerController.PlayRandomClip(m_audioSource, WarCryResponses);

        AbilityCooldown(180);

        m_warCryImage.color = new Color(1f, 1f, 1f);
        WarCryActive = false;
    }

    public void ActivateGodsStrength()
    {
        Debug.Log("Activated Gods Strength");
        m_godsStrengthImage.color = new Color(0.275f, 0.275f, 0.275f);
        GodsStrengthActive = true;

        //Do animation and voice line
        m_svenAnimator.SetTrigger("useGodsStrength");
        RadiantClickerController.PlayRandomClip(m_audioSource, GodsStrengthResponses);

        AbilityCooldown(180);

        m_godsStrengthImage.color = new Color(1f, 1f, 1f);
        GodsStrengthActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
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
