using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.VR;
using VRTK;
using UnityEngine.EventSystems;

public class RadiantSceneController : MonoBehaviour
{
    public delegate void OnLoadedSaveFile(SaveFileDto saveFile);
    public static event OnLoadedSaveFile LoadedSaveFile;
    public delegate void OnLoadedConfigFile(ConfigDto config);
    public static event OnLoadedConfigFile LoadedConfigFile;
    public delegate void ReturningToMainMenu();
    public static event ReturningToMainMenu OnReturningToMainMenu;

    public delegate void SceneStarted();
    public static event SceneStarted OnSceneStarted;

    public List<RadiantClickerController> SceneHeroes;
    public SaveFileDto CurrentSaveFile;
    public ConfigDto CurrentConfigFile;
    public decimal TotalGold = 0;
    public GameObject[] RoshanPrefab;
    public GameObject AegisPrefab;
    public GameObject CheesePrefab;

    public GameObject[] ItemModifierDisplayPrefabs;

    public GameObject[] VRPlayerItems;
    public GameObject[] NonVRPlayerItems;

    public const string THOUSAND_FORMAT = "{0}, {1}, {2}";
    public const string MILLION_FORMAT = "{0} million";
    public const string BILLION_FORMAT = "{0} billion";
    public const string TRILLION_FORMAT = "{0} trillion";
    public const string QUADRILLION_FORMAT = "{0} quadrillion";

    public readonly static string FILE_PATHS = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\DotAClickerVR\\";
    public readonly static string SAVE_FILE = "SaveFile.json";
    public readonly static string CONFIG_FILE = "ConfigFile.json";

    public float ClickCount;
    public string CurrentPlayerName = string.Empty;

    string SAVE_FILE_LOCATION { get { return FILE_PATHS + SAVE_FILE; } }
    string CONFIG_LOCATION { get { return FILE_PATHS + CONFIG_FILE; } }

    OptionsController m_options;
    float m_totalPlayTime = 0;
    
    //Roshan Controls
    public bool m_canDoRoshanEvent = false;
    /// <summary>
    /// Controller for the current or last roshan event
    /// </summary>
    RoshanController m_activeRoshan;
    public float RoshanEventCount = 0;
    int m_secondsToRoshanEvent = -1;
    bool m_roshanWaitingToSpawn = false;
    bool m_roshanEventInProgress = false;
    GameObject ActiveRoshan;
    AchievementEvents m_achievementEvents;
    BuyableItemsController modifierController = null;

    GlobalDataController m_globalData;

    [SerializeField]
    GameObject m_vrInputModule;

    [SerializeField]
    GameObject m_inputModule;

    void Awake()
    {
        VRSettings.enabled = SteamVR.enabled ? SteamVR.active : false;
        SetPlayerMode(VRSettings.enabled);

        RoshanController.RoshanEventEnded += OnRoshanEventEnded;
        RoshanController.RoshanEventEndedNotKilled += OnRoshanEventEndedNotKilled;

        LoadedSaveFile += OnLoadedSave;

        modifierController = GameObject.Find("ItemsListCanvas").GetComponent<BuyableItemsController>();

        m_options = GameObject.Find("OptionsCanvas").GetComponent<OptionsController>();
        m_achievementEvents = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();

        if(OnSceneStarted != null)
            OnSceneStarted.Invoke();

        try
        {
            //Mioght not exists when in editor
            m_globalData = GameObject.Find("GlobalDataController").GetComponent<GlobalDataController>();
        }
        catch(Exception e)
        {

        }
    }

    void Start ()
    {
        SceneHeroes = GetClickerHeroesInScene();

        //Load save & config after Awake
        LoadProgress();
        CurrentConfigFile = LoadConfig();

        if (m_globalData != null && CurrentPlayerName != null && CurrentPlayerName == string.Empty)
        {
            //Will be null in editor since globals are setup in MainMenu
            CurrentPlayerName = m_globalData.PlayerName;
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DoRoshanEvent();
        }

        if (m_canDoRoshanEvent && !m_roshanWaitingToSpawn)
            StartRoshanCountdown();
    }

