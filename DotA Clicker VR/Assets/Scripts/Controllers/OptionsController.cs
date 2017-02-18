using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.VR;
using System;
using UnityStandardAssets.ImageEffects;

public class OptionsController : MMOptionsController
{
    public bool IsMusicEnabled { get { return m_audioEnabled.isOn; } }
    public bool AllAudioDisabled { get { return m_audioEnabled.isOn; } }
    public float SuperSampleValue { get { return SuperSampleSlider.value; } }

    public AudioSource[] HeroesAudioSource;

    AmbientSoundManager m_ambientSound;
    OptionsController m_options;

    protected override void Awake()
    {
        base.Awake();

        m_ambientSound = GameObject.Find("RadiantSceneController").GetComponent<AmbientSoundManager>();
        m_options = GameObject.Find("OptionsCanvas").GetComponent<OptionsController>();


        RadiantSceneController.LoadedConfigFile += OnLoadedConfig;
    }

    protected override void Start()
    {
        base.Start();

        if(SuperSampleSlider != null)
            SuperSampleSlider.onValueChanged.AddListener(SuperSampleChanged);


        if(m_audioEnabled != null)
            m_audioEnabled.onValueChanged.AddListener(AudioToggle);
        if(m_musicEnabled != null)
            m_musicEnabled.onValueChanged.AddListener(AmbientMusicToggle);

        SuperSampleChanged(0);
        SetAdaptiveQualityStatus(false);
        if(m_adaptiveQuality != null)
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
        if(VRSettings.enabled)
        {
            VRSettings.renderScale = SuperSampleSlider.value;
            m_ssText.text = SuperSampleSlider.value.ToString(); //Math.Round(SuperSampleSlider.value, 2).ToString()
        }
    }

    public void AddSuperSampleValue()
    {
        SuperSampleSlider.value += 0.1f;
    }

    public void MinusSuperSampleValue()
    {
        SuperSampleSlider.value -= 0.1f;
    }

    private void OnLoadedConfig(ConfigDto config)
    {
        PreferencesDto prefs = null;
        if (config != null && config.Preferences != null)
            prefs = config.Preferences;
        else
            return;

        MasterVolSlider.value = prefs.MasterVolume;
        AmbientVolSlider.value = prefs.AmbientVolume;
        HeroVolSlider.value = prefs.HeroVolume;

        m_audioEnabled.isOn = prefs.MusicEnabled;
        m_audioEnabled.isOn = prefs.AllAudioEnabled;

        if(SuperSampleSlider != null)
            SuperSampleSlider.value = prefs.SuperSampleScale;
    }

    public void ToggleAdaptiveQuality(bool status)
    {
        SetAdaptiveQualityStatus(status);
    }

    void SetAdaptiveQualityStatus(bool status)
    {
        var objAdaptive = GameObject.Find("Camera (eye)");
        VRTK.VRTK_AdaptiveQuality adaptive = null;
        if (objAdaptive != null)
            adaptive = objAdaptive.GetComponent<VRTK.VRTK_AdaptiveQuality>();
        else
            return;

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
