using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TwitchCSharp.Clients;

public class TwitchStreamController : MonoBehaviour {

    GameObject m_streamObject;
    GUITexture m_streamGUITexture;
    MovieTexture m_streamTexture;
    AudioSource m_streamAudioSource;

    //string url = "http://video61.fra01.hls.ttvnw.net/hls-833ee8/canceldota_22789265568_499312128/medium/index-live.m3u8?token=id=6823583956500119618,bid=22789265568,exp=1471098606,node=video61.fra01,nname=video61.fra01,fmt=medium&sig=7282c51f37da2c3674c71b043cee2c56b761e551";
    //string url = "http://oneshot.qualcomm.com/webAR/content/strawberryfields_H264_AAC.mp4";
    string url = "http://unity3d.com/files/docs/sample.ogg";

    void Start ()
    {
        m_streamObject = transform.Find("StreamTexture").gameObject;
        m_streamAudioSource = transform.Find("StreamAudioSource").GetComponent<AudioSource>();
        m_streamGUITexture = m_streamObject.GetComponent<GUITexture>();

        PlayVideo();
    }

    void Update ()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            PlayVideo();
        }
	}

    IEnumerator PlayVideo()
    {
        Debug.Log("PlayVideo()");
        var www = new WWW(url);
        yield return www;

        m_streamTexture = www.movie;
        while (!m_streamTexture.isReadyToPlay)
        {
            Debug.Log("Not Ready Yet");
            yield break;
        }

        Debug.Log("Movie Ready");
        m_streamGUITexture.texture = m_streamTexture;

        m_streamObject.GetComponent<RawImage>().texture = m_streamTexture as MovieTexture;
        m_streamAudioSource.clip = m_streamTexture.audioClip;

        m_streamTexture.Play();
        m_streamAudioSource.Play();
    }
}
