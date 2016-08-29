using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmbientSoundManager : MonoBehaviour {

    public AudioClip[] AmbientClips;
    public AudioSource AmbientAudioSource;

    void Start()
    {
        AmbientAudioSource = GameObject.Find("AmbientAudio").GetComponent<AudioSource>();

        //Repeats ambient music, comment out whilst developing
        InvokeRepeating("PlayAmbientSound", 1, 1f);
    }

    void PlayAmbientSound()
    {
        int randomClips = Random.Range(0, AmbientClips.Length);
        if (!AmbientAudioSource.isPlaying)
        {
            AmbientAudioSource.PlayOneShot(AmbientClips[randomClips]);
            Debug.Log("Playing Ambient Sound");
        }
    }

    public void StopInvokeRepeating()
    {
        AmbientAudioSource.Stop();
        CancelInvoke("PlayAmbientSound");
    }

    public void StartInvokeRepeating()
    {
        InvokeRepeating("PlayAmbientSound", 1, 1f);
    }
}
