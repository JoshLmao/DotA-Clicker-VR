using UnityEngine;
using System.Collections;

public class MainMenuTestButton : MonoBehaviour {

    public AudioClip[] TestButtonResponses;

    AudioSource m_audioSource;
    Animator m_animator;

	void Start ()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponent<Animator>();
	}
	
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ViveController")
        {
            PlayResponse();
        }
    }

    public void PlayResponse()
    {
        if (m_audioSource.isPlaying)
            return;

        m_animator.SetTrigger("isClicked");
        int rng = Random.Range(0, TestButtonResponses.Length);
        m_audioSource.PlayOneShot(TestButtonResponses[rng]);
    }

    public bool IsPlayingSound()
    {
        return m_audioSource.isPlaying;
    }
}
