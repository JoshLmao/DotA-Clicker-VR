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

    GameObject m_snowballButton;
    GameObject m_walrusPunchButton;
    Image m_snowballImage;
    Image m_walrusPunchImage;

    void Start()
    {
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
    }

    void BuyWalrusPunchUpgrade()
    {
        WalrusPunchUpgrade = true;
        Debug.Log("Bought WalrusPunch Upgrade");
        //turn to white
        m_walrusPunchImage.color = new Color(1f, 1f, 1f);
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
        Debug.Log("Activated Snowball");
        m_snowballImage.color = new Color(0.275f, 0.275f, 0.275f);
        SnowballActive = true;

        AbilityCooldown(180);

        m_snowballImage.color = new Color(1f, 1f, 1f);
        SnowballActive = false;
    }

    public void ActivateWalrusPunch()
    {
        Debug.Log("Activated Walrus Punch");
        m_walrusPunchImage.color = new Color(0.275f, 0.275f, 0.275f);
        WalrusPunchActive = true;

        AbilityCooldown(180);

        m_walrusPunchImage.color = new Color(1f, 1f, 1f);
        WalrusPunchActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
