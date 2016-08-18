using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.VR;
using System;

public class OptionsController : MonoBehaviour {

    public AudioSource[] HeroesAudioSource;
    public Slider MasterVolSlider;
    public Slider AmbientVolSlider;
    public Slider HeroVolSlider;
    public Slider SuperSampleSlider;
    Text m_ssText;

    AmbientSoundManager m_ambientSound;
    Toggle m_audioEnabled;

	void Start ()
    {
        m_audioEnabled = transform.Find("AudioOptions/AudioEnabledToggle").GetComponent<Toggle>();
        m_ambientSound = GameObject.Find("RadiantSceneController").GetComponent<AmbientSoundManager>();

        MasterVolSlider = transform.Find("AudioOptions/MasterVolumeC/MasterVolSlider").GetComponent<Slider>();
        AmbientVolSlider = transform.Find("AudioOptions/AmbientVolC/AmbientMusicVolSlider").GetComponent<Slider>();
        HeroVolSlider = transform.Find("AudioOptions/HeroSoundsVolC/HeroesVolSlider").GetComponent<Slider>();

        SuperSampleSlider = transform.Find("VideoOptions/SSCanvas/SuperSampleSlider").GetComponent<Slider>();
        m_ssText = transform.Find("VideoOptions/SSCanvas/SSValue").GetComponent<Text>();

        //MasterVolSlider.onValueChanged.AddListener(SliderValuesChanged);
        //AmbientVolSlider.onValueChanged.AddListener(SliderValuesChanged);
        //HeroVolSlider.onValueChanged.AddListener(SliderValuesChanged);
        SuperSampleSlider.onValueChanged.AddListener(SuperSampleChanged);

        m_audioEnabled.onValueChanged.AddListener(ToggleValueChanged);

        SuperSampleChanged(0);
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
        //Ambient Sound Volume
        m_ambientSound.AmbientAudioSource.volume = AmbientVolSlider.value;
        //Hero Volume
        foreach (AudioSource source in HeroesAudioSource)
        {
            source.volume = HeroVolSlider.value;
        }
    }

    void SliderValuesChanged(/*float anything*/)
    {
        //Set all volume levels
        //Is Audio On?
        if(m_audioEnabled.isOn)
        {
            AudioListener.volume = 0;
            return;
        }

        /*
        //Master Volume
        AudioListener.volume = MasterVolSlider.value;
        //Ambient Sound Volume
        m_ambientSound.AmbientAudioSource.volume = AmbientVolSlider.value;
        //Hero Volume
        foreach (AudioSource source in HeroesAudioSource)
        {
            source.volume = HeroVolSlider.value;
        }
        */
    }

    void ToggleValueChanged(bool toggle)
    {
        if (m_audioEnabled.isOn)
        {
            AudioListener.volume = 0;
            return;
        }
    }

    void SuperSampleChanged(float value)
    {
        //VRSettings.renderScale = SuperSampleSlider.value;
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
}
