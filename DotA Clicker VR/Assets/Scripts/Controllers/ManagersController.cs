using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ManagersController : MonoBehaviour {

    public List<ManagerDto> Managers { get; set; }

    public delegate void OnBuyCMManager();
    public delegate void OnBuyRubickManager();
    public delegate void OnBuyOgreMagiManager();
    public delegate void OnBuyTuskManager();
    public delegate void OnBuyPhoenixManager();
    public delegate void OnBuySvenManager();
    public delegate void OnBuyAntiMageManager();
    public delegate void OnBuyAlchemistManager();

    public static event OnBuyCMManager BuyCMManager;
    public static event OnBuyRubickManager BuyRubickManager;
    public static event OnBuyOgreMagiManager BuyOgreMagiManager;
    public static event OnBuyTuskManager BuyTuskManager;
    public static event OnBuyPhoenixManager BuyPhoenixManager;
    public static event OnBuySvenManager BuySvenManager;
    public static event OnBuyAntiMageManager BuyAntiMageManager;
    public static event OnBuyAlchemistManager BuyAlchemistManager;

    [SerializeField]
    GameObject ManagerPrefab;

    bool isOnMainMenu = false;
    RadiantSceneController m_sceneController;

    //Has manager been bought
    bool m_CM, m_rubick, m_ogreMagi, m_tusk, m_phoenix, m_sven, m_antiMage, m_alchemist;

    [SerializeField]
    GameObject m_allBoughtPrefab;

    void Awake()
    {
        Managers = new List<ManagerDto>();
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

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
        AddManagers();

        RefreshManagersList();
    }

    private void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        //if (Managers.Count <= 0) return;

        foreach(HeroDto hero in saveFile.RadiantSide.Heroes)
        {
            if(hero.HasManager == true)
            {
                if (hero.HeroName == "Crystal Maiden")
                {
                    m_CM = true;

                    if(BuyCMManager != null)
                        BuyCMManager.Invoke();
                }
                else if(hero.HeroName == "Rubick")
                {
                    m_rubick = true;
                }
                else if(hero.HeroName == "Ogre Magi")
                {
                    m_ogreMagi = true;
                }
                else if(hero.HeroName == "Tusk")
                {
                    m_tusk = true;
                }
                else if(hero.HeroName == "Phoenix")
                {
                    m_phoenix = true;
                }
                else if(hero.HeroName == "Sven")
                {
                    m_phoenix = true;
                }
                else if(hero.HeroName == "Anti Mage")
                {
                    m_antiMage = true;
                }
                else if(hero.HeroName == "Alchemist")
                {
                    m_alchemist = true;
                }
            }
        }

        RefreshManagersList();
    }

    void Update ()
    {
	
	}

    void AddManagers()
    {
        if (Managers.Count > 0) return;

        if(!m_CM)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Crystal Maiden",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/CM"),
                Cost = 5000,
            });
        }
        if(!m_rubick)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Rubick",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Rubick"),
                Cost = 9000,
            });
        }
        if(!m_ogreMagi)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Ogre Magi",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/OgreMagi"),
                Cost = 14000,
            });
        }
        if(!m_tusk)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Tusk",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Tusk"),
                Cost = 20000,
            });
        }
        if(!m_phoenix)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Phoenix",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Phoenix"),
                Cost = 35000,
            });
        }
        if(!m_sven)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Sven",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Sven"),
                Cost = 50000,
            });
        }
        if(!m_antiMage)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Anti Mage",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/AntiMage"),
                Cost = 100000,
            });
        }
        if(!m_alchemist)
        {
            Managers.Add(new ManagerDto()
            {
                Name = "Alchemist",
                Image = Resources.Load<Sprite>("Images/UI/ManagerIcons/Alchemist"),
                Cost = 500000,
            });
        }
    }

    void RefreshManagersList()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int addY = -30; //Hack to get positioning of List UI
        if (Managers.Count == 0)
            AddManagers();

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
            Text upgradeCost = newManager.transform.Find("BuyManagerButton/GoldCost").GetComponent<Text>();
            upgradeCost.text = manager.Cost.ToString();
            Button button = newManager.transform.Find("BuyManagerButton").GetComponent<Button>();
            ManagerDto clickedManager = manager; //Fix for AddListener adding current manager to each button click
            button.onClick.AddListener(delegate { AddManager(clickedManager); });
        }

        if (Managers.Count < 1)
        {
            var canvas = Instantiate(m_allBoughtPrefab);
            canvas.transform.SetParent(this.transform, false);
        }
    }

    void AddManager(ManagerDto manager)
    {
        if (m_sceneController.TotalGold < manager.Cost || isOnMainMenu)
        {
            Debug.Log("Can't buy manager '" + manager.Name + "'");
            this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/magic_immune"));
            return;
        }

        if (manager.Name == "Io")
        {
            Debug.Log("Clicked Io Manager");
            BuyCMManager(); //Invoke Event
            m_CM = true;
        }
        else if (manager.Name == "Rubick")
        {
            Debug.Log("Clicked Rubick Manager");
            BuyRubickManager();
            m_rubick = true;
        }
        else if (manager.Name == "Ogre Magi")
        {
            Debug.Log("Clicked Ogre Magi Manager");
            BuyOgreMagiManager();
            m_ogreMagi = true;
        }
        else if (manager.Name == "Tusk")
        {
            Debug.Log("Clicked Tusk Manager");
            BuyTuskManager();
            m_tusk = true;
        }
        else if (manager.Name == "Phoenix")
        {
            Debug.Log("Clicked Phoenix Manager");
            BuyPhoenixManager();
            m_phoenix = true;
        }
        else if (manager.Name == "Sven")
        {
            Debug.Log("Clicked Sven Manager");
            BuySvenManager();
            m_sven = true;
        }
        else if (manager.Name == "Anti Mage")
        {
            Debug.Log("Clicked Anti Mage Manager");
            BuyAntiMageManager();
            m_antiMage = true;
        }
        else if (manager.Name == "Alchemist")
        {
            Debug.Log("Clicked Alchemist Manager");
            BuyAlchemistManager();
            m_alchemist = true;
        }

        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/buy"));
        Managers.RemoveAll(x => x.Name == manager.Name);
        RefreshManagersList();

        if(Managers.Count <= 0)
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.BuyAllManagers.Invoke();
            Debug.Log("Bought all Managers Achievements");
        }
        else if(Managers.Count == (Managers.Count - 1)) //One less than max
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.BuyAManager.Invoke();
            Debug.Log("Bought a Manager Achievements");
        }
    }
}
