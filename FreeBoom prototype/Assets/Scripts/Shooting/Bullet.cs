using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField]private PhotonView _PV;
    [SerializeField]private Rigidbody2D _rb = null;
    private float _damage = 0f;
    private float _distance = 0f;
    private float _speed = 0f;
    private Vector3 _direction;
    private Vector3 _spawnPosition;
    private Vector3 _playerVelocity;


    public void Set(Rigidbody2D playerVelocity, Vector2 direction,float speed, float damage, float distance)
    {
        _direction = direction;
        _speed = speed;
        _distance = distance;
        _damage = damage;
        _spawnPosition = transform.position;
        _playerVelocity = playerVelocity.velocity;
    }

    private void FixedUpdate()
    {
        if (!_PV.IsMine) return;
        //_rb.velocity = _playerVelocity + _direction * _speed;
        _rb.velocity = _playerVelocity + transform.TransformDirection(new Vector3(_speed, 0, 0));
        // _rb.velocity = transform.TransformDirection(_direction * _speed);
        //  _rb.velocity = transform.TransformDirection(new Vector3(_speed, 0, 0));
        // _rb.AddForce(_direction * _speed,ForceMode2D.Impulse);

        float maxDistance = Vector2.Distance(_spawnPosition, transform.position);
        if (maxDistance > _distance)
        {
             PhotonNetwork.Destroy(gameObject);

        }

    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (!_PV.IsMine) return;


    //    if (other.gameObject.tag == "Player")
    //    {
    //        if (other.gameObject.GetComponent<PhotonView>().Owner.GetTeam() != PhotonNetwork.LocalPlayer.GetTeam())
    //        {

    //            other.gameObject.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllViaServer, _damageAmount, PhotonNetwork.LocalPlayer);
    //            PhotonNetwork.Destroy(gameObject);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("else");
    //        PhotonNetwork.Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.TryGetComponent<PhotonView>(out PhotonView photonView))
        {
        //if (collision.tag == "Player")
        //{
        //    PhotonView photonView = collision.GetComponent<PhotonView>();

            if (photonView.Owner.GetTeam() != PhotonNetwork.LocalPlayer.GetTeam() && _PV.IsMine)
            {

                photonView.RPC("GetDamage", RpcTarget.AllViaServer, _damage, PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else if (collision.TryGetComponent<BankHealthHandler>(out BankHealthHandler bankHandler))
        //  else if(collision.tag == "BankDoor")
        {
           // BankHealthHandler bankHandler = collision.GetComponent<BankHealthHandler>();
            if (bankHandler.bankTeam != PhotonNetwork.LocalPlayer.GetTeam() && _PV.IsMine)
            {
                //_PV.RPC("RPC_HandleDamage", RpcTarget.AllBuffered, _damage, collision.GetComponent<PhotonView>());
                bankHandler._PV.RPC("RPC_HandleDamage", RpcTarget.AllBufferedViaServer, _damage);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            if(_PV.IsMine)
            PhotonNetwork.Destroy(gameObject);
        }
    }



    //[PunRPC]
    //public void RPC_HandleDamageMasterClient(float damage, PhotonView bankHandler)
    //{
    //    bankHandler.RPC("RPC_HandleDamage", RpcTarget.AllBuffered, damage);
    //}

    //[PunRPC]
    //public void RPC_HandleDamage(float damage, BankHandler bankHandler)
    //{
    //    bankHandler.HandleDamage(damage);
    //}
}
