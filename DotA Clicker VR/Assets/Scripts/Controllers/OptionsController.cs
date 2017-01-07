using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.VR;
using System;

public class OptionsController : MonoBehaviour {

    public bool IsMusicEnabled { get { return m_audioEnabled.isOn; } }
    public bool AllAudioDisabled { get { return m_audioEnabled.isOn; } }
    public float SuperSampleValue { get { return SuperSampleSlider.value; } }

    public AudioSource[] HeroesAudioSource;
    public Slider MasterVolSlider;
    public Slider AmbientVolSlider;
    public Slider HeroVolSlider;
    public Slider SuperSampleSlider;

    Text m_ssText;
    AmbientSoundManager m_ambientSound;
    Toggle m_audioEnabled;
    Toggle m_musicEnabled;
    Toggle m_adaptiveQuality;

    void Awake()
    {
        m_audioEnabled = transform.Find("AudioOptions/AudioEnabledToggle").GetComponent<Toggle>();
        m_musicEnabled = transform.Find("AudioOptions/AmbientAudioEnabled").GetComponent<Toggle>();
        m_ambientSound = GameObject.Find("RadiantSceneController").GetComponent<AmbientSoundManager>();
        m_adaptiveQuality = transform.Find("VideoOptions/AdaptiveQualityToggle").GetComponent<Toggle>();

        MasterVolSlider = transform.Find("AudioOptions/MasterVolumeC/MasterVolSlider").GetComponent<Slider>();
        AmbientVolSlider = transform.Find("AudioOptions/AmbientVolC/AmbientMusicVolSlider").GetComponent<Slider>();
        HeroVolSlider = transform.Find("AudioOptions/HeroSoundsVolC/HeroesVolSlider").GetComponent<Slider>();

        SuperSampleSlider = transform.Find("VideoOptions/SSCanvas/SuperSampleSlider").GetComponent<Slider>();
        m_ssText = transform.Find("VideoOptions/SSCanvas/SSValue").GetComponent<Text>();

        RadiantSceneController.LoadedSaveFile += OnLoadedSaveFile;
    }

	void Start ()
    {
        SuperSampleSlider.onValueChanged.AddListener(SuperSampleChanged);

        m_audioEnabled.onValueChanged.AddListener(AudioToggle);
        m_musicEnabled.onValueChanged.AddListener(AmbientMusicToggle);

        SuperSampleChanged(0);
        SetAdaptiveQualityStatus(false);
        m_adaptiveQuality.isOn = false;

        m_ambientSound.AmbientAudioSource.volume = AmbientVolSlider.value;
    }

    void Update()
    {
        //Is Audio On?
        if (m_audioEnabled.isOn)
        {
            AudioListener.volume = 0;
            return;
        }
        //Master Volume
        AudioListener.volume = MasterVolSlider.value;
        //Hero Volume
        foreach (AudioSource source in HeroesAudioSource)
        {
            source.volume = HeroVolSlider.value;
        }
    }

    void AudioToggle(bool toggle)
    {
        if (m_audioEnabled.isOn)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
    }

    void AmbientMusicToggle(bool toggle)
    {
        if (m_musicEnabled.isOn)
        {
            m_ambientSound.StartInvokeRepeating();
            m_ambientSound.AmbientAudioSource.volume = AmbientVolSlider.value;
        }
        else
        {
            m_ambientSound.AmbientAudioSource.volume = 0f;
            m_ambientSound.StopInvokeRepeating();
        }
    }

    void SuperSampleChanged(float value)
    {
        VRSettings.renderScale = SuperSampleSlider.value;
        m_ssText.text = SuperSampleSlider.value.ToString(); //Math.Round(SuperSampleSlider.value, 2).ToString()
    }

    public void AddSuperSampleValue()
    {
        SuperSampleSlider.value += 0.1f;
    }

    public void MinusSuperSampleValue()
    {
        SuperSampleSlider.value -= 0.1f;
    }

    public void OnLoadedSaveFile(SaveFileDto saveFile)
    {
        PreferencesDto prefs = saveFile.Preferences;

        MasterVolSlider.value = prefs.MasterVolume;
        AmbientVolSlider.value = prefs.AmbientVolume;
        HeroVolSlider.value = prefs.HeroVolume;

        m_audioEnabled.isOn = prefs.MusicEnabled;
        m_audioEnabled.isOn = prefs.AllAudioEnabled;

        SuperSampleSlider.value = prefs.SuperSampleScale;
    }

    public void ToggleAdaptiveQuality(bool status)
    {
        SetAdaptiveQualityStatus(status);
    }

    void SetAdaptiveQualityStatus(bool status)
    {
        var adaptive = GameObject.Find("Camera (eye)").GetComponent<VRTK.VRTK_AdaptiveQuality>();

        if (status)
        {
            adaptive.enabled = true;
            SuperSampleSlider.onValueChanged.RemoveAllListeners();
        }
        else
        {
            SuperSampleSlider.onValueChanged.AddListener(SuperSampleChanged);
            adaptive.enabled = false;
        }
    }
}
