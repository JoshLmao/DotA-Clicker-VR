using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradesController : MonoBehaviour
{
    public List<UpgradeDto> Upgrades { get; set; }

    public delegate void OnBuyOverchargeUpgrade();
    public delegate void OnBuyRelocateUpgrade();
    public delegate void OnBuyTelekinesisUpgrade();
    public delegate void OnBuySpellStealUpgrade();
    public delegate void OnBuyFireblastUpgrade();
    public delegate void OnBuyBloodlustUpgrade();
    public delegate void OnBuySnowballUpgrade();
    public delegate void OnBuyWalrusPunchUpgrade();
    public delegate void OnBuySunrayUpgrade();
    public delegate void OnBuySupernovaUpgrade();
    public delegate void OnBuyWarCryUpgrade();
    public delegate void OnBuyGodStrengthUpgrade();
    public delegate void OnBuyBlinkUpgrade();
    public delegate void OnBuyManaVoidUpgrade();
    public delegate void OnBuyGreevilsGreedUpgrade();
    public delegate void OnBuyChemicalRageUpgrade();

    public static event OnBuyOverchargeUpgrade BuyOverchargeUpgrade;
    public static event OnBuyRelocateUpgrade BuyRelocateUpgrade;
    public static event OnBuyTelekinesisUpgrade BuyTelekinesisUpgrade;
    public static event OnBuySpellStealUpgrade BuySpellStealUpgrade;
    public static event OnBuyFireblastUpgrade BuyFireblastUpgrade;
    public static event OnBuyBloodlustUpgrade BuyBloodlustUpgrade;
    public static event OnBuySnowballUpgrade BuySnowballUpgrade;
    public static event OnBuyWalrusPunchUpgrade BuyWalrusPunchUpgrade;
    public static event OnBuySunrayUpgrade BuySunrayUpgrade;
    public static event OnBuySupernovaUpgrade BuySupernovaUpgrade;
    public static event OnBuyWarCryUpgrade BuyWarCryUpgrade;
    public static event OnBuyGodStrengthUpgrade BuyGodsStrengthUpgrade;
    public static event OnBuyBlinkUpgrade BuyBlinkUpgrade;
    public static event OnBuyManaVoidUpgrade BuyManaVoidUpgrade;
    public static event OnBuyGreevilsGreedUpgrade BuyGreevilsGreedUpgrade;
    public static event OnBuyChemicalRageUpgrade BuyChemicalRageUpgrade;

    [SerializeField]
    GameObject UpgradePrefab;
    List<GameObject> UpgradeObjects { get; set; }
    RadiantSceneController m_sceneController;

	void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        Upgrades = new List<UpgradeDto>();
        AddDefaultUpgrades();

        RefreshUpgradesList();
	}
	
	void Update ()
    {
	
	}

    void AddDefaultUpgrades()
    {
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Overcharge",
            Description = "Overcharges Io to double his output for 30 seconds. Cooldown: 45 seconds",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Io_Overcharge"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Relocate",
            Description = "Quadruples Io's click amount for 20 seconds. Cooldown: 1.5 minutes",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Io_Relocate"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Telekinesis",
            Description = "Rubick lifts his click amount by 2 for 30 seconds. Cooldown: 1 minute",
            HeroUpgrade = "Rubick",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Rubick_Telekinesis"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Spell Steal",
            Description = "Rubick steals another heroes click amount for one click. Cooldown: 3 minutes",
            HeroUpgrade = "Rubick",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Rubick_SpellSteal"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Fireblast",
            Description = "The Ogre Magi blasts a wave of fire giving 3x his click amount for 45 seconds. Cooldown: 5 minutes",
            HeroUpgrade = "Ogre Magi",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/OgreMagi_Fireblast"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Bloodlust",
            Description = "Incites a frenzy in the Magi, decreasing his click duration by 30 seconds. Cooldown: 3.5 minutes",
            HeroUpgrade = "Ogre Magi",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/OgreMagi_Bloodlust"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Snowball",
            Description = "Tusk snowballs his click amount by 2. Cooldown: 4 minutes minutes",
            HeroUpgrade = "Tusk",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Tusk_Snowball"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Walrus Punch",
            Description = "Tusk connects with his mighty fist and gives you a bonus click. Cooldown: 7 minutes",
            HeroUpgrade = "Tusk",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Tusk_WalrusPunch"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Sunray",
            Description = "A beam of light powerful enough to decrease all cooldowns by a minute. Cooldown: 7 minutes",
            HeroUpgrade = "Phoenix",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Phoenix_SunRay"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Supernova",
            Description = "Completes a click every second of Supernova's duration. Cooldown: 10 minutes",
            HeroUpgrade = "Phoenix",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Phoenix_Supernova"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "War Cry",
            Description = "Decreases each clickers duration by 1/4. Cooldown: 7 minutes",
            HeroUpgrade = "Sven",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Sven_WarCry"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "God's Strength",
            Description = "Sven channels his rogue strength, increasing his teammates click amount by 2 for 30 seconds. Cooldown: 15 minutes",
            HeroUpgrade = "Sven",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Sven_GodsStrength"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Blink",
            Description = "In a blink, Anti Mage gives you a click for free. Cooldown: 2 minutes",
            HeroUpgrade = "Anti Mage",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/AntiMage_Blink"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Mana Void",
            Description = "For each second missing from AM's current click duration, the surrounding heroes get that duration taken off their current time. Cooldown: 10 minutes",
            HeroUpgrade = "Anti Mage",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/AntiMage_ManaVoid"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Greevil's Greed",
            Description = "For every attack, the Alchemist reduces his click duration by 20 seconds, lasts 1 minute. Cooldown: 20 minutes",
            HeroUpgrade = "Alchemist",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Alchemist_GreevilsGreed"),
            Cost = 0,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Chemical Rage",
            Description = "The Alchemist causes his Ogre to enter a chemically induced rage reducing his current click by 3/4. Cooldown: 30 minutes",
            HeroUpgrade = "Alchemist",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Alchemist_ChemicalRage"),
            Cost = 0,
        });

    }

    void RefreshUpgradesList()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int addY = -30; //Hack to get positioning of List UI
        foreach (UpgradeDto upgrade in Upgrades)
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
            UpgradeDto clickedUpgrade = upgrade; //Fix for AddListener adding current upgrade to each button click
            button.onClick.AddListener(delegate { AddUpgrade(clickedUpgrade); });

            //UpgradeObjects.Add(newUpgrade);
        }
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
        else if(upgrade.Name == "Relocate")
        {
            Debug.Log("Clicked Relocate");
            BuyRelocateUpgrade();
        }
        else if (upgrade.Name == "Telekinesis")
        {
            Debug.Log("Clicked Telekinesis");
            BuyTelekinesisUpgrade(); 
        }
        else if (upgrade.Name == "Spell Steal")
        {
            Debug.Log("Clicked Spell Steal");
            BuySpellStealUpgrade(); 
        }
        else if (upgrade.Name == "Fireblast")
        {
            Debug.Log("Clicked Fireblast");
            BuyFireblastUpgrade(); 
        }
        else if (upgrade.Name == "Bloodlust")
        {
            Debug.Log("Clicked Bloodlust");
            BuyBloodlustUpgrade(); 
        }
        else if (upgrade.Name == "Snowball")
        {
            Debug.Log("Clicked Snowball");
            BuySnowballUpgrade(); 
        }
        else if (upgrade.Name == "Walrus Punch")
        {
            Debug.Log("Clicked Walrus Punch");
            BuyWalrusPunchUpgrade(); 
        }
        else if (upgrade.Name == "Sunray")
        {
            Debug.Log("Clicked Sunray");
            BuySunrayUpgrade(); 
        }
        else if (upgrade.Name == "Supernova")
        {
            Debug.Log("Clicked Supernova");
            BuySupernovaUpgrade(); 
        }
        else if (upgrade.Name == "War Cry")
        {
            Debug.Log("Clicked War Cry");
            BuyWarCryUpgrade(); 
        }
        else if (upgrade.Name == "God's Strength")
        {
            Debug.Log("Clicked God's Strength");
            BuyGodsStrengthUpgrade(); 
        }
        else if (upgrade.Name == "Blink")
        {
            Debug.Log("Clicked Blink");
            BuyBlinkUpgrade(); 
        }
        else if (upgrade.Name == "Mana Void")
        {
            Debug.Log("Clicked Mana Void");
            BuyManaVoidUpgrade(); 
        }
        else if (upgrade.Name == "Greevil's Greed")
        {
            Debug.Log("Clicked Greevil's Greed");
            BuyGreevilsGreedUpgrade(); 
        }
        else if (upgrade.Name == "Chemical Rage")
        {
            Debug.Log("Clicked Chemical Rage");
            BuyChemicalRageUpgrade(); 
        }

        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/buy"));
        Upgrades.RemoveAll(x => x.Name == upgrade.Name);
        RefreshUpgradesList();
    }
}
