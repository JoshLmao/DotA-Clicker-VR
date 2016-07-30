using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SceneController : SceneControllerBase
{    
    public List<HeroClicker> SceneHeroes { get; set; }

	void Start ()
    {
        LoadProgress();
	}

	void Update ()
    {
	
	}

    void FixedUpdate()
    {

    }

    public override void LoadProgress()
    {
        
    }
}
