using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGun : MonoBehaviour
{
	public GameObject bot;
	public GameObject bullet_t1;
	public GameObject bullet_t2;
	//bot bullets

	public float speed = 1f;
	public float delay = 1;
	public bool enemy = false;
	public float hp = 50;

	public float team;
    // Start is called before the first frame update
    // Update is called once per frame
	void Start(){
		
	}
    void Update()
    {
		if (team == 1) {
			bot.GetComponent<Rigidbody2D> ().velocity = speed * Vector2.left;
		}
		if (team == 2) {
			bot.GetComponent<Rigidbody2D> ().velocity = speed * Vector2.right;
		}
		if (enemy) {
			speed = 0;
		}
		if (enemy) {
			delay = delay - Time.deltaTime;
			if (delay <= 0) {
				BotShoot();
				delay = 0.5f;
			}
		}
		if (hp <= 0) {
			Destroy (bot);
		}
    }
	public void OnTriggerEnter2D (Collider2D col){
		if (col.tag == "team1_object" && team == 2) {
			enemy = true;
			Debug.Log("Enemy Spotted!");
			delay = delay - Time.deltaTime;
			Quaternion rotation = Quaternion.LookRotation
				(col.transform.position - transform.position, transform.TransformDirection (Vector3.up));
			transform.rotation = new Quaternion (0, 0, rotation.z, rotation.w);
		}if (col.tag == "team2_object" && team == 1) {
			enemy = true;
			Debug.Log("Enemy Spotted!");
			delay = delay - Time.deltaTime;
			Quaternion rotation = Quaternion.LookRotation
				(col.transform.position - transform.position, transform.TransformDirection (Vector3.up));
			transform.rotation = new Quaternion (0, 0, rotation.z, rotation.w);
		}
	}
	public void OnTriggerExit2D (Collider2D col){
		if (col.tag == "team1_object" && team == 2) {
			enemy = false;
			Debug.Log("Enemy Lost!");
			speed = 1f;
			transform.rotation = new Quaternion(0, 0 ,0 ,0);
		}if (col.tag == "team2_object" && team == 1) {
			enemy = false;
			Debug.Log("Enemy Lost!");
			transform.rotation = new Quaternion(0, 0 ,0 ,0);
			speed = 1f;
		}
	}
	void BotShoot ()
	{
		if (team == 1) {
			Transform.Instantiate (bullet_t1, transform.position, transform.rotation);
		}if (team == 2) {
			Transform.Instantiate (bullet_t2, transform.position, transform.rotation);
		}
	}
	public void OnColissionEnter2D (Collider2D col) {
		if (team == 1 && col.tag == "bulletTeam_2") {
			hp -= 20;
		}if (team == 2 && col.tag == "bulletTeam_1") {
			hp -= 20;
		}
	}
}