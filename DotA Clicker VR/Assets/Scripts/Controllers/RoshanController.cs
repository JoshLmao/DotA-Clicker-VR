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


    //Spline stuff
    public Transform[] waypoints;
    int currentWayPoint = 0;
    Transform targetWaypoint;
    float speed = 4f;

    void Start()
    {
        if (RoshanEventStarted != null)
            RoshanEventStarted.Invoke();

        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();
        EventCount = m_sceneController.RoshanEventCount; //Set event count from scene controller

        Roshan = this.gameObject;
        m_roshanAnimator = this.GetComponent<Animator>();
        m_activeHealth = transform.Find("RoshanHealthUI/HealthBarRedActive").GetComponent<Image>();
        RoshanAudioSource = this.GetComponent<AudioSource>();

        //Health multiplied by eventCount divided by 1.6. Don't want to double health after every event
        if (EventCount > 0)
            m_roshanStartHealth *= (EventCount / 1.6f);

        waypoints[0] = GameObject.Find("Misc/RoshanWaypoints/1").gameObject.transform;
        waypoints[1] = GameObject.Find("Misc/RoshanWaypoints/2").gameObject.transform;
    }

	void Update()
    {
        m_playerCurrentGold = m_sceneController.TotalGold;

        if(!hasReachedPoint && currentWayPoint < waypoints.Length)
        {
            if (targetWaypoint == null)
                targetWaypoint = waypoints[currentWayPoint];

            WalkAlongPath();
        }

        if(m_playerCurrentGold > 0 && m_playerCurrentGold > 0 && !isDead)
        {
            float difference = m_playerCurrentGold - m_playerGoldOnStart;
            float actualHP = m_roshanStartHealth - difference;
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
            DestroyImmediate(child.gameObject);
        }
        DestroyImmediate(this.gameObject);
    }

    void WalkAlongPath()
    {
        // rotate towards the target
        transform.forward = Vector3.RotateTowards(transform.forward, targetWaypoint.position - transform.position, speed * Time.deltaTime, 0.0f);

        // move towards the target
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (transform.position == targetWaypoint.position)
        {
            currentWayPoint++;
            targetWaypoint = waypoints[currentWayPoint];
        }
    }
}
