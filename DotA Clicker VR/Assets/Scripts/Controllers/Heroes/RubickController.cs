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

    public int TelekinesisCooldown;
    [SerializeField]
    AudioClip TelekinesisAbilitySound;

    public int SpellStealCooldown;
    [SerializeField]
    AudioClip SpellStealAbilitySound;

    GameObject m_TelekinesisButton;
    GameObject m_relocateButton;
    Image m_TelekinesisImage;
    Image m_spellStealImage;
    Animator m_rubickAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController; //For Events

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        RubickModel = transform.Find("Rubick").gameObject;
        m_rubickAnimator = RubickModel.GetComponent<Animator>();
        m_audioSource = RubickModel.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Rubick/AbilitySound").GetComponent<AudioSource>();

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
        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuySpellStealUpgrade()
    {
        SpellStealUpgrade = true;
        Debug.Log("Bought Relocate Upgrade");
        //turn to white
        m_spellStealImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
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
        if (TelekinesisActive) return;
        Debug.Log("Activated Telekinesis");
        TelekinesisActive = true;
        m_TelekinesisImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useTelekinesis");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, TelekinesisResponses);

        if (!m_abilitySource.isPlaying)
            m_audioSource.PlayOneShot(TelekinesisAbilitySound);

        StartCoroutine(AbilityCooldown(TelekinesisCooldown, "Telekinesis"));
    }

    public void ActivateSpellSteal()
    {
        Debug.Log("Activated Spell Steal");
        m_spellStealImage.color = new Color(0.275f, 0.275f, 0.275f);
        SpellStealActive = true;

        //Do animation and voice line
        m_rubickAnimator.SetTrigger("useSpellSteal");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, SpellStealResponses);

        if (!m_abilitySource.isPlaying)
            m_audioSource.PlayOneShot(SpellStealAbilitySound);

        StartCoroutine(AbilityCooldown(SpellStealCooldown, "SpellSteal"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Telekinesis")
        {
            m_TelekinesisImage.color = new Color(1f, 1f, 1f);
            TelekinesisActive = false;
        }
        else if (ability == "SpellSteal")
        {
            m_spellStealImage.color = new Color(1f, 1f, 1f);
            SpellStealActive = false;
        }
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
