using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhotonPlayerShooting : MonoBehaviourPunCallbacks//PlayerController
{

    public enum Weapon {None,Bazuka,Pistol}
    public Weapon weapon = Weapon.None;

    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private float bulletSpeed = 0;
    [SerializeField] private float bulletDamage = 0;

    [SerializeField] private Transform shootPoint = null;

    public WeaponSettingsDatabase weaponSettingsDatabase;
    private PhotonPlayerMovement photonPlayerMovement;
    private PhotonView PV = null;
    private Boom boom;

    public ShootJoystick shootJoystick = null;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonPlayerMovement = GetComponent<PhotonPlayerMovement>();
        boom = Boom.Instance;
        shootJoystick = ShootJoystick.Instance;

        if (PV.IsMine)
        {
            CheckWeapon();
            shootJoystick.OnUpEvent += Shoot;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (!PV.IsMine) return;
        shootJoystick.OnUpEvent -= Shoot;
    }

    private void CheckWeapon()
    {

        switch (weapon)
        {
            case Weapon.Bazuka:
                bulletSpeed = weaponSettingsDatabase[0].Speed;
                bulletDamage = weaponSettingsDatabase[0].Damage;
                break;
        }
    }

    private void Shoot()
    {
        if (photonPlayerMovement.canMove)
        {
            Bullet bulletGameobject = PhotonNetwork.Instantiate(bulletPrefab.name, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();//Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                                                                                                                                                    //bulletGameobject.GetComponent<Bullet>().Set(shootPoint.forward * bulletSpeed, bulletDamage);
            bulletGameobject.Set(shootPoint.forward * bulletSpeed, bulletDamage);
      }

    }
}
