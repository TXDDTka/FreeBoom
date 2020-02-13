using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks/*,IPunPrefabPool//,IPunInstantiateMagicCallback*/
{
    private PhotonView PV;
    private Rigidbody rb = null;
    private float damageAmount = 0f;
    // private Coroutine lastRoutine = null;
    // private Coroutine bulletRoutine = null;
    [SerializeField] private float timeToDestroyBullet = 0f;
    // [SerializeField] private float timeToDeactivateBullet = 0f;
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
    public void Set(float speed, float damage, float destroyTime)
    {
      //  rb.velocity = rb.transform.right * speed;
        //rb.velocity = velocity;
        rb.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
        damageAmount = damage;
        timeToDestroyBullet = destroyTime;
        Invoke("DestoyBullet", timeToDestroyBullet);
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
