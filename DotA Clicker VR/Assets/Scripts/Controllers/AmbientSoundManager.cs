using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmbientSoundManager : MonoBehaviour {

    public AudioClip[] AmbientClips;
    public AudioSource AudioSource;
    public Slider MasterVolumeSlider;
    
	void Start ()
    {
        AudioSource = GameObject.Find("AmbientAudio").GetComponent<AudioSource>();
        MasterVolumeSlider = GameObject.Find("MasterVolSlider").GetComponent<Slider>();
        //InvokeRepeating("PlayAmbientSound", 1, 1f); //Comment out to stop audio playing
    }

	void Update ()
    {
        if (AudioSource.isPlaying) return;

        AudioListener.volume = MasterVolumeSlider.value;
	}

    void PlayAmbientSound()
    {
        int randomClips = Random.Range(0, AmbientClips.Length);
        if (!AudioSource.isPlaying)
        {
            AudioSource.PlayOneShot(AmbientClips[randomClips]);
            Debug.Log("Playing Ambient Sound");
        }
    }
}
