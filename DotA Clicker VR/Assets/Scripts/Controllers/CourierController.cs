using UnityEngine;
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
    Transform m_playerTransform;

    [SerializeField]
    Sprite m_toggleEnabled;
    [SerializeField]
    Sprite m_toggleDisabled;

    TwitchIRC m_twitchChat;
    KeyboardController m_keyboardController;
    GameObject m_keyboard;
    GameObject m_streamURLUI;
    GameObject m_displayKeyboardBtn;
    RadiantSceneController m_sceneController;
    GameObject m_invalidTwitchAuth;

    MediaPlayerCtrl m_mediaController;

    void Start ()
    {
        m_sceneController = GameObject.Find("RadiantSceneController").GetComponent<RadiantSceneController>();

        m_courier = this.gameObject;
        m_stream = transform.Find("TwitchStreamCanvas/StreamTexture").GetComponent<GUITexture>();
        m_streamAudio = transform.Find("TwitchStreamCanvas/StreamTexture").GetComponent<AudioSource>();
        m_player = GameObject.Find("[CameraRig]").gameObject;
        courierWaypoint = GameObject.Find("CourierWaypoint");
        m_crowAnimator = transform.Find("BabyRoshanModel").GetComponent<Animator>();
        m_followBtn = transform.Find("TwitchStreamCanvas/FollowBtn").gameObject;
        m_lockRotationBtn = transform.Find("TwitchStreamCanvas/LockRotationBtn").gameObject;
        m_audioMutedBtn = transform.Find("TwitchStreamCanvas/StreamAudioMutedBtn").gameObject;
        m_twitchChat = transform.Find("TwitchStreamCanvas/TwitchChat").GetComponent<TwitchIRC>();
        m_displayKeyboardBtn = transform.Find("TwitchStreamCanvas/ChangeStreamBtn").gameObject;

        m_invalidTwitchAuth = transform.Find("TwitchStreamCanvas/TwitchChat/ChatScrollable/InvalidAuthKey").gameObject;
        m_invalidTwitchAuth.SetActive(false);

        m_keyboardController = transform.Find("Keyboard").GetComponent<KeyboardController>();
        KeyboardController.EnterPressed += KeyboardEnter;

        m_keyboard = transform.Find("Keyboard").gameObject;
        m_streamURLUI = transform.Find("TwitchStreamCanvas/URL").gameObject;

        m_twitchChat.channelName = "";
        m_twitchChat.StartIRC();

        m_keyboard.SetActive(false);
        m_streamURLUI.SetActive(false);

        if(m_sceneController.CurrentConfigFile != null)
        {
            m_twitchChat.oauth = m_sceneController.CurrentConfigFile.TwitchAuthCode;
            m_twitchChat.nickName = m_sceneController.CurrentConfigFile.TwitchUsername;

            m_twitchChat.StartIRC();
        }
        else
        {
            m_invalidTwitchAuth.SetActive(true);
        }

        m_mediaController = transform.Find("TwitchStreamCanvas/StreamTexture").GetComponent<MediaPlayerCtrl>();
    }

    void Update ()
    {
        if(!isByPlayer && FollowPlayer)
        {
            m_playerTransform = GameObject.Find("[CameraRig]").transform;
            waypointPos = new Vector3(courierWaypoint.transform.position.x, m_playerTransform.position.y, courierWaypoint.transform.position.z);
            transform.position = Vector3.Lerp(transform.position, waypointPos, speed * Time.deltaTime);
            m_crowAnimator.SetBool("isMoving", true);
        }
        else if(isByPlayer)
        {
            m_crowAnimator.SetBool("isMoving", false);
        }   

        if(!LockRotation)
        {
            Quaternion rotation = Quaternion.LookRotation(m_player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        }

        //Update InputText
        m_streamURLUI.transform.Find("InputStream").GetComponent<Text>().text = m_keyboardController.Input;
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

            if(LockRotation)
            {
                ToggleRotationLock();
            }
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
        if(m_streamAudio == null)
            m_streamAudio = transform.Find("TwitchStreamCanvas/StreamTexture").GetComponent<AudioSource>();

        AudioMuted = !AudioMuted;
        m_audioMutedBtn.GetComponent<Toggle>().isOn = AudioMuted;

        if (AudioMuted)
            m_mediaController.SetVolume(0);
        else
            m_mediaController.SetVolume(1);

    }

    public void DisplayKeyboard()
    {
        m_displayKeyboardBtn.GetComponent<Image>().sprite = m_toggleEnabled;

        m_keyboard.SetActive(true);
        m_streamURLUI.SetActive(true);

        m_keyboardController.ClearStream();
    }

    public void KeyboardEnter()
    {
        m_twitchChat.stopThreads = true;
        m_displayKeyboardBtn.GetComponent<Image>().sprite = m_toggleDisabled;

        m_twitchChat.channelName = m_keyboardController.Input;
        m_twitchChat.stopThreads = false;
        m_twitchChat.StartIRC();

        m_keyboard.SetActive(false);
        m_streamURLUI.SetActive(false);

        m_twitchChat.ClearMessageList();
        m_keyboardController.ClearStream();
    }

    public void RetryOAuth()
    {
        if (m_sceneController.CurrentConfigFile != null)
        {
            m_twitchChat.stopThreads = true;
            m_twitchChat.ClearMessageList();
            m_sceneController.LoadConfig();

            m_twitchChat.oauth = m_sceneController.CurrentConfigFile.TwitchAuthCode;
            m_twitchChat.nickName = m_sceneController.CurrentConfigFile.TwitchUsername;
            m_twitchChat.stopThreads = false;
            m_twitchChat.StartIRC();
        }
        else
        {
            Debug.Log("Config File is null");
        }
    }
}
