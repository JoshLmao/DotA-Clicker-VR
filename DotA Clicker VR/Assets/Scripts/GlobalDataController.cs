using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataController : MonoBehaviour {

    public string PlayerName = string.Empty;

    void Awake()
    {

    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void SetPlayerName(string playerName)
    {
        PlayerName = playerName;
    }
}
