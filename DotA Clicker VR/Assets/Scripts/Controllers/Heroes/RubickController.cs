using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RubickController : MonoBehaviour
{
    public bool NullFieldUpgrade = false;
    public bool SpellStealUpgrade = false;

    GameObject m_nullFieldButton;
    GameObject m_relocateButton;
    Image m_nullFieldImage;
    Image m_spellStealImage;

    void Start()
    {
        m_nullFieldButton = transform.Find("Buttons/StandBack/UpgradesCanvas/NullFieldBack/NullFieldBtn").gameObject;
        m_relocateButton = transform.Find("Buttons/StandBack/UpgradesCanvas/SpellStealBack/SpellStealBtn").gameObject;
        m_nullFieldImage = m_nullFieldButton.GetComponent<Image>();
        m_spellStealImage = m_relocateButton.GetComponent<Image>();

        UpgradesController.BuyNullFieldUpgrade += BuyNullFieldUpgrade;
        UpgradesController.BuySpellStealUpgrade += BuySpellStealUpgrade;

        //turn to grey
        m_nullFieldImage.color = new Color(0.275f, 0.275f, 0.275f);
        m_spellStealImage.color = new Color(0.275f, 0.275f, 0.275f);
    }

    void BuyNullFieldUpgrade()
    {
        NullFieldUpgrade = true;
        Debug.Log("Bought nullField Upgrade");
        //turn to white
        m_nullFieldImage.color = new Color(1f, 1f, 1f);
    }

    void BuySpellStealUpgrade()
    {
        SpellStealUpgrade = true;
        Debug.Log("Bought Relocate Upgrade");
        //turn to white
        m_spellStealImage.color = new Color(1f, 1f, 1f);
    }
}
