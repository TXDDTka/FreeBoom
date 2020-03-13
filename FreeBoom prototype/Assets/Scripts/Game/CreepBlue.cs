using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreepBlue : MonoBehaviour
{
    private float speed = 5f;
    public GameObject creepBulletPrefab = null;
    private bool haveTarget;
    public PhotonTeams.Team botTeam = PhotonTeams.Team.Blue;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + -speed * Time.deltaTime, transform.position.y);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 8 && other.tag == "Player")
    //    {
    //        speed = 0;
    //        Bullet bulletGameobject = PhotonNetwork.Instantiate(creepBulletPrefab.name, transform.position, transform.rotation).GetComponent<Bullet>();
    //        haveTarget = true;
    //        Invoke("Shoot", 1f);
            
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == 8 && other.tag == "Player")
    //    {
    //        speed = 5;
    //        haveTarget = false;
    //    }
    //}

    private void Shoot()
    {
        if(haveTarget)
        {
            Bullet bulletGameobject = PhotonNetwork.Instantiate(creepBulletPrefab.name, transform.position, transform.rotation).GetComponent<Bullet>();
            Invoke("Shoot", 2f);
        }
    }
}
