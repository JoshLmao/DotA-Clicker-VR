using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject wayPoint;
    float timer = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            //The position of the waypoint will update to the player's position
            //UpdatePosition();
            timer = 0.5f;
        }
    }

    void UpdatePosition()
    {
        //The wayPoint's position will now be the player's current position.
        wayPoint.transform.position = transform.position;
    }
}
