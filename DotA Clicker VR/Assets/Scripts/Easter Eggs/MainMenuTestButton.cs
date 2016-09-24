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
            m_animator.SetTrigger("isClicked");
        }
    }

    void PlayResponse()
    {
        if (m_audioSource.isPlaying)
            return;

        int rng = Random.Range(0, TestButtonResponses.Length);
        m_audioSource.PlayOneShot(TestButtonResponses[rng]);
    }
}
