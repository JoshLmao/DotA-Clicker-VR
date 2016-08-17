using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	void Start ()
    {
	
	}
	
	void Update ()
    {
	
	}

    public void OnQuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Close Application");
#else
        Application.Quit();
#endif
    }

    public void OnLoadRadiantLevel()
    {
        SceneManager.LoadScene("RadiantSide");
    }
}
