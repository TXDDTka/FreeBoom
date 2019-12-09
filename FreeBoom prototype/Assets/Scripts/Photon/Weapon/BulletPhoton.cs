using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhoton : MonoBehaviour, IPunObservable
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public int bulletDamage;
    public string bulletTeam;
    // Use this for initialization

    void Awake () {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(string team, bool facingRight, int damage, Vector3 velocity) 
    {
        bulletTeam = team;
        sr.flipY = facingRight;
        bulletDamage = damage;
        rb.velocity = velocity;
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            //GameObject hit = coll.gameObject;
            // PlayerControllerPhoton.Instance.GetDamage(bulletDamage);
            coll.SendMessage("GetDamage", bulletDamage, SendMessageOptions.RequireReceiver);
        }
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Мы владелец этого игрока: отправляем другим наши данные
            stream.SendNext(sr.flipY);
        }
        else
        {
            // Не наш игрок,мы получаем данные от другого игрока
            sr.flipY = (bool)stream.ReceiveNext();
        }
    }

    //public void OnTriggerEnter2D(Collider2D coll)
    //{

    //    if (coll.tag == "Player" && bulletTeam == "team_2")
    //    {
    //        print("Вариант 1");
    //        if (coll.GetComponent<PlayerControllerPhoton>().team == "team_1")
    //        {
    //            coll.PlayerControllerPhoton.playerHealth -= bulletDamage;
    //            print("Попала в игрока из команды team1");
    //        }
    //    }
    //    else if (coll.tag == "Player" && bulletTeam == "team_1") 
    //        {
    //        print("Вариант 2");
    //            if (coll.GetComponent<PlayerControllerPhoton>().team == "team_2")
    //            {
    //                coll.GetComponent<PlayerControllerPhoton>().playerHealth -= bulletDamage;
    //                print("Попала в игрока из команды team2");
    //            }
    //            else if (coll.GetComponent<PlayerControllerPhoton>().team == "team_1")
    //        {
    //            coll.GetComponent<PlayerControllerPhoton>().playerHealth -= bulletDamage;
    //            print("Попала в своего");
    //        }
    //        {

    //        }
    //        }

    //else
    //{
    //    if (coll.tag == "Player" && bulletTeam == "team_1")
    //    {
    //        print("Вариант 3");
    //        if (coll.GetComponent<PlayerControllerPhoton>().team == "team_1")
    //        {
    //            coll.GetComponent<PlayerControllerPhoton>().playerHealth -= bulletDamage;
    //            print("Попала в своего");
    //        }
    //    }
    //}
}



    //public void Flip(bool changeFlip)
    //{
    //  //  sr.flipY = changeFlip;
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // Мы владелец этого игрока: отправляем другим наши данные
    //       // stream.SendNext(sr.flipY);
    //        // stream.SendNext(sr.flipX);
    //    }
    //    else
    //    {
    //        // Не наш игрок,мы получаем данные от другого игрока
    //       // sr.flipY = (bool)stream.ReceiveNext();
    //        //  sr.flipX = (bool)stream.ReceiveNext();
    //    }
    //}



