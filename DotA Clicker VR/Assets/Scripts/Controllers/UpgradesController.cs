using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradesController : MonoBehaviour
{
    public List<UpgradeDto> Upgrades { get; set; }

    public delegate void OnBuyCrystalNovaUpgrade(int level);
    public delegate void OnBuyFrostbiteUpgrade(int level);
    public delegate void OnBuyTelekinesisUpgrade(int level);
    public delegate void OnBuySpellStealUpgrade(int level);
    public delegate void OnBuyFireblastUpgrade(int level);
    public delegate void OnBuyBloodlustUpgrade(int level);
    public delegate void OnBuySnowballUpgrade(int level);
    public delegate void OnBuyWalrusPunchUpgrade(int level);
    public delegate void OnBuySunrayUpgrade(int level);
    public delegate void OnBuySupernovaUpgrade(int level);
    public delegate void OnBuyWarCryUpgrade(int level);
    public delegate void OnBuyGodStrengthUpgrade(int level);
    public delegate void OnBuyBlinkUpgrade(int level);
    public delegate void OnBuyManaVoidUpgrade(int level);
    public delegate void OnBuyGreevilsGreedUpgrade(int level);
    public delegate void OnBuyChemicalRageUpgrade(int level);

    public static event OnBuyCrystalNovaUpgrade BuyCrystalNovaUpgrade;
    public static event OnBuyFrostbiteUpgrade BuyFrostbiteUpgrade;
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

    bool isOnMainMenu = false;
    RadiantSceneController m_sceneController;

    bool m_cmAbil1, m_cmAbil2, m_rubickAbil1, m_rubickAbil2, m_ogreAbil1, m_ogreAbil2, m_tuskAbil1, m_tuskAbil2, m_phoenixAbil1, m_phoenixAbil2, m_svenAbil1, m_svenAbil2, m_antiAbil1, m_antiAbil2, m_alcAbil1, m_alcAbil2;

    void Start ()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            isOnMainMenu = true;
        }
        else
        {
            m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        }

        Upgrades = new List<UpgradeDto>();
        AddDefaultUpgrades();

        RefreshUpgradesList();

        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
	}
	
	void Update ()
    {
	
	}

    void AddDefaultUpgrades()
    {
        if(!m_cmAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Crystal Nova",
                Description = "Overcharges Io to double his output for 30 seconds. Cooldown: 45 seconds",
                HeroUpgrade = "Crystal Maiden",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/CM_CrystalNova"),
                Cost = 0,
            });
        }
        if(!m_cmAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Frostbite",
                Description = "Quadruples Io's click amount for 20 seconds. Cooldown: 1.5 minutes",
                HeroUpgrade = "Crystal Maiden",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/CM_Frostbite"),
                Cost = 0,
            });
        }
        if(!m_rubickAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Telekinesis",
                Description = "Rubick lifts his click amount by 2 for 30 seconds. Cooldown: 1 minute",
                HeroUpgrade = "Rubick",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Rubick_Telekinesis"),
                Cost = 0,
            });
        }
        if(!m_rubickAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Spell Steal",
                Description = "Rubick steals another heroes click amount for one click. Cooldown: 3 minutes",
                HeroUpgrade = "Rubick",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Rubick_SpellSteal"),
                Cost = 0,
            });
        }
        if(!m_ogreAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Fireblast",
                Description = "The Ogre Magi blasts a wave of fire giving 3x his click amount for 45 seconds. Cooldown: 5 minutes",
                HeroUpgrade = "Ogre Magi",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/OgreMagi_Fireblast"),
                Cost = 0,
            });
        }
        if(!m_ogreAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Bloodlust",
                Description = "Incites a frenzy in the Magi, decreasing his click duration by 30 seconds. Cooldown: 3.5 minutes",
                HeroUpgrade = "Ogre Magi",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/OgreMagi_Bloodlust"),
                Cost = 0,
            });
        }
        if(!m_tuskAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Snowball",
                Description = "Tusk snowballs his click amount by 2. Cooldown: 4 minutes minutes",
                HeroUpgrade = "Tusk",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Tusk_Snowball"),
                Cost = 0,
            });
        }
        if(!m_tuskAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Walrus Punch",
                Description = "Tusk connects with his mighty fist and gives you a bonus click. Cooldown: 7 minutes",
                HeroUpgrade = "Tusk",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Tusk_WalrusPunch"),
                Cost = 0,
            });
        }
        if(!m_phoenixAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Sunray",
                Description = "A beam of light powerful enough to decrease all cooldowns by a minute. Cooldown: 7 minutes",
                HeroUpgrade = "Phoenix",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Phoenix_SunRay"),
                Cost = 0,
            });
        }
        if(!m_phoenixAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Supernova",
                Description = "Completes a click every second of Supernova's duration. Cooldown: 10 minutes",
                HeroUpgrade = "Phoenix",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Phoenix_Supernova"),
                Cost = 0,
            });
        }
        if(!m_svenAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "War Cry",
                Description = "Decreases each clickers duration by 1/4. Cooldown: 7 minutes",
                HeroUpgrade = "Sven",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Sven_WarCry"),
                Cost = 0,
            });
        }
        if(!m_svenAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "God's Strength",
                Description = "Sven channels his rogue strength, increasing his teammates click amount by 2 for 30 seconds. Cooldown: 15 minutes",
                HeroUpgrade = "Sven",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Sven_GodsStrength"),
                Cost = 0,
            });
        }
        if(!m_antiAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Blink",
                Description = "In a blink, Anti Mage gives you a click for free. Cooldown: 2 minutes",
                HeroUpgrade = "Anti Mage",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/AntiMage_Blink"),
                Cost = 0,
            });
        }
        if(!m_antiAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Mana Void",
                Description = "For each second missing from AM's current click duration, the surrounding heroes get that duration taken off their current time. Cooldown: 10 minutes",
                HeroUpgrade = "Anti Mage",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/AntiMage_ManaVoid"),
                Cost = 0,
            });
        }
        if(!m_alcAbil1)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Greevil's Greed",
                Description = "For every attack, the Alchemist reduces his click duration by 20 seconds, lasts 1 minute. Cooldown: 20 minutes",
                HeroUpgrade = "Alchemist",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Alchemist_GreevilsGreed"),
                Cost = 0,
            });
        }
        if(!m_alcAbil2)
        {
            Upgrades.Add(new UpgradeDto()
            {
                Name = "Chemical Rage",
                Description = "The Alchemist causes his Ogre to enter a chemically induced rage reducing his current click by 3/4. Cooldown: 30 minutes",
                HeroUpgrade = "Alchemist",
                Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Alchemist_ChemicalRage"),
                Cost = 0,
            });
        }
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
        }
    }

    void AddUpgrade(UpgradeDto upgrade)
    {
        bool gotBothAbilityAchievement = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().CurrentSaveFile.Achievements.BuyAllAbilitiesForAHero;

        if (m_sceneController.TotalGold < upgrade.Cost || isOnMainMenu)
        {
            Debug.Log("Can't buy upgrade '" + upgrade.Name + "'");
            this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/magic_immune"));
            return;
        }

        if (upgrade.Name == "Crystal Nova")
        {
            Debug.Log("Clicked Crystal Nova");
            BuyCrystalNovaUpgrade(1); //Invoke Event
            m_cmAbil1 = true;

            bool canFindAbility = Upgrades.Exists(x => x.Name == "Crystal Nova");
            if (!canFindAbility && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if(upgrade.Name == "Frostbite")
        {
            Debug.Log("Clicked Frostbite");
            BuyFrostbiteUpgrade(1);
            m_cmAbil2 = true;

            if (!Upgrades.Exists(x => x.Name == "Frostbite") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Telekinesis")
        {
            Debug.Log("Clicked Telekinesis");
            BuyTelekinesisUpgrade(1);
            m_rubickAbil1 = true;

            if (!Upgrades.Exists(x => x.Name == "Spell Steal") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Spell Steal")
        {
            Debug.Log("Clicked Spell Steal");
            BuySpellStealUpgrade(1);
            m_rubickAbil2 = true;

            if (!Upgrades.Exists(x => x.Name == "Telekinesis") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Fireblast")
        {
            Debug.Log("Clicked Fireblast");
            BuyFireblastUpgrade(1);
            m_ogreAbil1 = true;

            if (!Upgrades.Exists(x => x.Name == "Bloodlust") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Bloodlust")
        {
            Debug.Log("Clicked Bloodlust");
            BuyBloodlustUpgrade(1);
            m_ogreAbil2 = true;

            if (!Upgrades.Exists(x => x.Name == "Fireblast") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Snowball")
        {
            Debug.Log("Clicked Snowball");
            BuySnowballUpgrade(1);
            m_tuskAbil1 = true;

            if (!Upgrades.Exists(x => x.Name == "Walrus Punch") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Walrus Punch")
        {
            Debug.Log("Clicked Walrus Punch");
            BuyWalrusPunchUpgrade(1);
            m_tuskAbil2 = true;

            if (!Upgrades.Exists(x => x.Name == "Snowball") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Sunray")
        {
            Debug.Log("Clicked Sunray");
            BuySunrayUpgrade(1);
            m_phoenixAbil1 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "Supernova") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Supernova")
        {
            Debug.Log("Clicked Supernova");
            BuySupernovaUpgrade(1);
            m_phoenixAbil2 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "Sunray") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "War Cry")
        {
            Debug.Log("Clicked War Cry");
            BuyWarCryUpgrade(1);
            m_svenAbil1 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "God's Strength") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "God's Strength")
        {
            Debug.Log("Clicked God's Strength");
            BuyGodsStrengthUpgrade(1);
            m_svenAbil2 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "War Cry") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Blink")
        {
            Debug.Log("Clicked Blink");
            BuyBlinkUpgrade(1);
            m_antiAbil1 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "Mana Void") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Mana Void")
        {
            Debug.Log("Clicked Mana Void");
            BuyManaVoidUpgrade(1);
            m_antiAbil2 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "Blink") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Greevil's Greed")
        {
            Debug.Log("Clicked Greevil's Greed");
            BuyGreevilsGreedUpgrade(1);
            m_alcAbil1 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "Chemical Rage") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }
        else if (upgrade.Name == "Chemical Rage")
        {
            Debug.Log("Clicked Chemical Rage");
            BuyChemicalRageUpgrade(1);
            m_alcAbil2 = true;

            GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().EnableRoshanEvents();

            if (!Upgrades.Exists(x => x.Name == "Greevil's Greed") && !gotBothAbilityAchievement)
            {
                AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
                events.BuyAllAbilitiesForAHero.Invoke();
                Debug.Log("Bought abilities for hero Achievements");
            }
        }

        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/buy"));
        Upgrades.RemoveAll(x => x.Name == upgrade.Name);
        RefreshUpgradesList();

        if(Upgrades.Count == (Upgrades.Count - 1)) //One less than count
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.BuyAnAbility.Invoke();
            Debug.Log("Bought an Ability Achievements");
        }
        else if(Upgrades.Count <= 0)
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.BuyAllAbilities.Invoke();
            Debug.Log("Bought All Abilities Achievements");
        }
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        foreach(HeroDto hero in saveFile.RadiantSide.Heroes)
        {
            if(hero.Ability1Level > 0)
            {
                if (hero.HeroName == "Crystal Maiden")
                {
                    m_cmAbil1 = true;
                    BuyCrystalNovaUpgrade(hero.Ability1Level);
                }
                else if (hero.HeroName == "Rubick")
                {
                    m_rubickAbil1 = true;
                    BuyTelekinesisUpgrade(hero.Ability1Level);
                }
                else if (hero.HeroName == "Ogre Magi")
                {
                    m_ogreAbil1 = true;
                    BuyFireblastUpgrade(hero.Ability1Level);
                }
                else if (hero.HeroName == "Tusk")
                {
                    m_tuskAbil1 = true;
                    BuySnowballUpgrade(hero.Ability1Level);  
                }
                else if (hero.HeroName == "Phoenix")
                {
                    m_phoenixAbil1 = true;
                    BuySunrayUpgrade(hero.Ability1Level);
                }
                else if (hero.HeroName == "Sven")
                {
                    m_svenAbil1 = true;
                    BuyWarCryUpgrade(hero.Ability1Level);
                }
                else if (hero.HeroName == "Anti Mage")
                {
                    m_antiAbil1 = true;
                    BuyBlinkUpgrade(hero.Ability1Level);
                }
                else if (hero.HeroName == "Alchemist")
                {
                    m_alcAbil1 = true;
                    BuyGreevilsGreedUpgrade(hero.Ability1Level); 
                }
            }

            if (hero.Ability2Level > 0)
            {
                if (hero.HeroName == "Crystal Maiden")
                {
                    m_cmAbil2 = true;
                    BuyFrostbiteUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Rubick")
                {
                    m_rubickAbil2 = true;
                    BuySpellStealUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Ogre Magi")
                {
                    m_ogreAbil2 = true;
                    BuyBloodlustUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Tusk")
                {
                    m_tuskAbil2 = true;
                    BuyWalrusPunchUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Phoenix")
                {
                    m_phoenixAbil2 = true;
                    BuySupernovaUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Sven")
                {
                    m_svenAbil2 = true;
                    BuyGodsStrengthUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Anti Mage")
                {
                    m_antiAbil2 = true;
                    BuyManaVoidUpgrade(hero.Ability2Level);
                }
                else if (hero.HeroName == "Alchemist")
                {
                    m_alcAbil2 = true;
                    BuyChemicalRageUpgrade(hero.Ability2Level);
                }
            }
        }
    }
}
