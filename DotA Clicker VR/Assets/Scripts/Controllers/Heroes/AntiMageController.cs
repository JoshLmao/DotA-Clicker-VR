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

    GameObject m_blinkButton;
    GameObject m_manaVoidButton;
    Image m_blinkImage;
    Image m_manaVoidImage;
    Animator m_antiMageAnimator;
    AudioSource m_audioSource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        AntiMage = transform.Find("AntiMage").gameObject;
        m_antiMageAnimator = AntiMage.GetComponent<Animator>();
        m_audioSource = AntiMage.GetComponent<AudioSource>();

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
    }

    void BuyManaVoidUpgrade()
    {
        ManaVoidUpgrade = true;
        Debug.Log("Bought ManaVoid Upgrade");
        //turn to white
        m_manaVoidImage.color = new Color(1f, 1f, 1f);
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
        Debug.Log("Activated Mana Void");
        m_blinkImage.color = new Color(0.275f, 0.275f, 0.275f);
        BlinkActive = true;

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useGodsStrength");
        RadiantClickerController.PlayRandomClip(m_audioSource, BlinkResponses);

        AbilityCooldown(180);

        m_blinkImage.color = new Color(1f, 1f, 1f);
        BlinkActive = false;
    }

    public void ActivateManaVoid()
    {
        Debug.Log("Activated Mana Void");
        m_manaVoidImage.color = new Color(0.275f, 0.275f, 0.275f);
        ManaVoidActive = true;

        //Do animation and voice line
        m_antiMageAnimator.SetTrigger("useGodsStrength");
        RadiantClickerController.PlayRandomClip(m_audioSource, ManaVoidResponses);

        AbilityCooldown(180);

        m_manaVoidImage.color = new Color(1f, 1f, 1f);
        ManaVoidActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
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
