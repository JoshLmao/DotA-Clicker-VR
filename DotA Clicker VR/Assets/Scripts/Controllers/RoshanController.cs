using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoshanController : MonoBehaviour {

    //Events to say Event has started & ended
    public delegate void OnRoshanEventStarted();
    public static event OnRoshanEventStarted RoshanEventStarted;

    public delegate void OnRoshanEventEnded();
    public static event OnRoshanEventEnded RoshanEventEnded;

    public GameObject Roshan;
    /// <summary>
    /// Amount needed for the player to earn to kill roshan the first time. Multiply this by event times count
    /// </summary>
    public float m_roshanStartHealth;
    /// <summary>
    /// Bool to see if Roshan has been killed
    /// </summary>
    public bool isDead = false;
    public AudioSource RoshanAudioSource;

    RadiantSceneController m_sceneController;
    Animator m_roshanAnimator;
    Image m_activeHealth;
    /// <summary>
    /// Amount of gold the player had when the roshan event started
    /// </summary>
    float m_playerGoldOnStart = 1;
    /// <summary>
    /// Current gold the player has now. Updating every frame
    /// </summary>
    float m_playerCurrentGold;
    bool hasReachedPoint = false;
    /// <summary>
    /// How many times the event has 
    /// </summary>
    float EventCount;

    void Start ()
    {
        if (RoshanEventStarted != null)
            RoshanEventStarted.Invoke();

        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        EventCount = m_sceneController.RoshanEventCount; //Set event count from scene controller

        Roshan = this.gameObject;
        m_roshanAnimator = this.GetComponent<Animator>();
        m_activeHealth = transform.Find("RoshanHealthUI/HealthBarRedActive").GetComponent<Image>();
        RoshanAudioSource = this.GetComponent<AudioSource>();
    }
	
	void Update ()
    {
        m_playerCurrentGold = m_sceneController.TotalGold;

        if(!hasReachedPoint)
        {
            Roshan.transform.position += new Vector3(0, 0, 4f * Time.deltaTime);
        }

        if(m_playerCurrentGold > 0 && m_playerCurrentGold > 0 && !isDead)
        {
            //float healthValue = (m_playerGoldOnStart / m_playerCurrentGold) / ;
            float difference = m_playerCurrentGold - m_playerGoldOnStart;
            float actualHP = m_roshanStartHealth - difference;


            //return (float)dividend.Ticks / (float)divisor.Ticks;
            float scaledValue = (actualHP - 0) / (m_roshanStartHealth - 0);

            m_activeHealth.fillAmount = scaledValue;

            if (m_activeHealth.fillAmount <= 0)
            {
                OnKilled();
                isDead = true;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Trigger" && col.name == "RoshanStandPoint")
        {
            m_roshanAnimator.SetTrigger("hasReachedPosition");
            hasReachedPoint = true;
        }
    }

    public void OnKilled()
    {
        transform.Find("RoshanHealthUI").gameObject.SetActive(false);

        m_roshanAnimator.SetBool("isDead", true);

        if (RoshanEventEnded != null)
            RoshanEventEnded.Invoke(); //Publish that the event has ended

        RoshanAudioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/Gameplay/Roshan/Roshan_Death"));

        StartCoroutine(WaitForRoshanDeathAnim()); //Waits for roshan to do anim then deletes
    }

    IEnumerator WaitForRoshanDeathAnim()
    {
        yield return new WaitForSeconds(2f);

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(this);
    }
}
