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

    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    if (!PV.IsMine) return;
    //    Invoke("DestoyBullet", timeToDestroyBullet);
    //}

    //void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    // e.g. store this gameobject as this player's charater in Player.TagObject
    //    //info.Sender.TagObject = gameObject;
    //}

    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{

    //    //bulletList.Add(info.photonView.gameObject);
    //    //info.photonView.transform.parent = transform;
    //    info.photonView.gameObject.SetActive(false);
    //}



    public override void OnDisable()
    {
        base.OnDisable();
        if (!PV.IsMine) return;
        CancelInvoke();
    }
    //private void Start()
    //{
    //    if (!PV.IsMine) return;
    //    //  lastRoutine = StartCoroutine(destoyBullet());
    //    //bulletRoutine = StartCoroutine(DeactivateBullet());
    //    Invoke("DeactivateBullet", timeToDeactivateBullet);
    //}

    public void Set(Vector3 velocity, float damage, float destroyTime)
    {
        rb.velocity = velocity;
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

               //gameObject.SetActive(false);
                //StopCoroutine(bulletRoutine);
                PhotonNetwork.Destroy(gameObject);
                //StopCoroutine(lastRoutine);
            }
      }
        else
        {
           // gameObject.SetActive(false);
            //StopCoroutine(bulletRoutine);
            PhotonNetwork.Destroy(gameObject);
            //StopCoroutine(lastRoutine);
        }
    }


    //public IEnumerator DeactivateBullet()
    //{
    //    yield return new WaitForSeconds(timeToDeactivateBullet);
    //    transform.gameObject.SetActive(false);
    //}

    //private void DeactivateBullet()
    //{
    //    gameObject.SetActive(false);
    //}

    //public new GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void Destroy(GameObject gameObject)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public IEnumerator destoyBullet()
    //{
    //    yield return new WaitForSeconds(timeToDestroyBullet);
    //    PhotonNetwork.Destroy(gameObject);
    //}
    public void DestoyBullet()
    { 
        PhotonNetwork.Destroy(gameObject);
    }
}
