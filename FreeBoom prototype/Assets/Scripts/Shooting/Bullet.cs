using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks/*,IPunPrefabPool//,IPunInstantiateMagicCallback*/
{
    private PhotonView PV;
    private Rigidbody rb = null;
    private float damageAmount = 0f;

   // [SerializeField] private float timeToDestroyBullet = 0f;
    private float distance = 0f;
    private Vector3 spawnPosition;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }


    public override void OnDisable()
    {
        base.OnDisable();
        if (!PV.IsMine) return;
        CancelInvoke();
    }

    //public void Set(Vector3 velocity, float damage, float destroyTime)
    //public void Set(float speed, float damage, float destroyTime)
    public void Set(Vector3 newSpawnPosition,float newDistance,float speed, float damage)//, float destroyTime)
    {
        spawnPosition = newSpawnPosition;
        distance = newDistance;
        //  rb.velocity = rb.transform.right * speed;
        //rb.velocity = velocity;
         rb.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
       // rb.velocity = transform.TransformDirection(Vector3.forward * speed);
        //rb.AddForce(bulletForece * 150, ForceMode.Impulse);
        //rb.AddRelativeForce(Vector2.right * 150, ForceMode.Impulse);
        damageAmount = damage;
       // timeToDestroyBullet = destroyTime;
        //Invoke("DestoyBullet", timeToDestroyBullet);
    }

    private void FixedUpdate()
    {
        float maxDistance = Vector3.Distance(spawnPosition, transform.position);
        if (maxDistance > distance)
        {
            //Debug.Log(maxDistance);
            rb.velocity = Vector3.zero;
        }

        //var heading = spawnPosition - transform.position;

        //if (heading.sqrMagnitude > distance * distance)
        //{
        //    Debug.Log(heading.sqrMagnitude);
        //    rb.velocity = Vector3.zero;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PV.IsMine) return;


        if (other.tag == "Player")
        { 
            if (other.GetComponent<PhotonView>().Owner.GetTeam() != PhotonNetwork.LocalPlayer.GetTeam())
            {
            
                other.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllViaServer, damageAmount, PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(gameObject);
            }
      }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void DestoyBullet()
    { 
        PhotonNetwork.Destroy(gameObject);
    }

}
