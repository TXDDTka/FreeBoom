using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PhotonView PV;
    private Rigidbody rb = null;
    private float damageAmount = 0f;
    private Coroutine lastRoutine = null;
    [SerializeField]private float timeToDestroyBullet = 0f;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!PV.IsMine) return;
        lastRoutine = StartCoroutine(destoyBullet());

    }

    public void Set(Vector3 velocity, float damage)
    {
        rb.velocity = velocity;
        damageAmount = damage;
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
                StopCoroutine(lastRoutine);
            }
      }
        else
        {
            PhotonNetwork.Destroy(gameObject);
            StopCoroutine(lastRoutine);
        }
    }



    public IEnumerator destoyBullet()
    {
        yield return new WaitForSeconds(timeToDestroyBullet);
        PhotonNetwork.Destroy(gameObject);
    }

}
