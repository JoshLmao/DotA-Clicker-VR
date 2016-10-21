using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;

public class RadiantSceneController : MonoBehaviour
{
    public delegate void OnLoadedSaveFile(SaveFileDto saveFile);
    public static event OnLoadedSaveFile LoadedSaveFile;
    public delegate void OnLoadedConfigFile(ConfigDto config);
    public static event OnLoadedConfigFile LoadedConfigFile;
    public delegate void ReturningToMainMenu();
    public static event ReturningToMainMenu OnReturningToMainMenu;

    public List<RadiantClickerController> SceneHeroes;
    public SaveFileDto CurrentSaveFile;
    public ConfigDto CurrentConfigFile;
    public int TotalGold = 3000;
    public GameObject[] RoshanPrefab;
    public GameObject AegisPrefab;
    public GameObject CheesePrefab;

    public GameObject[] ItemModifierDisplayPrefabs;

    public const string THOUSAND_FORMAT = "{0}, {1}, {2}";
    public const string MILLION_FORMAT = "{0} million";
    public const string BILLION_FORMAT = "{0} billion";
    public const string TRILLION_FORMAT = "{0} trillion";
    public const string QUADRILLION_FORMAT = "{0} quadrillion";

    readonly string FILE_PATHS = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\DotAClickerVR\\";
    readonly string SAVE_FILE = "SaveFile.json";
    readonly string CONFIG_FILE = "ConfigFile.json";

    public float ClickCount;

    string SAVE_FILE_LOCATION { get { return FILE_PATHS + SAVE_FILE; } }
    string CONFIG_LOCATION { get { return FILE_PATHS + CONFIG_FILE; } }

    Text m_goldUI;
    OptionsController m_options;
    float m_totalPlayTime = 0;
    
    //Roshan Controls
    public bool m_canDoRoshanEvent = false;
    /// <summary>
    /// Controller for the current or last roshan event
    /// </summary>
    RoshanController m_activeRoshan;
    public float RoshanEventCount = 0;
    bool m_roshanWaitingToSpawn = false;
    bool m_roshanEventInProgress = false;
    GameObject ActiveRoshan;
    AchievementEvents m_achievementEvents;
    BuyableItemsController modifierController = null;

    void Awake()
    {
        RoshanController.RoshanEventEnded += OnRoshanEventEnded;
        RoshanController.RoshanEventEndedNotKilled += OnRoshanEventEndedNotKilled;

        LoadedSaveFile += OnLoadedSave;

        CurrentConfigFile = LoadConfig();

        modifierController = GameObject.Find("ItemsListCanvas").GetComponent<BuyableItemsController>();

        m_options = GameObject.Find("OptionsCanvas").GetComponent<OptionsController>();
        m_achievementEvents = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();

        m_goldUI = GameObject.Find("UI/WorldSpaceUI/TotalGoldCanvas/AllGold/TotalGoldText").GetComponent<Text>();
    }

    void Start ()
    {
        SceneHeroes = GetClickerHeroesInScene();

        LoadProgress();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DoRoshanEvent();
        }

        if (m_canDoRoshanEvent && !m_roshanWaitingToSpawn)
            StartRoshanCountdown();

