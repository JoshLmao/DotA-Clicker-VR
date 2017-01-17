using UnityEngine;
using System.Collections;
using System;

public class GlobalObjectsController : MonoBehaviour
{
    public GameObject[] LoadingScreens;
    public string CurrentPlayerName = string.Empty;

    GameObject m_loadingScreens;

    void Awake()
    {
        RadiantSceneController.OnSceneStarted += OnRadiantSceneLoaded;
    }

    private void OnRadiantSceneLoaded()
    {
        var sceneController = GameObject.Find("RadiantSceneController");
        if (sceneController != null)
            sceneController.GetComponent<RadiantSceneController>().CurrentPlayerName = CurrentPlayerName;
    }

    void Start ()
    {
        m_loadingScreens = transform.Find("LoadingScreens").gameObject;

        SetDontDestroys();
        OnChangeScene(); //Set initial loading screen
	}

    void SetDontDestroys()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Transform child in this.transform)
        {
            DontDestroyOnLoad(child.gameObject);
        }
    }

    public void OnChangeScene()
    {
        CurrentPlayerName = GameObject.Find("PlayerNameStand").GetComponent<PlayerName>().Name;

        int rng = UnityEngine.Random.Range(0, LoadingScreens.Length);
        foreach(GameObject obj in LoadingScreens)
        {
            obj.SetActive(false);
        }

        LoadingScreens[rng].SetActive(true);
    }
}
