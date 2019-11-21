using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour {

	public float delay;
	public float team;

	public GameObject bot_Team1;
	public GameObject bot_Team2;
	// Use this for initialization
	void Start () {
		delay = 5;
	}
	
	// Update is called once per frame
	void Update () {
		delay -= Time.deltaTime;
		if (delay <= 0) {
			if (team == 1) {
				Transform.Instantiate (bot_Team1, transform.position, transform.rotation);
				delay = 5;
			}if (team == 2) {
				Transform.Instantiate (bot_Team2, transform.position, transform.rotation);
				delay = 5;
			}
		}
	}
}
