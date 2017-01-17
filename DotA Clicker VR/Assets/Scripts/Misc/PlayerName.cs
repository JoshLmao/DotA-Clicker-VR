using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour {

    public string Name = string.Empty;

    [SerializeField]
    KeyboardController m_keyboard;

    [SerializeField]
    Text m_playername;

    void Awake()
    {

    }

	void Start ()
    {
		
	}
	
	void Update ()
    {
        m_playername.text = m_keyboard.Input;
    }
}
