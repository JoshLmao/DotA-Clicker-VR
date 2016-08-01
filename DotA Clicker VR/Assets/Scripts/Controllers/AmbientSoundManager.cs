using UnityEngine;
using System.Collections;

public class AmbientSoundManager : MonoBehaviour {

    public AudioClip[] AmbientClips;
    public AudioSource AudioSource;

	void Start ()
    {
        AudioSource = GameObject.Find("AmbientAudio").GetComponent<AudioSource>();
        //InvokeRepeating("PlayAmbientSound", 1, 10f); //Comment out to stop audio playing
    }

	void Update ()
    {
        if (AudioSource.isPlaying) return;
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
