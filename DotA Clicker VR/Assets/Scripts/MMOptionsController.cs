using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using UnityStandardAssets.ImageEffects;

public class MMOptionsController : MonoBehaviour
{
    public Slider MasterVolSlider;
    public Slider AmbientVolSlider;
    public Slider HeroVolSlider;
    public Slider SuperSampleSlider;

    protected Text m_ssText;
    protected Toggle m_audioEnabled;
    protected Toggle m_musicEnabled;
    protected Toggle m_adaptiveQuality;

    protected ConfigDto m_currentConfig;

    protected Slider m_fieldOfViewSlider;
    protected Camera m_fpsCamera;
    protected float m_currentFieldOfView = 60;

    protected virtual void Awake()
    {
        m_audioEnabled = transform.Find("AudioOptions/AudioEnabledToggle").GetComponent<Toggle>();
        m_musicEnabled = transform.Find("AudioOptions/AmbientAudioEnabled").GetComponent<Toggle>();

        MasterVolSlider = transform.Find("AudioOptions/MasterVolumeC/MasterVolSlider").GetComponent<Slider>();
        AmbientVolSlider = transform.Find("AudioOptions/AmbientVolC/AmbientMusicVolSlider").GetComponent<Slider>();
        HeroVolSlider = transform.Find("AudioOptions/HeroSoundsVolC/HeroesVolSlider").GetComponent<Slider>();

        if (VRSettings.enabled)
        {
            m_adaptiveQuality = transform.Find("VideoOptions/VRItems/AdaptiveQualityToggle").GetComponent<Toggle>();
            SuperSampleSlider = transform.Find("VideoOptions/VRItems/SSCanvas/SuperSampleSlider").GetComponent<Slider>();
            m_ssText = transform.Find("VideoOptions/VRItems/SSCanvas/SSValue").GetComponent<Text>();
        }
        else
        {
            m_fieldOfViewSlider = GameObject.Find("FoVCanvas").transform.Find("Slider").GetComponent<Slider>();
            m_fpsCamera = GameObject.Find("FirstPersonCharacterCamera").GetComponent<Camera>();
        }


        if (m_fieldOfViewSlider != null)
            m_fieldOfViewSlider.onValueChanged.AddListener(FoVChanged);
        FoVChanged(m_currentFieldOfView);
    }

    protected virtual void Start()
    {
        CreateConfigFromExistingValues();
        SetConfigValues();
    }

    public void OnApplicationQuit()
    {
        SaveCurrentValuesToConfig();
    }

    protected void CreateConfigFromExistingValues()
    {
        if(m_currentConfig == null)
            m_currentConfig = new ConfigDto();

        ValidateLocations();

        string configLoc = RadiantSceneController.FILE_PATHS + RadiantSceneController.CONFIG_FILE;
        string json = File.ReadAllText(configLoc);
        if (json.Length == 0)
        {
            m_currentConfig.Preferences = new PreferencesDto();
            m_currentConfig.Preferences.MasterVolume = MasterVolSlider != null ? MasterVolSlider.value : 0.5f;
            m_currentConfig.Preferences.AmbientVolume = AmbientVolSlider != null ? AmbientVolSlider.value : 0.5f;
            m_currentConfig.Preferences.HeroVolume = HeroVolSlider != null ? HeroVolSlider.value : 0.5f;
            m_currentConfig.Preferences.MusicEnabled = m_audioEnabled != null ? m_audioEnabled.isOn : true;
            m_currentConfig.Preferences.AllAudioEnabled = m_audioEnabled != null ? m_audioEnabled.isOn : true;
        }
        else
        {
            m_currentConfig = JsonConvert.DeserializeObject<ConfigDto>(json);
        }

        string toJson = JsonConvert.SerializeObject(m_currentConfig, Formatting.Indented);
        File.WriteAllText(configLoc, toJson);
    }


    protected void SaveCurrentValuesToConfig()
    {
        if (m_currentConfig == null) return;

        string configLoc = RadiantSceneController.FILE_PATHS + RadiantSceneController.CONFIG_FILE;
        m_currentConfig.Preferences = new PreferencesDto()
        {
            MasterVolume = MasterVolSlider.value,
            AmbientVolume = AmbientVolSlider.value,
            HeroVolume = HeroVolSlider.value,
            MusicEnabled = m_audioEnabled.isOn,
            AllAudioEnabled = m_audioEnabled.isOn,
        };
        if(VRSettings.enabled)
        {
            m_currentConfig.Preferences.SuperSampleScale = SuperSampleSlider.value;
        }

        string toJson = JsonConvert.SerializeObject(m_currentConfig, Formatting.Indented);
        File.WriteAllText(configLoc, toJson);

    }

    protected void SetConfigValues()
    {
        ValidateLocations();
        if (m_currentConfig == null) return;

        MasterVolSlider.value = m_currentConfig.Preferences.MasterVolume;
        AmbientVolSlider.value = m_currentConfig.Preferences.AmbientVolume;
        HeroVolSlider.value = m_currentConfig.Preferences.HeroVolume;
        m_audioEnabled.isOn = m_currentConfig.Preferences.MusicEnabled;
        m_audioEnabled.isOn = m_currentConfig.Preferences.AllAudioEnabled;
        if (SuperSampleSlider != null)
            SuperSampleSlider.value = m_currentConfig.Preferences.SuperSampleScale;
    }

    protected void ValidateLocations()
    {
        if (!Directory.Exists(RadiantSceneController.FILE_PATHS))
            Directory.CreateDirectory(RadiantSceneController.FILE_PATHS);

        string configLoc = RadiantSceneController.FILE_PATHS + RadiantSceneController.CONFIG_FILE;
        if (!File.Exists(configLoc))
        {
            File.Create(configLoc).Close();
        }
    }


    protected void FoVChanged(float value)
    {
        if (m_fpsCamera != null)
        {
            m_fpsCamera.fieldOfView = m_currentFieldOfView;
        }

        if(m_fieldOfViewSlider != null)
        {
            m_fieldOfViewSlider.value = m_currentFieldOfView;
            m_fieldOfViewSlider.gameObject.transform.parent.transform.Find("Value").GetComponent<Text>().text = value.ToString();
        }
    }

    public void AddToFoV()
    {
        if (m_currentFieldOfView + 5 < 120)
            m_currentFieldOfView += 5;

        FoVChanged(m_currentFieldOfView);
    }

    public void MinusToFoV()
    {
        if (m_currentFieldOfView - 5 > 45)
            m_currentFieldOfView -= 5;

        FoVChanged(m_currentFieldOfView);
    }

    public void ToggleSSAO(bool status)
    {
        if (VRSettings.enabled) return;

        GameObject.Find("FirstPersonCharacterCamera").GetComponent<ScreenSpaceAmbientOcclusion>().enabled = status;
    }

    public void ToggleAntialiasing(bool status)
    {
        if (VRSettings.enabled) return;

        GameObject.Find("FirstPersonCharacterCamera").GetComponent<Antialiasing>().enabled = status;
    }

    public void ToggleGlobalFog(bool status)
    {
        if (VRSettings.enabled) return;

        GameObject.Find("FirstPersonCharacterCamera").GetComponent<GlobalFog>().enabled = status;
    }
}
