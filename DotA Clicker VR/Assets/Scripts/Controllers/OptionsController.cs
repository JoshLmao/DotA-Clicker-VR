using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

    public AudioSource[] HeroesAudioSource;
    public Slider MasterVolSlider;
    public Slider AmbientVolSlider;
    public Slider HeroVolSlider;

    AmbientSoundManager m_ambientSound;
    Toggle m_audioEnabled;
    Slider m_masterVolumeSlider;

	void Start ()
    {
        m_masterVolumeSlider = transform.Find("MasterVolSlider").GetComponent<Slider>();
        m_audioEnabled = transform.Find("AudioEnabledToggle").GetComponent<Toggle>();
        m_ambientSound = GameObject.Find("RadiantSceneController").GetComponent<AmbientSoundManager>();

        MasterVolSlider = transform.Find("MasterVolSlider").GetComponent<Slider>();
        AmbientVolSlider = transform.Find("AmbientMusicVolSlider").GetComponent<Slider>();
        HeroVolSlider = transform.Find("HeroesVolSlider").GetComponent<Slider>();

        //MasterVolSlider.onValueChanged.AddListener(SliderValuesChanged);
        //AmbientVolSlider.onValueChanged.AddListener(SliderValuesChanged);
        //HeroVolSlider.onValueChanged.AddListener(SliderValuesChanged);
        m_audioEnabled.onValueChanged.AddListener(ToggleValueChanged);
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
}
