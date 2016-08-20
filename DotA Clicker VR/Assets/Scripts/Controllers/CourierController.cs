﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CourierController : MonoBehaviour
{
    GameObject m_courier;
    GUITexture m_stream;
    AudioSource m_streamAudio;
    //string m_streamURL = "http://video59.fra01.hls.ttvnw.net/hls38/zai_22888226976_502560166/mobile/index-live.m3u8?token=id=3985712076563215819,bid=22888226976,exp=1471627091,node=video59.fra01,nname=video59.fra01,fmt=mobile&sig=d93ba79e56997e4fefe9a524147f16c4069a5047";
    WWW m_wwwData;

    static string STREAM_BASE = "https://player.twitch.tv/?channel=";
    string m_twitchChannel = "wagamamatv";
    string m_streamURL;

    public bool FollowPlayer = false;
    public float speed = 0.1f;

    public bool LockRotation = false;
    public float damping = 6.0f;

    public bool AudioMuted = false;

    GameObject courierWaypoint;
    GameObject m_player;
    Vector3 waypointPos;
    bool isByPlayer = false;
    Animator m_crowAnimator;
    GameObject m_followBtn;
    GameObject m_lockRotationBtn;
    GameObject m_audioMutedBtn;

    [SerializeField]
    Sprite m_toggleEnabled;
    [SerializeField]
    Sprite m_toggleDisabled;

    void Start ()
    {
        m_courier = this.gameObject;
        m_stream = transform.Find("TwitchStreamCanvas/StreamTexture").GetComponent<GUITexture>();
        m_streamAudio = transform.Find("TwitchStreamCanvas/StreamAudioSource").GetComponent<AudioSource>();
        m_player = GameObject.Find("[CameraRig]").gameObject;
        courierWaypoint = GameObject.Find("CourierWaypoint");
        m_crowAnimator = transform.Find("BabyRoshanModel").GetComponent<Animator>();
        m_followBtn = transform.Find("TwitchStreamCanvas/FollowBtn").gameObject;
        m_lockRotationBtn = transform.Find("TwitchStreamCanvas/LockRotationBtn").gameObject;
        m_audioMutedBtn = transform.Find("TwitchStreamCanvas/StreamAudioMutedBtn").gameObject;
        //m_streamURL = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";//STREAM_BASE + (m_twitchChannel == null ? "JoshLmao" : m_twitchChannel);
        //StartCoroutine(StreamStartup());
    }

    void Update ()
    {
        if(!isByPlayer && FollowPlayer)
        {
            waypointPos = new Vector3(courierWaypoint.transform.position.x, transform.position.y, courierWaypoint.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, waypointPos, speed * Time.deltaTime);
            //Vector3.MoveTowards(transform.position, waypointPos, speed * Time.deltaTime); //old
            m_crowAnimator.SetBool("isMoving", true);
        }
        else if(isByPlayer)
        {
            m_crowAnimator.SetBool("isMoving", false);
        }

        if(!LockRotation)
        {
            //transform.LookAt(m_player.transform, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, );

            Quaternion rotation = Quaternion.LookRotation(m_player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }
    }

    IEnumerator StreamStartup()
    {
        m_wwwData = new WWW(m_streamURL);
        Debug.Log(m_streamURL);
        yield return m_wwwData;
        RawImage renderer = m_stream.GetComponent<RawImage>();
        renderer.texture = m_wwwData.texture;

        //while (!movieTexture.isReadyToPlay)
        //{
        //    Debug.Log("Not ready to play");
        //    return;
        //}

        //var gt = m_stream;
        //gt.texture = movieTexture;

        //m_streamAudio.clip = movieTexture.audioClip;

        //movieTexture.Play();
        //m_streamAudio.Play();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "CourierTrigger")
        {
            isByPlayer = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "CourierTrigger")
        {
            isByPlayer = false;
        }
    }

    public void ToggleCourierFollow()
    {
        FollowPlayer = !FollowPlayer;

        if(FollowPlayer)
        {
            m_followBtn.GetComponent<Image>().sprite = m_toggleEnabled;
            m_followBtn.GetComponentInChildren<Text>().text = "Unfollow";
        }
        else
        {
            m_followBtn.GetComponent<Image>().sprite = m_toggleDisabled;
            m_followBtn.GetComponentInChildren<Text>().text = "Follow";
        }
    }

    public void ToggleRotationLock()
    {
        LockRotation = !LockRotation;

        if(LockRotation)
        {
            m_lockRotationBtn.GetComponent<Image>().sprite = m_toggleEnabled;
            m_lockRotationBtn.GetComponentInChildren<Text>().text = "Unlock Rotation";
        }
        else
        {
            m_lockRotationBtn.GetComponent<Image>().sprite = m_toggleDisabled;
            m_lockRotationBtn.GetComponentInChildren<Text>().text = "Lock Rotation";
        }
    }

    public void ToggleStreamAudio()
    {
        AudioMuted = !AudioMuted;
        m_audioMutedBtn.GetComponent<Toggle>().isOn = AudioMuted;
    }
}
