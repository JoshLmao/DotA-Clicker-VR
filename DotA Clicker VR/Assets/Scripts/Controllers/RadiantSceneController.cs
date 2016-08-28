using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;

public class RadiantSceneController : MonoBehaviour
{
    public List<RadiantClickerController> SceneHeroes;
    public SaveFileDto CurrentSaveFile;
    public int TotalGold = 3000;
    public GameObject[] RoshanPrefab;
    public GameObject AegisPrefab;
    public GameObject CheesePrefab;

    public const string THOUSAND_FORMAT = "{0}, {1}, {2}";
    public const string MILLION_FORMAT = "{0} million";
    public const string BILLION_FORMAT = "{0} billion";
    public const string TRILLION_FORMAT = "{0} trillion";
    public const string QUADRILLION_FORMAT = "{0} quadrillion";
    readonly string SAVE_FILE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JoshLmao\\";
    readonly string SAVE_FILE = "SaveFile.json";

    string SAVE_FILE_LOCATION { get { return SAVE_FILE_PATH + SAVE_FILE; } }
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

    void Start ()
    {
        LoadProgress();
        m_goldUI = GameObject.Find("TotalGoldText").GetComponent<Text>();

        SceneHeroes = GetClickerHeroesInScene();
        m_options = GameObject.Find("OptionsCanvas").GetComponent<OptionsController>();

        RoshanController.RoshanEventEnded += OnRoshanEventEnded;
	}

	void Update ()
    {
        m_goldUI.text = TotalGold.ToString();

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

        //Deserialize existing file
        string file = File.ReadAllText(SAVE_FILE_LOCATION);
        SaveFileDto loadedSaveFile = JsonConvert.DeserializeObject<SaveFileDto>(file);
        CurrentSaveFile = loadedSaveFile;
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

    public void SaveFile()
    {
        //Check if folders & file exists
        SaveFileExists();

        //Add current playtime to total play time
        m_totalPlayTime += Time.realtimeSinceStartup;

        //Save data
        SaveFileDto saveFile = new SaveFileDto()
        {
            PlayerName = "Test",
            RadiantSide = new RadiantSideDto()
            {
                TotalGold = TotalGold,
                /*
                 * Hero Order in List:
                 * 0 = Alchemist, 1 = Ogre, 2 = Tusk, 3 = Io, 4 = AntiMagi, 5 = Sven, 6 = Phoenix, 7 = Rubick
                 */
                Io = new HeroDto()
                {
                    ClickersBought = SceneHeroes[3].ClickerMultiplier,
                    Ability1Level = SceneHeroes[3].Ability1Level,
                    Ability2Level = SceneHeroes[3].Ability2Level,
                },
                Rubick = new HeroDto()
                {
                    ClickersBought = SceneHeroes[7].ClickerMultiplier,
                    Ability1Level = SceneHeroes[7].Ability1Level,
                    Ability2Level = SceneHeroes[7].Ability2Level,
                },
                OgreMagi = new HeroDto()
                {
                    ClickersBought = SceneHeroes[1].ClickerMultiplier,
                    Ability1Level = SceneHeroes[1].Ability1Level,
                    Ability2Level = SceneHeroes[1].Ability2Level,
                },
                Tusk = new HeroDto()
                {
                    ClickersBought = SceneHeroes[2].ClickerMultiplier,
                    Ability1Level = SceneHeroes[2].Ability1Level,
                    Ability2Level = SceneHeroes[2].Ability2Level,
                },
                Phoenix = new HeroDto()
                {
                    ClickersBought = SceneHeroes[6].ClickerMultiplier,
                    Ability1Level = SceneHeroes[6].Ability1Level,
                    Ability2Level = SceneHeroes[6].Ability2Level,
                },
                Sven = new HeroDto()
                {
                    ClickersBought = SceneHeroes[5].ClickerMultiplier,
                    Ability1Level = SceneHeroes[5].Ability1Level,
                    Ability2Level = SceneHeroes[5].Ability2Level,
                },
                AntiMage = new HeroDto()
                {
                    ClickersBought = SceneHeroes[4].ClickerMultiplier,
                    Ability1Level = SceneHeroes[4].Ability1Level,
                    Ability2Level = SceneHeroes[4].Ability2Level,
                },
                Alchemist = new HeroDto()
                {
                    ClickersBought = SceneHeroes[0].ClickerMultiplier,
                    Ability1Level = SceneHeroes[0].Ability1Level,
                    Ability2Level = SceneHeroes[0].Ability2Level,
                },
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
                TotalPlayTime = CurrentSaveFile != null ? CurrentSaveFile.SessionStats.TotalPlayTime += m_totalPlayTime : m_totalPlayTime,
            }
        };

        string json = JsonConvert.SerializeObject(saveFile, Formatting.Indented);
        File.WriteAllText(SAVE_FILE_LOCATION, json);
    }

    void SaveFileExists()
    {
        if (!Directory.Exists(SAVE_FILE_PATH))
            Directory.CreateDirectory(SAVE_FILE_PATH);

        if (!File.Exists(SAVE_FILE_LOCATION))
            File.Create(SAVE_FILE_LOCATION).Dispose();
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
        var roshanPrefab = Instantiate(RoshanPrefab[roshanCount]);
        m_activeRoshan = roshanPrefab.GetComponent<RoshanController>();

        m_roshanEventInProgress = true;
    }

    void OnRoshanEventEnded()
    {
        //Roshan Event has ended
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
        m_roshanWaitingToSpawn = true;
    }

    IEnumerator TriggerRoshanEvent(float time)
    {
        yield return new WaitForSeconds(time);

        DoRoshanEvent(); //Do the event

        m_roshanWaitingToSpawn = false;
    }
}