    public void LoadProgress()
    {
        if (!File.Exists(SAVE_FILE_LOCATION))
            return;

        try
        {
            //Deserialize existing file
            string file = File.ReadAllText(SAVE_FILE_LOCATION);
            SaveFileDto loadedSaveFile = JsonConvert.DeserializeObject<SaveFileDto>(file);
            CurrentSaveFile = loadedSaveFile;
        }
        catch(Exception e)
        {
            Debug.LogError("Can't load save file - " + e.ToString());
            CurrentSaveFile = new SaveFileDto();
        }

        if (LoadedSaveFile != null)
            LoadedSaveFile.Invoke(CurrentSaveFile);

        //If it hasnt been set in menus
        CurrentPlayerName = CurrentSaveFile.PlayerName;

        if (CurrentPlayerName.ToLower() == "420bootywizard")
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.TheManTheMythTheLegend.Invoke();
            Debug.Log("420BootyWizard Achievements");
        }

        OnLoadedSave(CurrentSaveFile);
    }

    public List<RadiantClickerController> GetClickerHeroesInScene()
    {
        List<RadiantClickerController> listClickers = new List<RadiantClickerController>();
        RadiantClickerController[] clickers = FindObjectsOfType<RadiantClickerController>();
        foreach(RadiantClickerController clicker in clickers)
        {
            listClickers.Add(clicker);
        }
        return listClickers;
    }

    public void OnApplicationQuit()
    {
        SaveFile();
        SaveConfig();
    }

    double CorrectTimeRemaining(int heroIndex)
    {
        double timeRemaining = (TimeSpan.FromSeconds(SceneHeroes[heroIndex].SecondsToCompleteClick) - SceneHeroes[heroIndex].CurrentClickerTime).TotalSeconds;

        if (timeRemaining != SceneHeroes[heroIndex].SecondsToCompleteClick && timeRemaining > 0)
            return timeRemaining;
        else
            return 0;
    }

    public void SaveFile()
    {
        //Check if folders & file exists
        CheckSaveFileFolders();

        //Add current playtime to total play time
        m_totalPlayTime += Time.realtimeSinceStartup;
        bool hasSaveFile = false;

        if (CurrentSaveFile != null)
            hasSaveFile = true;

        //Save data
        SaveFileDto saveFile = new SaveFileDto()
        {
            PlayerName = CurrentPlayerName,
            RadiantSide = new RadiantSideDto()
            {
                TotalGold = TotalGold,
                RoshanEvents = m_canDoRoshanEvent,
            },
            SessionStats = new StatsDto()
            {
                TotalPlayTime = hasSaveFile && CurrentSaveFile.SessionStats != null ? CurrentSaveFile.SessionStats.TotalPlayTime += m_totalPlayTime : m_totalPlayTime,
                ClickCount = ClickCount,
                ItemStats = new ItemStatsDto()
                {
                    IronBranchCount = modifierController.IronBranchCount,
                    ClarityCount = modifierController.ClarityCount,
                    MagicStickCount = modifierController.MagicStickCount,
                    QuellingBladeCount = modifierController.QuellingBladeCount,
                    MangoCount = modifierController.MangoCount,
                    PowerTreadsCount = modifierController.PowerTreadsCount,
                    BottleCount = modifierController.BottleCount,
                    BlinkDaggerCount = modifierController.BlinkDaggerCount,
                    HyperstoneCount = modifierController.HyperstoneCount,
                    BloodstoneCount = modifierController.BloodstoneCount,
                    ReaverCount = modifierController.ReaverCount,
                    DivineRapierCount = modifierController.DivineRapierCount,
                    RecipeCount = modifierController.RecipeCount,
                }
            },
            Achievements = new AchievementsDto()
            {
                Earn625Gold = m_achievementEvents.Earn625GoldStatus,
                Earn6200Gold = m_achievementEvents.Earn6200GoldStatus,
                Earn15000Gold = m_achievementEvents.Earn15000GoldStatus,
                Earn100000Gold = m_achievementEvents.Earn100000GoldStatus,
                EarnMillionGold = m_achievementEvents.EarnMillionGoldStatus,

                ClickOnce = m_achievementEvents.ClickOnceStatus,
                ClickFiveHundred = m_achievementEvents.ClickFiveHundredStatus,
                ClickThousand = m_achievementEvents.ClickThousandStatus,
                ClickFifteenThousand = m_achievementEvents.ClickFifteenThousandStatus,
                ClickFiftyThousand = m_achievementEvents.ClickFiftyThousandStatus,

                BuyAnAbility = m_achievementEvents.BuyAnAbilityStatus,
                BuyAllAbilitiesForAHero = m_achievementEvents.BuyAllAbilitiesForAHeroStatus,
                BuyAllAbilities = m_achievementEvents.BuyAllAbilitiesStatus,

                BuyAManager = m_achievementEvents.BuyAManagerStatus,
                BuyAllManagers = m_achievementEvents.BuyAllManagersStatus,

                BuyAnItem = m_achievementEvents.BuyAnItemStatus,
                BuyEachItemOnce = m_achievementEvents.BuyEachItenOnceStatus,

                DefeatRoshan = m_achievementEvents.DefeatRoshanStatus,
                DefeatRoshanTenTimes = m_achievementEvents.DefeatRoshanTenTimesStatus,

                TheAegisIsMine = m_achievementEvents.AegisIsMineStatus,
                CheeseGromit = m_achievementEvents.CheeseGromitStatus,

                TheClosestYoullGetToABattleCup = m_achievementEvents.ClosestYoullGetStatus,
                WhenDidEGThrowLast = m_achievementEvents.EGThrowLastStatus,
                TheManTheMythTheLegend = m_achievementEvents.ManMythLegendStatus,
            }
        };

        /*
         * Hero Order in List:
         * 0 = Alchemist, 1 = Ogre, 2 = Tusk, 3 = Io, 4 = AntiMagi, 5 = Sven, 6 = Phoenix, 7 = Rubick
         */        
        saveFile.RadiantSide.Heroes = new List<HeroDto>();
        for (int i = 0; i < SceneHeroes.Count; i++)
        {
            saveFile.RadiantSide.Heroes.Add(new HeroDto()
            {
                HeroName = SceneHeroes[i].HeroName,
                ClickersBought = SceneHeroes[i].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(i),

                Ability1Level = SceneHeroes[i].Ability1Level,
                Ability1UseCount = SceneHeroes[i].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[i].m_ability1ClickTime != DateTime.MinValue ? (DateTime.Now - SceneHeroes[i].m_ability1ClickTime).TotalSeconds : 0,
                Ability1InUse = SceneHeroes[i].Ability1InUse,

                Ability2Level = SceneHeroes[i].Ability2Level,
                Ability2UseCount = SceneHeroes[i].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[i].m_ability2ClickTime != DateTime.MinValue ? (DateTime.Now - SceneHeroes[i].m_ability2ClickTime).TotalSeconds : 0,
                Ability2InUse = SceneHeroes[i].Ability2InUse,

                ModifierActive = SceneHeroes[i].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[i].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[i].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateModifierTimeRemaining(SceneHeroes[i].m_currentModifierRoutineStarted, SceneHeroes[i].m_currentModifierTotalTime) : 0,

                HasManager = SceneHeroes[i].HasManager,
            });
        }

        saveFile.Roshan = new RoshanDto()
        {
            DefeatCount = RoshanEventCount,
            DurationTillNextSpawn = m_roshanEventInProgress ? 0 : m_secondsToRoshanEvent,
            CanDoRoshanEvents = m_canDoRoshanEvent,
        };

        try
        {
            string json = JsonConvert.SerializeObject(saveFile, Formatting.Indented);
            File.WriteAllText(SAVE_FILE_LOCATION, json);
        }
        catch (Exception e)
        {
            Debug.Log("Exception occured: - Can't save file");
        }
    }

    private double CalculateModifierTimeRemaining(DateTime timeStartedOn, double totalTime)
    {
        if (totalTime == -1)
            return 0;
        else
        {
            var start = DateTime.Now - timeStartedOn;
            return totalTime - start.TotalSeconds;
        }
    }

    void CheckSaveFileFolders()
    {
        if (!Directory.Exists(FILE_PATHS))
            Directory.CreateDirectory(FILE_PATHS);

        if (!File.Exists(SAVE_FILE_LOCATION))
            File.Create(SAVE_FILE_LOCATION).Close();
    }

    public void OnDestroy()
    {
        SaveFile();
        SaveConfig();
    }

    void DoRoshanEvent()
    {
        if (m_roshanEventInProgress)
            return;

        RoshanEventCount++; //Increment count of how many roshan events

        int roshanCount = UnityEngine.Random.Range(0, RoshanPrefab.Length);
        ActiveRoshan = Instantiate(RoshanPrefab[roshanCount]);
        m_activeRoshan = ActiveRoshan.GetComponent<RoshanController>();

        m_roshanEventInProgress = true;
    }

    void OnRoshanEventEnded()
    {
        //Roshan Event has ended, been killed
        //Instantiate cheese & aegis
        var aegis = Instantiate(AegisPrefab);
        aegis.transform.position = m_activeRoshan.Roshan.transform.position + new Vector3(0f, 7f, 0f); //Spawn it above the floor
        aegis.transform.rotation = new Quaternion(aegis.transform.rotation.x, aegis.transform.rotation.y + 15f, aegis.transform.rotation.z, aegis.transform.rotation.w); //Give rotation for velocity
        aegis.GetComponent<Rigidbody>().velocity += new Vector3(0f, 0f, 7f); //Give velocity

        if(RoshanEventCount > 3)
        {
            var cheese = Instantiate(CheesePrefab);
            cheese.transform.position = m_activeRoshan.Roshan.transform.position + new Vector3(0f, 7f, 0f);
            cheese.transform.rotation = new Quaternion(cheese.transform.rotation.x, cheese.transform.rotation.y - 15f, cheese.transform.rotation.z, cheese.transform.rotation.w);
            cheese.GetComponent<Rigidbody>().velocity += new Vector3(0f, 0f, 7f);
        }

        m_roshanEventInProgress = false;

        if(RoshanEventCount == 10)
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.DefeatRoshanTenTimes.Invoke();
            Debug.Log("Defeat Roshan 10 Times Achievements");
        }
        else if (RoshanEventCount >= 1)
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.DefeatRoshan.Invoke();
            Debug.Log("Defeat Roshan Achievements");
        }
    }

    void OnRoshanEventEndedNotKilled()
    {
        foreach (Transform child in ActiveRoshan.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(ActiveRoshan);

        m_roshanEventInProgress = false;

        if (RoshanEventCount != 0)
            RoshanEventCount--; //Minus since it was an unsucessful event
    }

    /// <summary>
    /// Called when buying abilities for heroes Phoenix and above. Switches toggle to be able to 
    /// start the RoshanCountdown timer
    /// </summary>
    public void EnableRoshanEvents()
    {
        if (!m_canDoRoshanEvent)
            m_canDoRoshanEvent = true;
        else
            return;
    }

    /// <summary>
    /// Called to pick a random time between 20 mins (1600 s) and an hour (3600 s) to start the roshan event
    /// </summary>
    void StartRoshanCountdown()
    {
        if (!m_canDoRoshanEvent && m_roshanWaitingToSpawn)
            return;

        //Todo: Sound to indicate that roshan events can happen
        if(m_secondsToRoshanEvent == -1)
        {
            m_secondsToRoshanEvent = UnityEngine.Random.Range(1200, 3600); //Can do event between 20 mins or a hour
        }

        StartCoroutine(TriggerRoshanEvent(m_secondsToRoshanEvent));
        Debug.Log("Seconds to Roshan Event: '" + m_secondsToRoshanEvent + "'");
        m_roshanWaitingToSpawn = true;
    }

    IEnumerator TriggerRoshanEvent(float time)
    {
        yield return new WaitForSeconds(time);

        DoRoshanEvent(); //Do the event

        m_roshanWaitingToSpawn = false;
    }

    public ConfigDto LoadConfig()
    {
        ConfigDto config;

        if(!Directory.Exists(FILE_PATHS))
            Directory.CreateDirectory(FILE_PATHS);
        
        if (!File.Exists(CONFIG_LOCATION))
        {
            //File doesnt exist. Create default file with content
            var stream = File.Create(CONFIG_LOCATION);
            stream.Close();

            config = new ConfigDto();
            if (m_options != null)
            {
                //Set defaults in quotes for other users
                config = GetLatestConfigValues();
            }

            config.TwitchUsername = "";
            config.TwitchAuthCode = "";
            
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(CONFIG_LOCATION, json);

            CurrentConfigFile = config;

            if (LoadedConfigFile != null && config != null)
                LoadedConfigFile.Invoke(config);

            return config;
        }
        else
        {
            //File exists. Load it
            string content = File.ReadAllText(CONFIG_LOCATION);
            try
            {
                config = JsonConvert.DeserializeObject<ConfigDto>(content);
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't deserialize Config File");
                return null;
            }


            if (LoadedConfigFile != null)
                LoadedConfigFile.Invoke(config);

            return config;
        }
    }

    ConfigDto GetLatestConfigValues()
    {
        var config = CurrentConfigFile;
        config.TwitchAuthCode = CurrentConfigFile.TwitchAuthCode;
        config.TwitchUsername =CurrentConfigFile.TwitchUsername;

        config.Preferences = new PreferencesDto()
        {
            MasterVolume = m_options.MasterVolSlider.value,
            AmbientVolume = m_options.AmbientVolSlider.value,
            HeroVolume = m_options.HeroVolSlider.value,
            MusicEnabled = m_options.IsMusicEnabled,
            AllAudioEnabled = m_options.AllAudioDisabled,
            SuperSampleScale = m_options.SuperSampleValue,
        };
        return config;
    }

    public void SaveConfig()
    {
        if (CurrentConfigFile == null)
        {
            Debug.LogError("Can't save config file because it's null");
            return;
        }

        CheckSaveFileFolders();

        CurrentConfigFile = GetLatestConfigValues();

        try
        {
            var json = JsonConvert.SerializeObject(CurrentConfigFile, Formatting.Indented);
            File.WriteAllText(CONFIG_LOCATION, json);
        }
        catch (Exception e)
        {
            Debug.Log("Can't save config file - " + e.Message);
        }
    }

    public void OnLoadedSave(SaveFileDto saveFile)
    {
        if(saveFile != null)
        {
            if(saveFile.RadiantSide != null)
            {
                TotalGold = saveFile.RadiantSide.TotalGold;
                m_canDoRoshanEvent = saveFile.RadiantSide.RoshanEvents;
            }
            if (saveFile.Roshan != null)
            {
                m_secondsToRoshanEvent = saveFile.Roshan.DurationTillNextSpawn;
                RoshanEventCount = saveFile.Roshan.DefeatCount;
                m_canDoRoshanEvent = saveFile.Roshan.CanDoRoshanEvents;

                StartRoshanCountdown();
            }
        }
    }

    public void ReturnToMainMenu()
    {
        if (OnReturningToMainMenu != null)
            OnReturningToMainMenu.Invoke();

        SaveFile();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void AddToClickTotal(int amount)
    {
        //amount should always be zero
        ClickCount += amount;

        CheckTotalClickAchievements();
    }

    public void AddToTotal(decimal amount, double abilityMultiplier, double itemMultiplier)
    {
        if (amount <= 0)
            return;

        if(abilityMultiplier <= 0 && itemMultiplier > 0)
        {
            //an item is active and no abilities in use
            TotalGold += Math.Round(amount * (decimal)itemMultiplier, 0, MidpointRounding.ToEven);
        }
        else if(abilityMultiplier > 0 && itemMultiplier <= 0)
        {
            //ability in use but no item
            TotalGold += Math.Round(amount * (decimal)abilityMultiplier, 0, MidpointRounding.ToEven);
        }
        else if(abilityMultiplier <= 0 && itemMultiplier > 0)
        {
            //ability and item multiplier is in use
            var totalMultiplier = abilityMultiplier * itemMultiplier;
            TotalGold += Math.Round(amount * (decimal)totalMultiplier, 0, MidpointRounding.ToEven);
        }
        else if(abilityMultiplier < 0 && itemMultiplier < 0)
        {
            //nothing in use
            TotalGold += amount;
        }
        else
        {
            Debug.Log("Can't add amount to TotalGold. You're IF statement is shit");
            return;
        }

        CheckTotalGoldAchievements();
    }

    void CheckTotalGoldAchievements()
    {
        //reverse order
        if (TotalGold >= 1000000 && !m_achievementEvents.EarnMillionGoldStatus)
        {
            m_achievementEvents.EarnMillionGold.Invoke();
        }
        else if (TotalGold >= 100000 && !m_achievementEvents.Earn100000GoldStatus)
        {
            m_achievementEvents.Earn100000Gold.Invoke();
        }
        else if (TotalGold >= 15000 && !m_achievementEvents.Earn15000GoldStatus)
        {
            m_achievementEvents.Earn15000Gold.Invoke();
        }
        else if (TotalGold >= 6200 && !m_achievementEvents.Earn6200GoldStatus)
        {
            m_achievementEvents.Earn6200Gold.Invoke();
        }
        else if (TotalGold >= 625 && !m_achievementEvents.Earn625GoldStatus)
        {
            m_achievementEvents.Earn625Gold.Invoke();
        }
    }

    void CheckTotalClickAchievements()
    {
        //reverse order
        if (ClickCount >= 50000 && !m_achievementEvents.ClickFiftyThousandStatus)
        {
            m_achievementEvents.EarnMillionGold.Invoke();
        }
        else if (TotalGold >= 15000 && !m_achievementEvents.ClickFifteenThousandStatus)
        {
            m_achievementEvents.Earn100000Gold.Invoke();
        }
        else if (TotalGold >= 1000 && !m_achievementEvents.ClickThousandStatus)
        {
            m_achievementEvents.Earn15000Gold.Invoke();
        }
        else if (ClickCount >= 500 && !m_achievementEvents.ClickFiveHundredStatus)
        {
            m_achievementEvents.Earn6200Gold.Invoke();
        }
        else if (ClickCount >= 1 && !m_achievementEvents.ClickOnceStatus)
        {
            m_achievementEvents.Earn625Gold.Invoke();
        }

    }

    public void RemoveFromTotal(decimal amount)
    {
        if (amount <= 0) return;

        var checkBelowZero = TotalGold - amount;
        if(checkBelowZero > 0)
        {
            TotalGold -= amount;
        }
    }

    public void SetPlayerMode(bool isVR)
    {
        Debug.Log("VRMode is " + VRSettings.enabled);

        if (isVR)
        {
            foreach (GameObject obj in VRPlayerItems)
            {
                foreach(Transform child in obj.transform)
                {
                    child.gameObject.SetActive(true);
                }
                obj.SetActive(true);
            }

            foreach (GameObject obj in NonVRPlayerItems)
            {
                foreach (Transform child in obj.transform)
                {
                    child.gameObject.SetActive(false);
                }
                obj.SetActive(false);
            }

            m_vrInputModule.SetActive(true);
            m_inputModule.SetActive(false);
        }
        else
        {
            foreach (GameObject obj in VRPlayerItems)
            {
                foreach (Transform child in obj.transform)
                {
                    child.gameObject.SetActive(false);
                }
                obj.SetActive(false);
            }

            foreach (GameObject obj in NonVRPlayerItems)
            {
                foreach (Transform child in obj.transform)
                {
                    child.gameObject.SetActive(true);
                }
                obj.SetActive(true);
            }

            m_vrInputModule.SetActive(false);
            m_inputModule.SetActive(true);
        }
        SetUISystemActive(isVR);

    }

    void SetUISystemActive(bool vrEnabled)
    {
        //Disable UIPointers & UICanvas which set eventsystem to work with VR
        GameObject.Find("LeftController").GetComponent<VRTK_UIPointer>().enabled = vrEnabled;
        GameObject.Find("RightController").GetComponent<VRTK_UIPointer>().enabled = vrEnabled;
        var allPointers = GameObject.FindObjectsOfType<VRTK_UIPointer>();
        for (int i = 0; i < allPointers.Length; i++)
        {
            allPointers[i].GetComponent<VRTK_UIPointer>().enabled = vrEnabled;
        }

        var allVRCanvas = FindObjectsOfType<VRTK_UICanvas>();
        for (int i = 0; i < allVRCanvas.Length; i++)
        {
            allVRCanvas[i].GetComponent<VRTK_UICanvas>().enabled = vrEnabled;
        }
    }
}
