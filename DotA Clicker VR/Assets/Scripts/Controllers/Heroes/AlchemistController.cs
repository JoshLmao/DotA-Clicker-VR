using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AlchemistController : MonoBehaviour
{
    public bool GreevilsGreedUpgrade = false;
    public bool GreevilsGreedActive = false;

    public bool ChemicalRageUpgrade = false;
    public bool ChemicalRageActive = false;

    public bool AlchemistManager = false;
    public GameObject Alchemist;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] GreevilsGreedResponses;

    [SerializeField]
    AudioClip[] ChemicalRageResponses;

    public int GreevilsGreedCooldown;
    [SerializeField]
    AudioClip GreevilsGreedAbilitySound;

    public int ChemicalRageCooldown;
    [SerializeField]
    AudioClip ChemicalRageAbilitySound;

    GameObject m_greevilsGreedButton;
    GameObject m_chemicalRageButton;
    Image m_greevilsGreedImage;
    Image m_chemicalRageImage;
    Animator m_alcAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Alchemist = transform.Find("Alchemist").gameObject;
        m_alcAnimator = Alchemist.GetComponent<Animator>();
        m_audioSource = Alchemist.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Alchemist/AbilitySound").GetComponent<AudioSource>();

        m_greevilsGreedButton = transform.Find("Buttons/StandBack/UpgradesCanvas/GreevilsGreedBack/GreevilsGreedBtn").gameObject;
        m_chemicalRageButton = transform.Find("Buttons/StandBack/UpgradesCanvas/ChemicalRageBack/ChemicalRageBtn").gameObject;
        m_greevilsGreedImage = m_greevilsGreedButton.GetComponent<Image>();
        m_chemicalRageImage = m_chemicalRageButton.GetComponent<Image>();

        UpgradesController.BuyGreevilsGreedUpgrade += BuyGreevilsGreedUpgrade;
        UpgradesController.BuyChemicalRageUpgrade += BuyChemicalRageUpgrade;
        ManagersController.BuyAlchemistManager += BuyAlchemistManager;

        //turn to grey
        m_greevilsGreedImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_chemicalRageImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyGreevilsGreedUpgrade()
    {
        GreevilsGreedUpgrade = true;
        Debug.Log("Bought GreevilsGreed Upgrade");
        //turn to white
        m_greevilsGreedImage.color = new Color(1f, 1f, 1f);
    }

    void BuyChemicalRageUpgrade()
    {
        ChemicalRageUpgrade = true;
        Debug.Log("Bought ChemicalRage Upgrade");
        //turn to white
        m_chemicalRageImage.color = new Color(1f, 1f, 1f);
    }

    void BuyAlchemistManager()
    {
        Debug.Log("Bought Alchemist Manager");
        AlchemistManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = AlchemistManager;
    }

    public void ActivateGreevilsGreed()
    {
        if (GreevilsGreedActive) return;
        Debug.Log("Activated Greevils Greed");
        GreevilsGreedActive = true;
        m_greevilsGreedImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_alcAnimator.SetTrigger("useGreevilsGreed");
        RadiantClickerController.PlayRandomClip(m_audioSource, GreevilsGreedResponses);

        StartCoroutine(AbilityCooldown(GreevilsGreedCooldown, "GreevilsGreed"));
    }

    public void ActivateChemicalRage()
    {
        if (ChemicalRageActive) return;
        Debug.Log("Activated Chemical Rage");
        GreevilsGreedActive = true;
        m_chemicalRageImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_alcAnimator.SetBool("useChemicalRage", true);
        RadiantClickerController.PlayRandomClip(m_audioSource, ChemicalRageResponses);

        StartCoroutine(AbilityCooldown(ChemicalRageCooldown, "ChemicalRage"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "GreevilsGreed")
        {
            m_greevilsGreedImage.color = new Color(1f, 1f, 1f);
            GreevilsGreedActive = false;
        }
        else if (ability == "ChemicalRage")
        {
            m_chemicalRageImage.color = new Color(1f, 1f, 1f);
            GreevilsGreedActive = false;
            m_alcAnimator.SetBool("useChemicalRage", false);
        }
    }

    void ClickedButton(string name)
    {
        if(name == "AlchemistBuyStand")
        {
            m_alcAnimator.SetBool("isAttacking", true);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "AlchemistBuyStand")
        {
            m_alcAnimator.SetBool("isAttacking", false);
        }
    }
}
