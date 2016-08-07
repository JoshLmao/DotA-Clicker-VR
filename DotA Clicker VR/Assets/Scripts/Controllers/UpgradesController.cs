using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradesController : MonoBehaviour
{
    public List<UpgradeDto> Upgrades { get; set; }

    public delegate void OnBuyOverchargeUpgrade();
    public static event OnBuyOverchargeUpgrade BuyOverchargeUpgrade;

    public delegate void OnBuyRelocateUpgrade();
    public static event OnBuyRelocateUpgrade BuyRelocateUpgrade;

    [SerializeField]
    GameObject UpgradePrefab;

    RadiantSceneController m_sceneController;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        Upgrades = new List<UpgradeDto>();
        RefreshUpgrades();

        int addY = -30; //Hack to get positioning of List UI

        foreach(UpgradeDto upgrade in Upgrades)
        {
            GameObject newUpgrade = GameObject.Instantiate(UpgradePrefab);
            newUpgrade.transform.SetParent(this.transform, false);
            newUpgrade.name = upgrade.Name;

            newUpgrade.GetComponent<RectTransform>().localPosition = new Vector3(newUpgrade.transform.localPosition.x, addY, newUpgrade.transform.localPosition.z);
            addY -= 275; //Space between each UI

            Image icon = newUpgrade.transform.Find("UpgradeImage").GetComponent<Image>();
            icon.sprite = upgrade.Image;
            Text upgradeName = newUpgrade.transform.Find("UpgradeName").GetComponent<Text>();
            upgradeName.text = upgrade.Name + " - " + upgrade.HeroUpgrade;
            Text description = newUpgrade.transform.Find("UpgradeDesc").GetComponent<Text>();
            description.text = upgrade.Description;
            Text upgradeCost = newUpgrade.transform.Find("BuyButton/CostCanvas/GoldCost").GetComponent<Text>();
            upgradeCost.text = upgrade.Cost + " gold";
            Button button = newUpgrade.transform.Find("BuyButton").GetComponent<Button>();
            UpgradeDto clickedUpgrade = upgrade; //Fix for AddListener adding last upgrade to each button click
            button.onClick.AddListener(delegate { AddUpgrade(clickedUpgrade); });
        }
	}
	
	void Update ()
    {
	
	}

    void RefreshUpgrades()
    {
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Overcharge",
            Description = "Overcharges Io to double his output for 30 seconds. Cooldown: 1 minute",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Io_Overcharge"),
            Cost = 2000,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Relocate",
            Description = "Quadruples Io's click amount for 20 seconds. Cooldown: 3 minutes",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Io_Relocate"),
            Cost = 6000,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Null Field",
            Description = "",
            HeroUpgrade = "Rubick",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Rubick_NullField"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Spell Steal",
            Description = "",
            HeroUpgrade = "Rubick",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Rubick_SpellSteal"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Fireblast",
            Description = "",
            HeroUpgrade = "Ogre Magi",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/OgreMagi_Fireblast"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Bloodlust",
            Description = "",
            HeroUpgrade = "Ogre Magi",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/OgreMagi_Bloodlust"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Snowball",
            Description = "",
            HeroUpgrade = "Tusk",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Tusk_Snowball"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Walrus Punch",
            Description = "",
            HeroUpgrade = "Tusk",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Tusk_WalrusPunch"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Sunray",
            Description = "",
            HeroUpgrade = "Phoenix",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Phoenix_SunRay"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Supernova",
            Description = "",
            HeroUpgrade = "Phoenix",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Phoenix_Supernova"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "War Cry",
            Description = "",
            HeroUpgrade = "Sven",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Sven_WarCry"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "God's Strength",
            Description = "",
            HeroUpgrade = "Sven",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Sven_GodsStrength"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Blink",
            Description = "In a blink, Anti Mage gives you a click for free! Cooldown: 0 mins",
            HeroUpgrade = "Anti Mage",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/AntiMage_Blink"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Mana Void",
            Description = "",
            HeroUpgrade = "Anti Mage",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/AntiMage_ManaVoid"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Greevil's Greed",
            Description = "",
            HeroUpgrade = "Alchemist",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Alchemist_GreevilsGreed"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Chemical Rage",
            Description = "The Alchemist causes his Ogre to enter a chemically induced rage giving you ... Cooldown: 0",
            HeroUpgrade = "Alchemist",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Alchemist_ChemicalRage"),
            Cost = 0,
        });

    }

    void AddUpgrade(UpgradeDto upgrade)
    {
        if (m_sceneController.TotalGold < upgrade.Cost)
        {
            Debug.Log("Can't buy upgrade '" + upgrade.Name + "'");
            return;
        }

        if (upgrade.Name == "Overcharge")
        {
            Debug.Log("Clicked IO Overcharge");
            BuyOverchargeUpgrade(); //Invoke Event
        }
        else
        {
            Debug.Log("Adding other Upgrade");
        }
    }
}
