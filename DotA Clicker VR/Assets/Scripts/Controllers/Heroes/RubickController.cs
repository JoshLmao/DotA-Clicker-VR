using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RubickController : MonoBehaviour
{
    public bool TelekinesisUpgrade = false;
    public bool TelekinesisActive = false;

    public bool SpellStealUpgrade = false;
    public bool SpellStealActive = false;

    public bool RubickManager = false;
    public GameObject RubickModel;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] TelekinesisResponses;

    [SerializeField]
    AudioClip[] SpellStealResponses;

    [SerializeField]
    AudioClip TelekinesisAbilitySound;
    [SerializeField]
    AudioClip SpellStealAbilitySound;

    GameObject m_TelekinesisButton;
    GameObject m_relocateButton;
    Image m_TelekinesisImage;
    Image m_spellStealImage;
    Animator m_rubickAnimator;
    AudioSource m_audioSource;
    RadiantClickerController m_clickerController; //For Events

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        RubickModel = transform.Find("Rubick").gameObject;
        m_rubickAnimator = RubickModel.GetComponent<Animator>();
        m_audioSource = RubickModel.GetComponent<AudioSource>();

        m_TelekinesisButton = transform.Find("Buttons/StandBack/UpgradesCanvas/TelekinesisBack/TelekinesisBtn").gameObject;
        m_relocateButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SpellStealBack/SpellStealBtn").gameObject;
        m_TelekinesisImage = m_TelekinesisButton.GetComponent<Image>();
        m_spellStealImage = m_relocateButton.GetComponent<Image>();

        UpgradesController.BuyTelekinesisUpgrade += BuyTelekinesisUpgrade;
        UpgradesController.BuySpellStealUpgrade += BuySpellStealUpgrade;
        ManagersController.BuyRubickManager += BuyRubickManager;

        //turn to grey
        m_TelekinesisImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_spellStealImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyTelekinesisUpgrade()
    {
        TelekinesisUpgrade = true;
        Debug.Log("Bought Telekinesis Upgrade");
        //turn to white
        m_TelekinesisImage.color = new Color(1f, 1f, 1f);
    }

    void BuySpellStealUpgrade()
    {
        SpellStealUpgrade = true;
        Debug.Log("Bought Relocate Upgrade");
        //turn to white
        m_spellStealImage.color = new Color(1f, 1f, 1f);
    }

    void BuyRubickManager()
    {
        Debug.Log("Bought Rubick Manager");
        RubickManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = RubickManager;
    }

    public void ActivateTelekinesis()
    {
        Debug.Log("Activated Telekinesis");
        m_TelekinesisImage.color = new Color(0.275f, 0.275f, 0.275f);
        TelekinesisActive = true;

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useTelekinesis");
        RadiantClickerController.PlayRandomClip(m_audioSource, TelekinesisResponses);
        
        AbilityCooldown(180);

        m_TelekinesisImage.color = new Color(1f, 1f, 1f);
        TelekinesisActive = false;
    }

    public void ActivateSpellSteal()
    {
        Debug.Log("Activated Spell Steal");
        m_spellStealImage.color = new Color(0.275f, 0.275f, 0.275f);
        SpellStealActive = true;

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useSpellSteal");
        RadiantClickerController.PlayRandomClip(m_audioSource, SpellStealResponses);

        AbilityCooldown(180);

        m_spellStealImage.color = new Color(1f, 1f, 1f);
        SpellStealActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }

    void ClickedButton(string name)
    {
        if(name == "RubickBuyStand")
        {
            m_rubickAnimator.SetBool("isAttacking", true);
            RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if(name == "RubickBuyStand")
        {
            m_rubickAnimator.SetBool("isAttacking", false);
        }
    }
}
