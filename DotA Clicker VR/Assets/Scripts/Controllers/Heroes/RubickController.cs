using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RubickController : MonoBehaviour
{
    public bool NullFieldUpgrade = false;
    public bool NullFieldActive = false;

    public bool SpellStealUpgrade = false;
    public bool SpellStealActive = false;

    public bool RubickManager = false;

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
        ManagersController.BuyRubickManager += BuyRubickManager;

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

    void BuyRubickManager()
    {
        Debug.Log("Bought Rubick Manager");
        RubickManager = true;
        RadiantClickerController clicker = this.GetComponent<RadiantClickerController>();
        clicker.HasManager = RubickManager;
    }

    public void ActivateNullField()
    {
        Debug.Log("Activated Null Field");
        m_nullFieldImage.color = new Color(0.275f, 0.275f, 0.275f);
        NullFieldActive = true;

        AbilityCooldown(180);

        m_nullFieldImage.color = new Color(1f, 1f, 1f);
        NullFieldActive = false;
    }

    public void ActivateSpellSteal()
    {
        Debug.Log("Activated Spell Steal");
        m_spellStealImage.color = new Color(0.275f, 0.275f, 0.275f);
        SpellStealActive = true;

        AbilityCooldown(180);

        m_spellStealImage.color = new Color(1f, 1f, 1f);
        SpellStealActive = false;
    }

    IEnumerator AbilityCooldown(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
