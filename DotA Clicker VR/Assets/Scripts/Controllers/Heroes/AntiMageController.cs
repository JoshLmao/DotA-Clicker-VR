using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AntiMageController : MonoBehaviour
{
    public bool BlinkUpgrade = false;
    public bool ManaVoidUpgrade = false;

    GameObject m_blinkButton;
    GameObject m_manaVoidButton;
    Image m_blinkImage;
    Image m_manaVoidImage;

    void Start()
    {
        m_blinkButton = transform.Find("Buttons/StandBack/UpgradesCanvas/BlinkBack/BlinkBtn").gameObject;
        m_manaVoidButton = transform.Find("Buttons/StandBack/UpgradesCanvas/ManaVoidBack/ManaVoidBtn").gameObject;
        m_blinkImage = m_blinkButton.GetComponent<Image>();
        m_manaVoidImage = m_manaVoidButton.GetComponent<Image>();

        UpgradesController.BuyBlinkUpgrade += BuyBlinkUpgrade;
        UpgradesController.BuyManaVoidUpgrade += BuyManaVoidUpgrade;

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
}
