using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TuskController : MonoBehaviour
{
    public bool SnowballUpgrade = false;
    public bool SnowballActive = false;

    public bool WalrusPunchUpgrade = false;
    public bool WalrusPunchActive = false;

    public bool TuskManager = false;
    public GameObject Tusk;

    [SerializeField]
    AudioClip[] AttackingResponses;

    [SerializeField]
    AudioClip[] SnowballResponses;

    [SerializeField]
    AudioClip[] WalrusPunchResponses;

    public int SnowballCooldown;
    [SerializeField]
    AudioClip SnowballAbilitySound;

    public int WalrusPunchCooldown;
    [SerializeField]
    AudioClip WalrusPunchAbilitySound;

    GameObject m_snowballButton;
    GameObject m_walrusPunchButton;
    Image m_snowballImage;
    Image m_walrusPunchImage;
    Animator m_tuskAnimator;
    AudioSource m_audioSource;
    AudioSource m_abilitySource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Tusk = transform.Find("Tusk").gameObject;
        m_tuskAnimator = Tusk.GetComponent<Animator>();
        m_audioSource = Tusk.GetComponent<AudioSource>();
        m_abilitySource = GameObject.Find("Tusk/AbilitySound").GetComponent<AudioSource>();

        m_snowballButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SnowballBack/SnowballBtn").gameObject;
        m_walrusPunchButton = transform.Find("Buttons/StandBack/UpgradesCanvas/WalrusPunchBack/WalrusPunchBtn").gameObject;
        m_snowballImage = m_snowballButton.GetComponent<Image>();
        m_walrusPunchImage = m_walrusPunchButton.GetComponent<Image>();

        UpgradesController.BuySnowballUpgrade += BuySnowballUpgrade;
        UpgradesController.BuyWalrusPunchUpgrade += BuyWalrusPunchUpgrade;
        ManagersController.BuyTuskManager += BuyTuskManager;

        //turn to grey
        m_snowballImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_walrusPunchImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuySnowballUpgrade()
    {
        SnowballUpgrade = true;
        Debug.Log("Bought Snowball Upgrade");
        //turn to white
        m_snowballImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability1Level = 1;
        m_clickerController.ResetLevelIcons("1");
    }

    void BuyWalrusPunchUpgrade()
    {
        WalrusPunchUpgrade = true;
        Debug.Log("Bought WalrusPunch Upgrade");
        //turn to white
        m_walrusPunchImage.color = new Color(1f, 1f, 1f);
        m_clickerController.Ability2Level = 1;
        m_clickerController.ResetLevelIcons("2");
    }

    void BuyTuskManager()
    {
        Debug.Log("Bought Tusk Manager");
        TuskManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = TuskManager;
    }

    public void ActivateSnowball()
    {
        if (SnowballActive) return;
        Debug.Log("Activated Snowball");
        SnowballActive = true;
        m_snowballImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_tuskAnimator.SetTrigger("useSupernova");
        if(!m_audioSource.isPlaying)
             RadiantClickerController.PlayRandomClip(m_audioSource, SnowballResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(SnowballAbilitySound);

        StartCoroutine(AbilityCooldown(SnowballCooldown, "Snowball"));
    }

    public void ActivateWalrusPunch()
    {
        if (WalrusPunchActive) return;
        Debug.Log("Activated Walrus Punch");
        WalrusPunchActive = true;
        m_walrusPunchImage.color = new Color(0.275f, 0.275f, 0.275f);

        //Do animation and voice line
        m_tuskAnimator.SetTrigger("useSupernova");
        if(!m_audioSource.isPlaying)
            RadiantClickerController.PlayRandomClip(m_audioSource, WalrusPunchResponses);

        if (!m_abilitySource.isPlaying)
            m_abilitySource.PlayOneShot(WalrusPunchAbilitySound);

        StartCoroutine(AbilityCooldown(WalrusPunchCooldown, "WalrusPunch"));
    }

    IEnumerator AbilityCooldown(float time, string ability)
    {
        yield return new WaitForSeconds(time);

        if (ability == "Snowball")
        {
            m_walrusPunchImage.color = new Color(1f, 1f, 1f);
            WalrusPunchActive = false;
        }
        else if (ability == "WalrusPunch")
        {
            m_snowballImage.color = new Color(1f, 1f, 1f);
            SnowballActive = false;
        }
    }

    void ClickedButton(string name)
    {
        if (name == "TuskBuyStand")
        {
            m_tuskAnimator.SetBool("isAttacking", true);
            RadiantClickerController.PlayRandomClip(m_audioSource, AttackingResponses);
        }
    }

    void ClickedFinished(string name)
    {
        if (name == "TuskBuyStand")
        {
            m_tuskAnimator.SetBool("isAttacking", false);
        }
    }
}
