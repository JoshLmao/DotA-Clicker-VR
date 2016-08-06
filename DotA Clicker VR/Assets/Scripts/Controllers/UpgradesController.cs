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

        foreach(UpgradeDto upgrade in Upgrades)
        {
            GameObject newUpgrade = GameObject.Instantiate(UpgradePrefab);
            newUpgrade.transform.SetParent(this.transform, false);
            newUpgrade.name = upgrade.Name;

            //newUpgrade.GetComponent<RectTransform>().position = new Vector3(newUpgrade.transform.position.x, newUpgrade.transform.position.y + 180, newUpgrade.transform.position.z);

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
        //Wisp upgrade 1
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Overcharge",
            Description = "Overcharges Io to double his output for 30 seconds",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/Io/Io_Overcharge"),
            Cost = 2000,
        });
        //Wisp Upgrade 2
        Upgrades.Add(new UpgradeDto()
        {
            Name = "Relocate",
            Description = "",
            HeroUpgrade = "Io",
            Image = Resources.Load<Sprite>("Images/UI/UpgradeIcons/IO/Io_Relocate"),
            Cost = 6000,
        });
    }
}
