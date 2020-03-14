using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreepRed : MonoBehaviour
{
    private float speed = 5f;
    public GameObject creepBulletPrefab = null;
    private bool haveTarget;
    public PhotonTeams.Team botTeam = PhotonTeams.Team.Red;
    [SerializeField] private float lengthBeam;
    RaycastHit info;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
        if (Physics.Raycast(transform.position, transform.right, out info, lengthBeam))
        {
            if (info.collider.gameObject.tag == "Player")
            {
                speed = 0;
            }
            else
            {
                speed = 5f;
            }
        }
    }

    private void Shoot()
    {
        if(haveTarget)
        {
            Bullet bulletGameobject = PhotonNetwork.Instantiate(creepBulletPrefab.name, transform.position, transform.rotation).GetComponent<Bullet>();
            Invoke("Shoot", 2f);
        }
    }
}
