using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponPhoton : MonoBehaviour, IPunObservable
{
    private Transform shootPoint;
    private GameObject muzzle;
    private float rotationAngle;

    private SpriteRenderer sr;
    public ShootJoystick shootJoystick;
    private MoveJoystick moveJoystick;

    private WeaponSwitching wp = null;
    private PhotonView photonview;

    public int bulletDamage;
    public float bulletSpeed;

    //GameObject bullet;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        shootPoint = transform.GetChild(0);
        muzzle = transform.GetChild(1).gameObject;
        muzzle.SetActive(false);
        photonview = GetComponent<PhotonView>();
       // shootJoystick = ShootJoystick.Instance;
       // wp = FindObjectOfType<WeaponSwitchingPhoton>();
    }

    private void OnEnable()
    {


        if (!photonview.IsMine) return;
        if (shootJoystick == null) shootJoystick = ShootJoystick.Instance;// FindObjectOfType<ShootJoystick>();

        shootJoystick.OnUpEvent += Fire;

        if (wp == null) wp = FindObjectOfType<WeaponSwitching>();





        // if (wp == null) wp = WeaponSwitchingPhoton.Instance;// FindObjectOfType<WeaponSwitchingPhoton>();

        //    if (moveJoystick == null)
        //moveJoystick = FindObjectOfType<MoveJoystick>();
    }

    private void OnDisable()
    {
        if (!photonview.IsMine) return;
        transform.rotation = Quaternion.identity;
        sr.flipY = false;

        shootJoystick.OnUpEvent -= Fire;

    }

    private void Update()
    {
        if (!photonview.IsMine) return;

        if (shootJoystick.HasInput)
           RotateGun(); 
    }

 
    private void RotateGun()
    {

            rotationAngle = Mathf.Atan2(shootJoystick.Vertical, shootJoystick.Horizontal) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

            if (rotationAngle < 0) 
                rotationAngle += 360;
            else if (rotationAngle > 360) 
                rotationAngle -= 360;

            if (rotationAngle > 90 && rotationAngle < 270)
                sr.flipY = true;
            else
            sr.flipY = false;
    }




    private void Fire()
    {
        GameObject bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BulletPhoton"),
        shootPoint.position, shootPoint.rotation);
        BulletPhoton b = bullet.GetComponent<BulletPhoton>();
        b.Initialize("team_1", sr.flipY, bulletDamage, transform.right * bulletSpeed);
        print("Выстрелил:" + PhotonNetwork.LocalPlayer.NickName);
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

    //private void Fire()
    //{
    //    //StartCoroutine(FireRoutine());
    //   // photonview.RPC("RPC_Fire", RpcTarget.AllBuffered);
    //    StartCoroutine(FireRoutine());
    //}

    //  [PunRPC]
    //private void RPC_Fire()
    //{
    //    StartCoroutine(FireRoutine());
    //}

    // private IEnumerator FireRoutine()
    //private void Fire()
    //{
    //    //Debug.Log($"fired from {transform.name}");
    //    //muzzle.SetActive(true);
    //    //yield return new WaitForEndOfFrame();
    //    //muzzle.SetActive(false);

    //     bullet = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BulletPhoton"),
    //     shootPoint.position, shootPoint.rotation);
    //    //GameObject go = Instantiate(bullet,shootPoint.position, shootPoint.rotation);
    //    //if (rotationAngle > 90 && rotationAngle < 270)
    //    //  bullet.GetComponent<SpriteRenderer>().flipY = true;
    //    bullet.GetComponent<BulletPhoton>().bulletTeam = GetComponentInParent<PlayerControllerPhoton>().team;
    //    bullet.GetComponent<Rigidbody2D>().velocity = transform.right * wp.BulletSpeed;
    //    print("Выстрелил:" + PhotonNetwork.LocalPlayer.NickName);
    //   // yield return new WaitForSeconds(5);
    //    // PhotonNetwork.Destroy(bullet);
    //     //Destroy(bullet.gameObject,5);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawLine(transform.position, transform.right * 5);
    //}
}
