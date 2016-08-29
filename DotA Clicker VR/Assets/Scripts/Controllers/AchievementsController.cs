using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AchievementsController : MonoBehaviour {

    public List<AchievementDto> Achievements { get; set; }

    [SerializeField]
    GameObject UIItemPrefab; //For UI prefab

    RadiantSceneController m_sceneController;

    void Start()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        Achievements = new List<AchievementDto>();
            //AddDefaultItems();
            //RefreshItemsList();

        //Todo: Listen to achievement events

        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

    void AddDefaultItems()
    {
        if (m_sceneController.CurrentSaveFile == null)
            Debug.Log("Current save file is null");

        Achievements.Add(new AchievementDto()
        {
            Name = "When Did EG Throw Last?",
            Description = "Throw the EG logo off the map",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.WhenDidEGThrowLast,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "The Aegis is mine!",
            Description = "Defeat Roshan once",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.TheAegisIsMine,
        });
        Achievements.Add(new AchievementDto()
        {
            Name = "Click once",
            Description = "Wow. Such a hard achievement to get",
            Image = Resources.Load<Sprite>("Images/UI/ItemsIcons/Iron_Branch_icon"),
            IsUnlocked = m_sceneController.CurrentSaveFile.Achievements.ClickOnce,
        });
    }

    void RefreshItemsList()
    { 
        var children = new List<GameObject>();
        foreach (Transform child in transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        Achievements.Sort((x, y) => string.Compare(x.Name, y.Name)); //Sort list into alphabetical Order

        int addY = -30; //Hack to get positioning of List UI
        foreach (AchievementDto achievement in Achievements)
        {
            GameObject newUpgrade = GameObject.Instantiate(UIItemPrefab);
            newUpgrade.transform.SetParent(this.transform, false);
            newUpgrade.name = achievement.Name;

            newUpgrade.GetComponent<RectTransform>().localPosition = new Vector3(newUpgrade.transform.localPosition.x, addY, newUpgrade.transform.localPosition.z);
            addY -= 150; //Space between each UI

            Image icon = newUpgrade.transform.Find("Image").GetComponent<Image>();
            icon.sprite = achievement.Image;
            Image locked = newUpgrade.transform.Find("ImageLocked").GetComponent<Image>();
            locked.fillAmount = achievement.IsUnlocked == true ? 0 : 1;

            Text upgradeName = newUpgrade.transform.Find("Name").GetComponent<Text>();
            upgradeName.text = achievement.Name;
            upgradeName.color = achievement.IsUnlocked == true ? Color.white : Color.gray;

            Text description = newUpgrade.transform.Find("Desc").GetComponent<Text>();
            description.text = achievement.Description;
            description.color = achievement.IsUnlocked == true ? Color.white : Color.gray;
        }
    }


    void OnLoadedSaveFile()
    {
        AddDefaultItems();
        RefreshItemsList();
    }
}
