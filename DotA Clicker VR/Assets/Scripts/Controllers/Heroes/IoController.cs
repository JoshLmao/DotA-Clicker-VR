using UnityEngine;
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

    GameObject m_overchargeButton;
    GameObject m_relocateButton;
    Image m_overchargeImage;
    Image m_relocateImage;
    Animator m_ioAnimator;
    AudioSource m_audioSource;
    RadiantClickerController m_clickerController;

    void Start()
    {
        m_clickerController = GetComponent<RadiantClickerController>();
        m_clickerController.OnClickedButton += ClickedButton;
        m_clickerController.OnClickedFinished += ClickedFinished;

        Io = transform.Find("Io").gameObject;
        m_ioAnimator = Io.GetComponent<Animator>();
        m_audioSource = Io.GetComponent<AudioSource>();

        m_overchargeButton = transform.Find("Buttons/StandBack/UpgradesCanvas/OverchargeBack/OverchargeBtn").gameObject;
        m_relocateButton = transform.Find("Buttons/StandBack/UpgradesCanvas/RelocateBack/RelocateBtn").gameObject;
        m_overchargeImage = m_overchargeButton.GetComponent<Image>();
        m_relocateImage = m_relocateButton.GetComponent<Image>();

        UpgradesController.BuyOverchargeUpgrade += BuyOverchargeUpgrade;
        UpgradesController.BuyRelocateUpgrade += BuyRelocateUpgrade;
        ManagersController.BuyIoManager += BuyIoManager;

        //turn to grey
        m_overchargeImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_relocateImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyOverchargeUpgrade()
    {
        OverchargeUpgrade = true;
        Debug.Log("Bought Overcharge Upgrade");
        //turn to white
        m_overchargeImage.color = new Color(1f, 1f, 1f);
    }

    void BuyRelocateUpgrade()
    {
        RelocateUpgrade = true;
        Debug.Log("Bought Relocate Upgrade");
        //turn to white
        m_relocateImage.color = new Color(1f, 1f, 1f);
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
        /** Overcharges Io to double his output for 30 seconds. Cooldown: 1 minute **/
        Debug.Log("Activated Overcharge");
        m_overchargeImage.color = new Color(0.275f, 0.275f, 0.275f);
        OverchargeActive = true;

        AbilityCooldown(60);

        m_relocateImage.color = new Color(1f, 1f, 1f);
        OverchargeActive = false;
    }

    public void ActivateRelocate()
    {
        /** Quadruples Io's click amount for 20 seconds. Cooldown: 3 minutes **/
        Debug.Log("Activated Relocate");
        m_relocateImage.color = new Color(0.275f, 0.275f, 0.275f);
        OverchargeActive = true;

        AbilityCooldown(180);

        m_relocateImage.color = new Color(1f, 1f, 1f);
        OverchargeActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
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
