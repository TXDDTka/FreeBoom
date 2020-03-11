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

    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    _PV = GetComponent<PhotonView>();
    //    _rb = GetComponent<Rigidbody2D>();
    //    _spawnPosition = transform.position;
    //}

    //private void Awake()
    //{
    //    _spawnPosition = transform.position;
    //}
    //public override void OnDisable()
    //{
    //    base.OnDisable();
    //    if (!_PV.IsMine) return;
    //    CancelInvoke();
    //}

    public void Set(Vector2 direction,float speed, float damage, float distance)
    {
        _direction = direction;
        _speed = speed;
        _distance = distance;
        _damageAmount = damage;
        _spawnPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!_PV.IsMine) return;
         _rb.velocity = _direction * _speed;
        // _rb.velocity = transform.TransformDirection(_direction * _speed);
        _rb.velocity = transform.TransformDirection(new Vector3(_speed, 0, 0));
        // _rb.AddForce(_direction * _speed,ForceMode2D.Impulse);

        float maxDistance = Vector2.Distance(_spawnPosition, transform.position);
        if (maxDistance > _distance)
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!_PV.IsMine) return;


        if (other.gameObject.tag == "Player")
        { 
            if (other.gameObject.GetComponent<PhotonView>().Owner.GetTeam() != PhotonNetwork.LocalPlayer.GetTeam())
            {

                other.gameObject.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.AllViaServer, _damageAmount, PhotonNetwork.LocalPlayer);
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("else");
            PhotonNetwork.Destroy(gameObject);
        }
    }

    //public void DestoyBullet()
    //{ 
    //    PhotonNetwork.Destroy(gameObject);
    //}
}
