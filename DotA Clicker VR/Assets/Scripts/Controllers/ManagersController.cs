using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ManagersController : MonoBehaviour {

    public List<ManagerDto> Managers { get; set; }

    public delegate void OnBuyIoManager();
    public delegate void OnBuyRubickManager();
    public delegate void OnBuyOgreMagiManager();
    public delegate void OnBuyTuskManager();
    public delegate void OnBuyPhoenixManager();
    public delegate void OnBuySvenManager();
    public delegate void OnBuyAntiMageManager();
    public delegate void OnBuyAlchemistManager();

    public static event OnBuyIoManager BuyIoManager;
    public static event OnBuyRubickManager BuyRubickManager;
    public static event OnBuyOgreMagiManager BuyOgreMagiManager;
    public static event OnBuyTuskManager BuyTuskManager;
    public static event OnBuyPhoenixManager BuyPhoenixManager;
    public static event OnBuySvenManager BuySvenManager;
    public static event OnBuyAntiMageManager BuyAntiMageManager;
    public static event OnBuyAlchemistManager BuyAlchemistManager;

    [SerializeField]
    GameObject ManagerPrefab;

    RadiantSceneController m_sceneController;

    void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        Managers = new List<ManagerDto>();
        AddManagers();

        RefreshManagersList();
    }
	
	void Update ()
    {
	
	}

    void AddManagers()
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

    void RefreshManagersList()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int addY = -30; //Hack to get positioning of List UI
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
            Button button = newManager.transform.Find("BuyButton").GetComponent<Button>();
            ManagerDto clickedManager = manager; //Fix for AddListener adding current manager to each button click
            button.onClick.AddListener(delegate { AddManager(clickedManager); });
        }
    }

    void AddManager(ManagerDto manager)
    {
        if (m_sceneController.TotalGold < manager.Cost)
        {
            Debug.Log("Can't buy manager '" + manager.Name + "'");
            return;
        }

        if (manager.Name == "Io")
        {
            Debug.Log("Clicked Io Manager");
            BuyIoManager(); //Invoke Event
        }
        else if (manager.Name == "Rubick")
        {
            Debug.Log("Clicked Rubick Manager");
            BuyRubickManager(); 
        }
        else if (manager.Name == "Ogre Magi")
        {
            Debug.Log("Clicked Ogre Magi Manager");
            BuyOgreMagiManager(); 
        }
        else if (manager.Name == "Tusk")
        {
            Debug.Log("Clicked Tusk Manager");
            BuyTuskManager(); 
        }
        else if (manager.Name == "Phoenix")
        {
            Debug.Log("Clicked Phoenix Manager");
            BuyPhoenixManager(); 
        }
        else if (manager.Name == "Sven")
        {
            Debug.Log("Clicked Sven Manager");
            BuySvenManager(); 
        }
        else if (manager.Name == "Anti Mage")
        {
            Debug.Log("Clicked Anti Mage Manager");
            BuyAntiMageManager(); 
        }
        else if (manager.Name == "Alchemist")
        {
            Debug.Log("Clicked Alchemist Manager");
            BuyAlchemistManager(); 
        }

        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/buy"));
        Managers.RemoveAll(x => x.Name == manager.Name);
        RefreshManagersList();
    }
}
