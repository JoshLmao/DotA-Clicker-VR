using UnityEngine;
using System.Collections;

public class GlobalObjectsController : MonoBehaviour
{
    public GameObject[] LoadingScreens;

    GameObject m_loadingScreens;

	void Start ()
    {
        m_loadingScreens = transform.Find("LoadingScreens").gameObject;

        SetDontDestroys();
        OnChangeScene(); //Set initial loading screen
	}
	
	void Update ()
    {
	    
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
        int rng = Random.Range(0, LoadingScreens.Length);
        
        foreach(GameObject obj in LoadingScreens)
        {
            obj.SetActive(false);
        }

        LoadingScreens[rng].SetActive(true);
    }
}
