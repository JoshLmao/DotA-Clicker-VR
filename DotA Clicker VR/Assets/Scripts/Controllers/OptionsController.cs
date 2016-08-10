using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

    Toggle m_audioEnabled;
    Slider m_masterVolumeSlider;

	void Start ()
    {
        m_masterVolumeSlider = transform.Find("MasterVolSlider").GetComponent<Slider>();
        m_audioEnabled = transform.Find("AudioEnabledToggle").GetComponent<Toggle>();
	}
	
	void Update ()
    {
        if(m_audioEnabled.isOn)
        {
            AudioListener.volume = 0;
        }

        AudioListener.volume = m_masterVolumeSlider.value;
	}
}
