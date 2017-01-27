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

    float m_currentFieldOfView = 60;
    Slider m_fieldOfViewSlider;
    Camera m_fpsCamera;

    protected override void Awake()
    {
        base.Awake();

        m_ambientSound = GameObject.Find("RadiantSceneController").GetComponent<AmbientSoundManager>();
        m_options = GameObject.Find("OptionsCanvas").GetComponent<OptionsController>();

        m_fieldOfViewSlider = GameObject.Find("FoVCanvas").transform.Find("Slider").GetComponent<Slider>();
        m_fpsCamera = GameObject.Find("FirstPersonCharacterCamera").GetComponent<Camera>();

        RadiantSceneController.LoadedConfigFile += OnLoadedConfig;
    }

    protected override void Start()
    {
        base.Start();

        if(SuperSampleSlider != null)
            SuperSampleSlider.onValueChanged.AddListener(SuperSampleChanged);

        if(m_fieldOfViewSlider != null)
            m_fieldOfViewSlider.onValueChanged.AddListener(FoVChanged);
        FoVChanged(m_currentFieldOfView);

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

    public void AddToFoV()
    {
        if(m_currentFieldOfView + 5 < 120)
            m_currentFieldOfView += 5;

        FoVChanged(m_currentFieldOfView);
    }

    public void MinusToFoV()
    {
        if(m_currentFieldOfView - 5 > 45)
            m_currentFieldOfView -= 5;

        FoVChanged(m_currentFieldOfView);
    }

    void FoVChanged(float value)
    {
        if(m_fpsCamera != null)
        {
            m_fpsCamera.fieldOfView = m_currentFieldOfView;
        }

        m_fieldOfViewSlider.value = m_currentFieldOfView;
        m_fieldOfViewSlider.gameObject.transform.parent.transform.Find("Value").GetComponent<Text>().text = value.ToString();
    }
}