        m_goldUI.text = TotalGold.ToString();
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
        }

        if (LoadedSaveFile != null)
            LoadedSaveFile.Invoke(CurrentSaveFile);

        OnLoadedSave(CurrentSaveFile);

        if (CurrentSaveFile.PlayerName.ToLower() == "420bootywizard")
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.TheManTheMythTheLegend.Invoke();
            Debug.Log("420BootyWizard Achievements");
        }
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
    }

    double CorrectTimeRemaining(int heroIndex)
    {
        double timeRemaining = (TimeSpan.FromSeconds(SceneHeroes[heroIndex].SecondsToCompleteClick) - SceneHeroes[heroIndex].CurrentClickerTime).TotalSeconds;
        return timeRemaining != SceneHeroes[heroIndex].SecondsToCompleteClick ? timeRemaining : 0;
    }

    public void SaveFile()
    {
        //Check if folders & file exists
        SaveFileExists();

        //Add current playtime to total play time
        m_totalPlayTime += Time.realtimeSinceStartup;
        bool hasSaveFile = false;

        if (CurrentSaveFile != null)
            hasSaveFile = true;

        //Calculate TimeSpans for ModiferTimeRemaining
        //foreach(RadiantClickerController clicker in SceneHeroes)
        //{
            
        //}

        //Save data
        SaveFileDto saveFile = new SaveFileDto()
        {
            PlayerName = "Test",
            RadiantSide = new RadiantSideDto()
            {
                TotalGold = TotalGold,



                RoshanEvents = m_canDoRoshanEvent,
            },
            Preferences = new PreferencesDto()
            {
                MasterVolume = m_options.MasterVolSlider.value,
                AmbientVolume = m_options.AmbientVolSlider.value,
                HeroVolume = m_options.HeroVolSlider.value,
                MusicEnabled = m_options.IsMusicEnabled,
                AllAudioEnabled = m_options.AllAudioDisabled,
                SuperSampleScale = m_options.SuperSampleValue,
            },
            SessionStats = new StatsDto()
            {
                TotalPlayTime = hasSaveFile ? CurrentSaveFile.SessionStats.TotalPlayTime += m_totalPlayTime : m_totalPlayTime,
                ClickCount = ClickCount,

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
                DongsOutForBulldog = m_achievementEvents.DongsOutStatus,
            }
        };

        /*
         * Hero Order in List:
         * 0 = Alchemist, 1 = Ogre, 2 = Tusk, 3 = Io, 4 = AntiMagi, 5 = Sven, 6 = Phoenix, 7 = Rubick
         */

        saveFile.RadiantSide.Heroes = new List<HeroDto>()
        {
            new HeroDto()
            {
                HeroName = "Crystal Maiden",
                ClickersBought = SceneHeroes[3].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(3),

                Ability1Level = SceneHeroes[3].Ability1Level,
                Ability1UseCount = SceneHeroes[3].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[3].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[3].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[3].Ability2Level,
                Ability2UseCount = SceneHeroes[3].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[3].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[3].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[3].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[3].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[3].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[3].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[3].HasManager,
            },
            new HeroDto()
            {
                HeroName = "Rubick",
                ClickersBought = SceneHeroes[7].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(7),

                Ability1Level = SceneHeroes[7].Ability1Level,
                Ability1UseCount = SceneHeroes[7].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[7].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[7].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[7].Ability2Level,
                Ability2UseCount = SceneHeroes[7].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[7].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[7].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[7].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[7].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[7].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[7].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[7].HasManager,
            },
            new HeroDto()
            {
                HeroName = "Ogre Magi",
                ClickersBought = SceneHeroes[1].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(1),

                Ability1Level = SceneHeroes[1].Ability1Level,
                Ability1UseCount = SceneHeroes[1].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[1].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[1].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[1].Ability2Level,
                Ability2UseCount = SceneHeroes[1].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[1].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[1].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[1].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[1].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[1].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[1].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[1].HasManager,
            },
            new HeroDto()
            {
                HeroName = "Tusk",
                ClickersBought = SceneHeroes[2].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(2),

                Ability1Level = SceneHeroes[2].Ability1Level,
                Ability1UseCount = SceneHeroes[2].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[2].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[2].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[2].Ability2Level,
                Ability2UseCount = SceneHeroes[2].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[2].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[2].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[2].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[2].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[2].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[2].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[2].HasManager,
            },
            new HeroDto()
            {
                HeroName = "Phoenix",
                ClickersBought = SceneHeroes[6].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(6),

                Ability1Level = SceneHeroes[6].Ability1Level,
                Ability1UseCount = SceneHeroes[6].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[6].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[6].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[6].Ability2Level,
                Ability2UseCount = SceneHeroes[6].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[6].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[6].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[6].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[6].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[6].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[6].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[6].HasManager,
            },
            new HeroDto()
            {
                HeroName = "Sven",
                ClickersBought = SceneHeroes[5].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(5),

                Ability1Level = SceneHeroes[5].Ability1Level,
                Ability1UseCount = SceneHeroes[5].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[5].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[5].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[5].Ability2Level,
                Ability2UseCount = SceneHeroes[5].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[5].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[5].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[5].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[5].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[5].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[5].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[5].HasManager,
            },
            new HeroDto()
            {
                HeroName = "AntiMage",
                ClickersBought = SceneHeroes[4].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(4),

                Ability1Level = SceneHeroes[4].Ability1Level,
                Ability1UseCount = SceneHeroes[4].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[4].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[4].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[4].Ability2Level,
                Ability2UseCount = SceneHeroes[4].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[4].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[4].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[4].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[4].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[4].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[4].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[4].HasManager,
            },
            new HeroDto()
            {
                HeroName = "Alchemist",
                ClickersBought = SceneHeroes[0].ClickerMultiplier,
                ClickerTimeRemaining = CorrectTimeRemaining(0),

                Ability1Level = SceneHeroes[0].Ability1Level,
                Ability1UseCount = SceneHeroes[0].Ability1UseCount,
                Ability1RemainingTime = SceneHeroes[0].m_ability1ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[0].m_ability1ClickTime) : 0,

                Ability2Level = SceneHeroes[0].Ability2Level,
                Ability2UseCount = SceneHeroes[0].Ability2UseCount,
                Ability2RemainingTime = SceneHeroes[0].m_ability2ClickTime != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[0].m_ability2ClickTime) : 0,

                ModifierActive = SceneHeroes[0].m_currentModifierRoutineStarted == DateTime.MinValue ? false : true,
                CurrentModifier = SceneHeroes[0].m_currentModifier,
                ModifierTimeRemaining = SceneHeroes[0].m_currentModifierRoutineStarted != DateTime.MinValue ? CalculateTimeRemaining(SceneHeroes[0].m_currentModifierRoutineStarted) : 0,

                HasManager = SceneHeroes[0].HasManager,
            },
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

    void SaveFileExists()
    {
        if (!Directory.Exists(FILE_PATHS))
            Directory.CreateDirectory(FILE_PATHS);

        if (!File.Exists(SAVE_FILE_LOCATION))
            File.Create(SAVE_FILE_LOCATION).Close();
    }

    public void OnDestroy()
    {
        SaveFile();
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

        if(RoshanEventCount == 1)
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.DefeatRoshan.Invoke();
            Debug.Log("Defeat Roshan Achievements");
        }
        else if(RoshanEventCount == 10)
        {
            AchievementEvents events = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
            events.DefeatRoshanTenTimes.Invoke();
            Debug.Log("Defeat Roshan 10 Times Achievements");
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
        int secondsToEvent = UnityEngine.Random.Range(1200, 3600); //Can do event between 20 mins or a hour
        StartCoroutine(TriggerRoshanEvent(secondsToEvent));
        Debug.Log("Seconds to Roshan Event: '" + secondsToEvent + "'");
        m_roshanWaitingToSpawn = true;
    }

    IEnumerator TriggerRoshanEvent(float time)
    {
        GameObject.Find("waitingOnRoshanTimer").GetComponent<Text>().text = time.ToString();
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

            config = new ConfigDto()
            {
                TwitchUsername = "DotAClickerVR",
                TwitchAuthCode = "oauth:93tmro12txrri3b1mobo6mirxz0wg9",
            };

            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(CONFIG_LOCATION, json);
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

            if(LoadedConfigFile != null)
                LoadedConfigFile.Invoke(config);

            return config;
        }
    }

    public void OnLoadedSave(SaveFileDto saveFile)
    {
        TotalGold = saveFile.RadiantSide.TotalGold;
        m_canDoRoshanEvent = saveFile.RadiantSide.RoshanEvents;
    }

    double CalculateTimeRemaining(DateTime start)
    {
        var span = start - DateTime.Now;
        return span.TotalSeconds;
    }

    public void ReturnToMainMenu()
    {
        if (OnReturningToMainMenu != null)
            OnReturningToMainMenu.Invoke();

        SaveFile();

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
