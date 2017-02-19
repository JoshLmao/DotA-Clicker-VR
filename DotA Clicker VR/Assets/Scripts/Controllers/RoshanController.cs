using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RoshanController : MonoBehaviour {

    //Events to say Event has started & ended
    public delegate void OnRoshanEventStarted();
    public static event OnRoshanEventStarted RoshanEventStarted;

    public delegate void OnRoshanEventEnded();
    public static event OnRoshanEventEnded RoshanEventEnded;

    public delegate void OnRoshanEventEndedNotKilled();
    public static event OnRoshanEventEndedNotKilled RoshanEventEndedNotKilled;

    public GameObject Roshan;
    /// <summary>
    /// Amount needed for the player to earn to kill roshan the first time. Multiply this by event times count
    /// </summary>
    public float m_roshanStartHealth = 500;
    /// <summary>
    /// Bool to see if Roshan has been killed
    /// </summary>
    public bool isDead = false;
    public AudioSource RoshanAudioSource;

    //Between 1 and 0
    public float CurrentHealth;

    public double TimeRemaining = -1;

    RadiantSceneController m_sceneController;
    Animator m_roshanAnimator;
    Image m_activeHealth;
    /// <summary>
    /// Amount of gold the player had when the roshan event started
    /// </summary>
    public float m_playerGoldOnStart = -1;
    /// <summary>
    /// Current gold the player has now. Updating every frame
    /// </summary>
    float m_playerCurrentGold;
    bool hasReachedPoint = false;
    /// <summary>
    /// How many times the event has been done
    /// </summary>
    float EventCount;

    //Spline stuff
    public Transform[] waypoints;
    int currentWayPoint = 0;
    Transform targetWaypoint;
    float speed = 4f;
    bool goToHalfWay = true;
    bool goToHome = false;

    AchievementEvents m_achievementEvents;
    DateTime m_eventStartTime;
    DateTime m_predictedEndTime;

    void Awake()
    {
        RadiantSceneController.LoadedSaveFile += OnLoadSave;
        m_achievementEvents = GameObject.Find("Helpers/Events").GetComponent<AchievementEvents>();
    }

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

        if(m_playerGoldOnStart == -1)
        {
            m_playerGoldOnStart = (float)m_sceneController.TotalGold;
        }

        //Because Prefab doesn't save these
        waypoints[0] = GameObject.Find("Misc/RoshanWaypoints/1").gameObject.transform;
        waypoints[1] = GameObject.Find("Misc/RoshanWaypoints/2").gameObject.transform;
        waypoints[2] = GameObject.Find("Misc/RoshanWaypoints/3").gameObject.transform;
        waypoints[3] = GameObject.Find("Misc/RoshanWaypoints/4").gameObject.transform;
        waypoints[4] = GameObject.Find("Misc/RoshanWaypoints/5").gameObject.transform;
    }

	void Update()
    {
        m_playerCurrentGold = (float)m_sceneController.TotalGold;

        if (!hasReachedPoint && currentWayPoint < waypoints.Length)
        {
            if (targetWaypoint == null)
                targetWaypoint = waypoints[currentWayPoint];

            WalkAlongPath();
        }

        if(m_playerCurrentGold > 0 && !isDead)
        {
            float difference = m_playerCurrentGold - m_playerGoldOnStart;
            float actualHP = m_roshanStartHealth - difference;
            CurrentHealth = (actualHP - 0) / (m_roshanStartHealth - 0);

            m_activeHealth.fillAmount = CurrentHealth;

            if (m_activeHealth.fillAmount <= 0)
            {
                OnKilled();
                isDead = true;
            }
        }

        if(hasReachedPoint)
        {
            TimeRemaining = -(DateTime.Now - m_predictedEndTime).TotalSeconds;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Trigger" && col.name == "RoshanStandPoint" && !hasReachedPoint && goToHalfWay)
        {
            m_roshanAnimator.SetTrigger("hasReachedPosition");
            hasReachedPoint = true;

            goToHome = true;
            goToHalfWay = false;

            //Start countdown till event is over in seconds
            float time = (120 / EventCount) * 2; //Standard time of 120s. Should scale with amount of events

            if (TimeRemaining != -1)
            {
                //Loaded from save file

                StartCoroutine(EventEndTime((float)TimeRemaining));
                m_predictedEndTime = DateTime.Now.AddSeconds(TimeRemaining);
            }
            else
            {
                //Started normally
                StartCoroutine(EventEndTime(time));
                m_eventStartTime = DateTime.Now;
                m_predictedEndTime = DateTime.Now.AddSeconds(time);
            }

        }
        else if (col.tag == "Trigger" && col.name == "RoshanReturnPoint" && !hasReachedPoint && goToHome)
        {
            m_roshanAnimator.SetTrigger("hasReachedPosition");
            hasReachedPoint = true;

            goToHome = false;
            goToHalfWay = true;

            currentWayPoint = 0;

            if (RoshanEventEndedNotKilled != null)
                RoshanEventEndedNotKilled.Invoke(); //Event ended without being killed
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
            if (currentWayPoint > waypoints.Length - 1)
                return;
            targetWaypoint = waypoints[currentWayPoint];
        }
    }

    IEnumerator EventEndTime(float time)
    {
        Debug.Log("Roshan Event: Waiting '" + time + "' for event to end");
        yield return new WaitForSeconds(time);

        m_roshanAnimator.SetTrigger("doFirebreath");

        StartCoroutine(WaitForEventEndAnim());
    }

    IEnumerator WaitForEventEndAnim()
    {
        yield return new WaitForSeconds(7f); //Firebreath animation duration //6.4

        m_roshanAnimator.SetTrigger("isWalking");
        hasReachedPoint = false;
    }

    private void OnLoadSave(SaveFileDto saveFile)
    {
        if(saveFile != null && saveFile.Roshan != null)
        {
            
        }
    }
}
