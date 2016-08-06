using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradesController : MonoBehaviour
{
    public List<UpgradeDto> Upgrades { get; set; }

    [SerializeField]
    GameObject UpgradePrefab;

	void Start ()
    {
        Upgrades = new List<UpgradeDto>();

        RefreshUpgrades();

        int addY = -30; //Hack to get positioning of List UI

        foreach(UpgradeDto upgrade in Upgrades)
        {
            GameObject newUpgrade = GameObject.Instantiate(UpgradePrefab);
            newUpgrade.transform.SetParent(this.transform, false);
            newUpgrade.name = upgrade.Name;

            newUpgrade.GetComponent<RectTransform>().localPosition = new Vector3(newUpgrade.transform.localPosition.x, addY, newUpgrade.transform.localPosition.z);
            addY -= 180;

            Image icon = newUpgrade.transform.Find("UpgradeImage").GetComponent<Image>();
            icon.sprite = upgrade.Image;
            Text upgradeName = newUpgrade.transform.Find("UpgradeName").GetComponent<Text>();
            upgradeName.text = upgrade.Name + " - " + upgrade.HeroUpgrade;
            Text description = newUpgrade.transform.Find("UpgradeDesc").GetComponent<Text>();
            description.text = upgrade.Description;
            Text upgradeCost = newUpgrade.transform.Find("BuyButton/CostCanvas/GoldCost").GetComponent<Text>();
            upgradeCost.text = upgrade.Cost + " gold";
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
            Description = "Overcharges Io to double his output for 30 seconds",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Io_Overcharge"),
            Cost = 2000,
        });
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Relocate",
            Description = "",
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

    }
}
