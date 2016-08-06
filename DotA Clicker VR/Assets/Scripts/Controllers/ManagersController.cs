using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ManagersController : MonoBehaviour {

    public List<ManagerDto> Managers { get; set; }

    [SerializeField]
    GameObject ManagerPrefab;

    void Start ()
    {
        Managers = new List<ManagerDto>();
        RefreshManagers();

        int addY = 585; //Hack to get positioning of List UI

        foreach (ManagerDto manager in Managers)
        {
            GameObject newManager = GameObject.Instantiate(ManagerPrefab);
            newManager.transform.SetParent(this.transform, false);
            newManager.name = manager.Name;

            newManager.GetComponent<RectTransform>().localPosition = new Vector3(newManager.transform.localPosition.x, addY, newManager.transform.localPosition.z);
            addY -= 230;

            Image icon = newManager.transform.Find("ManagerImage").GetComponent<Image>();
            icon.sprite = manager.Image;
            Text upgradeName = newManager.transform.Find("ManagerName").GetComponent<Text>();
            upgradeName.text = manager.Name + " Manager";
            Text upgradeCost = newManager.transform.Find("BuyButton/CostCanvas/GoldCost").GetComponent<Text>();
            upgradeCost.text = manager.Cost + " gold";
        }
    }
	
	void Update ()
    {
	
	}

    void RefreshManagers()
    {
        Managers.Add(new ManagerDto()
        {
            Name = "Io",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Io"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Rubick",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Rubick"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Ogre Magi",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/OgreMagi"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Tusk",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Tusk"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Phoenix",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Phoenix"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Sven",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Sven"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Anti Mage",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/AntiMage"),
            Cost = 0,
        });
        Managers.Add(new ManagerDto()
        {
            Name = "Alchemist",
            Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Alchemist"),
            Cost = 0,
        });
    }
}
