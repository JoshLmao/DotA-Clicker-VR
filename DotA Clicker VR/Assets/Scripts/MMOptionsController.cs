﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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

    protected virtual void Awake()
    {
        m_audioEnabled = transform.Find("AudioOptions/AudioEnabledToggle").GetComponent<Toggle>();
        m_musicEnabled = transform.Find("AudioOptions/AmbientAudioEnabled").GetComponent<Toggle>();
        m_adaptiveQuality = transform.Find("VideoOptions/AdaptiveQualityToggle").GetComponent<Toggle>();

        MasterVolSlider = transform.Find("AudioOptions/MasterVolumeC/MasterVolSlider").GetComponent<Slider>();
        AmbientVolSlider = transform.Find("AudioOptions/AmbientVolC/AmbientMusicVolSlider").GetComponent<Slider>();
        HeroVolSlider = transform.Find("AudioOptions/HeroSoundsVolC/HeroesVolSlider").GetComponent<Slider>();

        SuperSampleSlider = transform.Find("VideoOptions/SSCanvas/SuperSampleSlider").GetComponent<Slider>();
        m_ssText = transform.Find("VideoOptions/SSCanvas/SSValue").GetComponent<Text>();
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
            m_currentConfig.Preferences = new PreferencesDto()
            {
                MasterVolume = MasterVolSlider.value,
                AmbientVolume = AmbientVolSlider.value,
                HeroVolume = HeroVolSlider.value,
                MusicEnabled = m_audioEnabled.isOn,
                AllAudioEnabled = m_audioEnabled.isOn,
                SuperSampleScale = SuperSampleSlider.value,
            };
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
            SuperSampleScale = SuperSampleSlider.value,
        };

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
}