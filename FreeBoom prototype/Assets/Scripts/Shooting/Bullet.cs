using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField]private PhotonView _PV;
    [SerializeField]private Rigidbody2D _rb = null;
    private float _damageAmount = 0f;
    private float _distance = 0f;
    private float _speed = 0f;
    private Vector3 _direction;
    private Vector3 _spawnPosition;


    public override void OnDisable()
    {
        _direction = direction;
        _speed = speed;
        _distance = distance;
        _damageAmount = damage;
        _spawnPosition = transform.position;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_PV.IsMine) return;


        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.GetComponent<PhotonView>().Owner.GetTeam() != PhotonNetwork.LocalPlayer.GetTeam())
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
