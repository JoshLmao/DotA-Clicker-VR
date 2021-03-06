using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BuyableItemsController : MonoBehaviour
{
    public List<ItemDto> Items { get; set; }
    public int ItemBoughtCount = 0;

    public int IronBranchCount, ClarityCount, MagicStickCount, QuellingBladeCount, MangoCount, PowerTreadsCount, 
        BottleCount, BlinkDaggerCount, HyperstoneCount, BloodstoneCount, ReaverCount, DivineRapierCount, RecipeCount;

    [SerializeField]
    GameObject UIItemPrefab; //For UI prefab

    [SerializeField]
    GameObject[] ItemsPrefabs; //For the actual item models

    bool isOnMainMenu = false;
    RadiantSceneController m_sceneController;

    [SerializeField]
    GameObject m_allBoughtPrefab;

    AchievementEvents m_achievementEvents;

    void Awake()
    {
        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;

        m_achievementEvents = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            isOnMainMenu = true;
        }
        else
        {
            m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        }

        Items = new List<ItemDto>();

        AddDefaultItems();
        RefreshItemsList();
    }

    void Update()
    {

    }

    void AddDefaultItems()
    {
        Items.Add(new ItemDto()
        {
            Name = "Iron Branch",
            Description = "A simple iron branch",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            Cost = 50,
            ItemPrefab = ItemsPrefabs[0], //By order of added in inspector
        });
        Items.Add(new ItemDto()
        {
            Name = "Clarity",
            Description = "Clear water that enhances a heroe's ability to meditate",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Clarity_icon"),
            Cost = 300,
            ItemPrefab = ItemsPrefabs[1],
        });
        Items.Add(new ItemDto()
        {
            Name = "Magic Stick",
            Description = "A simple wand used to channel magic energies",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Magic_Stick_icon"),
            Cost = 600,
            ItemPrefab = ItemsPrefabs[2],
        });
        Items.Add(new ItemDto()
        {
            Name = "Quelling Blade",
            Description = "The axe of the fallen gnome",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Quelling_Blade_icon"),
            Cost = 900,
            ItemPrefab = ItemsPrefabs[3],
        });
        Items.Add(new ItemDto()
        {
            Name = "Mango",
            Description = "The bittersweet flavors of Jidi Isle are irresistible to amphibians",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Enchanted_Mango_icon"),
            Cost = 1000,
            ItemPrefab = ItemsPrefabs[4],
        });
        Items.Add(new ItemDto()
        {
            Name = "Power Treads",
            Description = "A apir of tough-skinned boots that change to meet the demands of the wearer",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Power_Treads_icon"),
            Cost = 1500,
            ItemPrefab = ItemsPrefabs[5],
        });
        Items.Add(new ItemDto()
        {
            Name = "Bottle",
            Description = "An old bottle that survived the ages, the contents placed inside become enchanted",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Bottle_icon"),
            Cost = 2500,
            ItemPrefab = ItemsPrefabs[6],
        });
        Items.Add(new ItemDto()
        {
            Name = "Blink Dagger",
            Description = "The fabled dagger used by the fastest assassin ever to walk the lands",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Blink_Dagger_icon"),
            Cost = 2750,
            ItemPrefab = ItemsPrefabs[7],
        });
        Items.Add(new ItemDto()
        {
            Name = "Hyperstone",
            Description = "A mystical, carved stone that boosts the fervor of the holder",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Hyperstone_icon"),
            Cost = 3250,
            ItemPrefab = ItemsPrefabs[8],
        });
        Items.Add(new ItemDto()
        {
            Name = "Bloodstone",
            Description = "The Bloodstone's bright ruby color is unmistakable on the battlefield, as the owner seems to habe infinte vitality and spirit",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Bloodstone_icon"),
            Cost = 3750,
            ItemPrefab = ItemsPrefabs[9],
        });
        Items.Add(new ItemDto()
        {
            Name = "Reaver",
            Description = "A massive axe capable of tearing whole mountains down",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Reaver_icon"),
            Cost = 4500,
            ItemPrefab = ItemsPrefabs[10],
        });
        Items.Add(new ItemDto()
        {
            Name = "Divine Rapier",
            Description = "So powerful, it cannot have a single owner.",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Divine_Rapier_icon"),
            Cost = 5000,
            ItemPrefab = ItemsPrefabs[11],
        });
        Items.Add(new ItemDto()
        {
            Name = "Recipe",
            Description = "???",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Recipe_Scroll_icon"),
            Cost = 6000,
            ItemPrefab = ItemsPrefabs[12],
        });
    }

    void RefreshItemsList()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        int addY = -30; //Hack to get positioning of List UI
        foreach (ItemDto item in Items)
        {
            GameObject newUpgrade = GameObject.Instantiate(UIItemPrefab);
            newUpgrade.transform.SetParent(this.transform, false);
            newUpgrade.name = item.Name;

            newUpgrade.GetComponent<RectTransform>().localPosition = new Vector3(newUpgrade.transform.localPosition.x, addY, newUpgrade.transform.localPosition.z);
            addY -= 275; //Space between each UI

            Image icon = newUpgrade.transform.Find("ItemImage").GetComponent<Image>();
            icon.sprite = item.Image;
            Text upgradeName = newUpgrade.transform.Find("ItemName").GetComponent<Text>();
            upgradeName.text = item.Name;
            Text description = newUpgrade.transform.Find("ItemDesc").GetComponent<Text>();
            description.text = item.Description;
            Text upgradeCost = newUpgrade.transform.Find("BuyItemButton/CostCanvas/GoldCost").GetComponent<Text>();
            upgradeCost.text = item.Cost + " gold";
            Button button = newUpgrade.transform.Find("BuyItemButton").GetComponent<Button>();
            ItemDto clickedItem = item; //Fix for AddListener adding current upgrade to each button click
            button.onClick.AddListener(delegate { BuyItem(clickedItem); });
        }

        if (Items.Count < 1)
        {
            var canvas = Instantiate(m_allBoughtPrefab);
            canvas.transform.SetParent(this.transform, false);
        }
    }

    void BuyItem(ItemDto item)
    {
        if (m_sceneController.TotalGold < item.Cost || isOnMainMenu)
        {
            Debug.Log("Can't buy item '" + item.Name + "'");
            this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/magic_immune"));
            return;
        }

        //Remove cost & spawn item
        m_sceneController.RemoveFromTotal(item.Cost);
        SpawnItem(item);

        if(item.Name == "Iron Branch")
        {
            IronBranchCount++;
        }
        else if(item.Name == "Clarity")
        {
            ClarityCount++;
        }
        else if (item.Name == "Magic Stick")
        {
            MagicStickCount++;
        }
        else if (item.Name == "Quelling Blade")
        {
            QuellingBladeCount++;
        }
        else if (item.Name == "Mango")
        {
            MangoCount++;
        }
        else if (item.Name == "Power Treads")
        {
            PowerTreadsCount++;
        }
        else if (item.Name == "Bottle")
        {
            BottleCount++;
        }
        else if (item.Name == "Blink Dagger")
        {
            BlinkDaggerCount++;
        }
        else if (item.Name == "Hyperstone")
        {
            HyperstoneCount++;
        }
        else if (item.Name == "Bloodstone")
        {
            BloodstoneCount++;
        }
        else if (item.Name == "Reaver")
        {
            ReaverCount++;
        }
        else if (item.Name == "Divine Rapier")
        {
            DivineRapierCount++;
        }
        else if (item.Name == "Recipe")
        {
            RecipeCount++;
        }

        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/buy"));
        ItemBoughtCount++;

        CheckItemAchievements();
    }

    private void CheckItemAchievements()
    {
        if (ItemBoughtCount == Items.Count && !m_achievementEvents.BuyAnItemStatus)
        {
            m_achievementEvents.BuyAnItem.Invoke();
            Debug.Log("Bought One Item Achievements");
        }

        //check if the achievement has been unlocked
        bool boughtEachItem = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>().CurrentSaveFile.Achievements.BuyEachItemOnce;
        if (IronBranchCount >= 1 && ClarityCount >= 1 && MagicStickCount >= 1 && QuellingBladeCount >= 1 && MangoCount >= 1 && PowerTreadsCount >= 1 && BottleCount >= 1
            && BlinkDaggerCount >= 1 && HyperstoneCount >= 1 && BloodstoneCount >= 1 && ReaverCount >= 1 && DivineRapierCount >= 1 && RecipeCount >= 1 && !boughtEachItem
            && !m_achievementEvents.BuyEachItenOnceStatus)
        {

            m_achievementEvents.BuyEachItemOnce.Invoke();
            Debug.Log("Bought one of each item Achievements");
        }
    }

    void SpawnItem(ItemDto item)
    {
        Transform spawnPoint = GameObject.Find("HeroItemSpawnPoint").transform;
        var obj = Instantiate(item.ItemPrefab);
        obj.transform.position = spawnPoint.position;
    }

    void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        ItemStatsDto stats = saveFile.SessionStats.ItemStats;
        IronBranchCount = stats.IronBranchCount;
        ClarityCount = stats.ClarityCount;
        MagicStickCount = stats.MagicStickCount;
        QuellingBladeCount = stats.QuellingBladeCount;
        MangoCount = stats.MangoCount;
        PowerTreadsCount = stats.PowerTreadsCount;
        BottleCount = stats.BottleCount;
        BlinkDaggerCount = stats.BlinkDaggerCount;
        HyperstoneCount = stats.HyperstoneCount;
        BloodstoneCount = stats.BloodstoneCount;
        ReaverCount = stats.ReaverCount;
        DivineRapierCount = stats.DivineRapierCount;
        RecipeCount = stats.RecipeCount;
    }
}
