using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BuyableItemsController : MonoBehaviour
{

    public List<ItemDto> Items { get; set; }

    [SerializeField]
    GameObject UIItemPrefab; //For UI prefab

    [SerializeField]
    GameObject[] ItemsPrefabs; //For the actual item models

    bool isOnMainMenu = false;
    RadiantSceneController m_sceneController;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
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
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[0], //By order of added in inspector
        });
        Items.Add(new ItemDto()
        {
            Name = "Clarity",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Clarity_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[1],
        });
        Items.Add(new ItemDto()
        {
            Name = "Magic Stick",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Magic_Stick_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[2],
        });
        Items.Add(new ItemDto()
        {
            Name = "Quelling Blade",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Quelling_Blade_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[3],
        });
        Items.Add(new ItemDto()
        {
            Name = "Mango",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Enchanted_Mango_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[4],
        });
        Items.Add(new ItemDto()
        {
            Name = "Power Treads",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Power_Treads_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[5],
        });
        Items.Add(new ItemDto()
        {
            Name = "Bottle",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Bottle_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[6],
        });
        Items.Add(new ItemDto()
        {
            Name = "Blink Dagger",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Blink_Dagger_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[7],
        });
        Items.Add(new ItemDto()
        {
            Name = "Hyperstone",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Hyperstone_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[8],
        });
        Items.Add(new ItemDto()
        {
            Name = "Bloodstone",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Bloodstone_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[9],
        });
        Items.Add(new ItemDto()
        {
            Name = "Reaver",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Reaver_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[10],
        });
        Items.Add(new ItemDto()
        {
            Name = "Divine Rapier",
            Description = "",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Divine_Rapier_icon"),
            Cost = 0,
            ItemPrefab = ItemsPrefabs[11],
        });
        Items.Add(new ItemDto()
        {
            Name = "Recipe",
            Description = "???",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Recipe_Scroll_icon"),
            Cost = 10,
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
            Text upgradeCost = newUpgrade.transform.Find("BuyButton/CostCanvas/GoldCost").GetComponent<Text>();
            upgradeCost.text = item.Cost + " gold";
            Button button = newUpgrade.transform.Find("BuyButton").GetComponent<Button>();
            ItemDto clickedItem = item; //Fix for AddListener adding current upgrade to each button click
            button.onClick.AddListener(delegate { AddUpgrade(clickedItem); });
        }
    }

    void AddUpgrade(ItemDto item)
    {
        if (m_sceneController.TotalGold < item.Cost || isOnMainMenu)
        {
            Debug.Log("Can't buy item '" + item.Name + "'");
            this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/magic_immune"));
            return;
        }

        SpawnItem(item);

        this.GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/UI/buy"));
    }

    void SpawnItem(ItemDto item)
    {
        Transform spawnPoint = GameObject.Find("HeroItemSpawnPoint").transform;
        var obj = Instantiate(item.ItemPrefab);
        obj.transform.position = spawnPoint.position;
    }

}
