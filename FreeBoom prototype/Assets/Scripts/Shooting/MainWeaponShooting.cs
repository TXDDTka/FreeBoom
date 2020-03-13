using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeaponShooting : MonoBehaviourPunCallbacks
{
    //  public enum ReloadingStatus { NoReloadNeeded,NeedReload, Reloading}
    //  public ReloadingStatus reloadingStatus = ReloadingStatus.NoReloadNeeded;
    [SerializeField] private PlayerManager playerManager = null;
    public WeaponData._WeaponName weaponName = WeaponData._WeaponName.None;
    [SerializeField]private bool canShoot = true;
    //private void Awake()
    //{
    //    playerManager = GetComponentInParent<PlayerManager>();
    //}

    //public override void OnEnable()
    //{
    //    //  base.OnEnable();
    //    if (!playerManager.PV.IsMine) return;
    //    playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;
    //    EnableCurrentWeapon();
    //}

    public void Start()
    {
        if (!playerManager.PV.IsMine) return;
        //playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;
        EnableCurrentWeapon();
        //  reloadingCoroutine = ReloadMainWeapon();
    }

    //public void Enable()
    //{
    //    EnableCurrentWeapon();
    //}

    public Vector2 BulletDirection()
    {
        Vector2 direction = new Vector2();
        if (playerManager.shootJoystick.Direction != Vector2.zero)
        {
            direction = playerManager.shootJoystick.Direction;
        }
        else
        {
            //if (!playerManager.spriteRenderer.flipX)
            //    direction = transform.right;
            //else
            //    direction = -transform.right;
            direction = transform.right;
        }

        return direction;
    }

    public void EnableCurrentWeapon()
    {
        // if (!playerManager.PV.IsMine) return;
        playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;
        switch (weaponName)
        {
            case WeaponData._WeaponName.Bazuka:
            playerManager.shootJoystick.OnUpEvent += BazukaShoot;
                break;
            case WeaponData._WeaponName.AutomaticRifle:
                playerManager.shootJoystick.OnUpEvent += AutomaticRifleShoot;
            break;
            case WeaponData._WeaponName.ShotGun:
                playerManager.shootJoystick.OnUpEvent += ShootGunShoot;
                break;
        }
    }



    private void AutomaticRifleShoot()
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(AutomaticRifleShootIEnumerator());
        }
    }

    private IEnumerator AutomaticRifleShootIEnumerator()
    {
        if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount > 0 && !playerManager.playerWeaponManager.mainWeapon.weaponReloading)
        {
            for (int i = 0; i < playerManager.playerWeaponManager.mainWeapon.bulletsPerShoot; i++)
            {

                Bullet bulletGameobject = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.mainWeapon.bulletPrefab.name, playerManager.playerWeaponManager.mainWeaponShootPoint.position, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation).GetComponent<Bullet>();


                bulletGameobject.Set(playerManager.rb, BulletDirection(), playerManager.playerWeaponManager.mainWeapon.bulletSpeed, playerManager.playerWeaponManager.mainWeapon.bulletDamage, playerManager.playerWeaponManager.mainWeapon.shootingDistance);


                yield return new WaitForSeconds(playerManager.playerWeaponManager.mainWeapon.shootDelayPerShoot);

            }

            playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount -= playerManager.playerWeaponManager.mainWeapon.bulletsPerShoot;
            playerManager.changeWeaponBar.ChangeMainWeaponBulletsCount(playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount);

            if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount > 0)
            {
                //playerManager.playerWeaponManager.mainWeapon.needReload = true;
                //StartCoroutine(reloadingCoroutine);
                Reload();
            }
            else if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount == 0)
            {
                playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);
            }
            else
            {
                yield return new WaitForSeconds(playerManager.playerWeaponManager.mainWeapon.shootDelay);
                canShoot = true;
              //  StartCoroutine(ShootDealy());
            }

            
            //yield return new WaitForSeconds(playerManager.playerWeaponManager.mainWeapon.shootDelay);
            //canShoot = true;
        }
    }

    

    private void BazukaShoot()
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(BazukaShootIEnumerator());
        }
    }

    private IEnumerator BazukaShootIEnumerator()
    {

            if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount > 0)// && !playerManager.playerWeaponManager.mainWeapon.weaponReloading)
            {
                Bullet bulletGameobject = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.mainWeapon.bulletPrefab.name, playerManager.playerWeaponManager.mainWeaponShootPoint.position, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation).GetComponent<Bullet>();
                bulletGameobject.Set(playerManager.rb, BulletDirection(), playerManager.playerWeaponManager.mainWeapon.bulletSpeed, playerManager.playerWeaponManager.mainWeapon.bulletDamage, playerManager.playerWeaponManager.mainWeapon.shootingDistance);
                
                playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount -= playerManager.playerWeaponManager.mainWeapon.bulletsPerShoot;
                playerManager.changeWeaponBar.ChangeMainWeaponBulletsCount(playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount);

                if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount > 0)
                {
                // playerManager.playerWeaponManager.mainWeapon.needReload = true;
                
             //   playerManager.playerWeaponManager.mainWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.NeedReload;
                // StartCoroutine(reloadingCoroutine);
                Reload();
                }
                else if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount == 0)
                {
                playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);
                }
                else
                {
                yield return new WaitForSeconds(playerManager.playerWeaponManager.mainWeapon.shootDelay);
                canShoot = true;
                }
             //canShoot = false;
             //yield return new WaitForSeconds(playerManager.playerWeaponManager.mainWeapon.shootDelay);
             //canShoot = true;
        }
    }


    private void ShootGunShoot()
    {
        if(canShoot)
        {
            canShoot = false;
             StartCoroutine(ShootGunShootIEnumerator());
         //   TestShootGun();
        }
        
    }


    private void TestShootGun()
    {
        float test = -10;
        for(int i = 0; i < 3; i ++)
        {

            float x = playerManager.playerWeaponManager.mainWeaponShootPoint.rotation.eulerAngles.x;
            float y = playerManager.playerWeaponManager.mainWeaponShootPoint.rotation.eulerAngles.y;
            float z = playerManager.playerWeaponManager.mainWeaponShootPoint.rotation.eulerAngles.z;

           // playerManager.playerWeaponManager.mainWeaponShootPoint.rotation = Quaternion.Euler(x + test, y + test, z + test);
            Vector3 direction = new Vector3(x + test, y + test, z + test);
            Vector3 dir =  playerManager.playerWeaponManager.mainWeaponShootPoint.TransformDirection(x + 10, y + 10, z - 10);
            Bullet bulletGameobject_1 = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.mainWeapon.bulletPrefab.name, playerManager.playerWeaponManager.mainWeaponShootPoint.position, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation).GetComponent<Bullet>();
            bulletGameobject_1.Set(playerManager.rb, dir, playerManager.playerWeaponManager.mainWeapon.bulletSpeed, playerManager.playerWeaponManager.mainWeapon.bulletDamage, playerManager.playerWeaponManager.mainWeapon.shootingDistance);
            test += 10;
        }
    }

    private IEnumerator ShootGunShootIEnumerator()
    {
        if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount > 0 && !playerManager.playerWeaponManager.mainWeapon.weaponReloading)
        {

            float  x = playerManager.playerWeaponManager.mainWeaponShootPoint.rotation.eulerAngles.x;
            float y = playerManager.playerWeaponManager.mainWeaponShootPoint.rotation.eulerAngles.y;
            float z = playerManager.playerWeaponManager.mainWeaponShootPoint.rotation.eulerAngles.z;

            playerManager.playerWeaponManager.mainWeaponShootPoint.rotation = Quaternion.Euler(x - 10, y - 10, z - 10);
            //playerManager.playerWeaponManager.mainWeaponShootPoint.rotation = Quaternion.RotateTowards(playerManager.playerWeaponManager.mainWeaponShootPoint.rotation, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation, z - 10);
            //Vector3 direction = new Vector3(x - 10, y - 10, z - 10);
            Bullet bulletGameobject_1 = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.mainWeapon.bulletPrefab.name, playerManager.playerWeaponManager.mainWeaponShootPoint.position, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation).GetComponent<Bullet>();
            bulletGameobject_1.Set(playerManager.rb, BulletDirection(), playerManager.playerWeaponManager.mainWeapon.bulletSpeed, playerManager.playerWeaponManager.mainWeapon.bulletDamage, playerManager.playerWeaponManager.mainWeapon.shootingDistance);


             playerManager.playerWeaponManager.mainWeaponShootPoint.rotation = Quaternion.Euler(x + 10, y + 10, z + 10);
          //  playerManager.playerWeaponManager.mainWeaponShootPoint.rotation = Quaternion.RotateTowards(playerManager.playerWeaponManager.mainWeaponShootPoint.rotation, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation, z + 10);
         //   direction = new Vector3(x + 10, y + 10, z + 10);
            Bullet bulletGameobject_2 = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.mainWeapon.bulletPrefab.name, playerManager.playerWeaponManager.mainWeaponShootPoint.position, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation).GetComponent<Bullet>();
            bulletGameobject_2.Set(playerManager.rb, BulletDirection(), playerManager.playerWeaponManager.mainWeapon.bulletSpeed, playerManager.playerWeaponManager.mainWeapon.bulletDamage, playerManager.playerWeaponManager.mainWeapon.shootingDistance);

            playerManager.playerWeaponManager.mainWeaponShootPoint.rotation = Quaternion.Euler(x, y, z);
            //direction = new Vector3(x, y, z);
            Bullet bulletGameobject_3 = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.mainWeapon.bulletPrefab.name, playerManager.playerWeaponManager.mainWeaponShootPoint.position, playerManager.playerWeaponManager.mainWeaponShootPoint.rotation).GetComponent<Bullet>();
            bulletGameobject_3.Set(playerManager.rb, BulletDirection(), playerManager.playerWeaponManager.mainWeapon.bulletSpeed, playerManager.playerWeaponManager.mainWeapon.bulletDamage, playerManager.playerWeaponManager.mainWeapon.shootingDistance);

            playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount -= playerManager.playerWeaponManager.mainWeapon.bulletsPerShoot;

            playerManager.changeWeaponBar.ChangeMainWeaponBulletsCount(playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount);

            if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount > 0)
            {
                //   StartCoroutine(ReloadMainWeapon());
                Reload();
            }
            else if (playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount == 0)
            {
                playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);
            }
            else
            {
                yield return new WaitForSeconds(playerManager.playerWeaponManager.mainWeapon.shootDelay);
                canShoot = true;

            }

            
            
        }
    }

    public void Reload()
    {
        //if (playerManager.playerWeaponManager.mainWeapon.reloadingStatus == PlayerWeaponManager.ReloadingStatus.NeedReload)
        //{
        playerManager.playerWeaponManager.mainWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.Reloading;
            playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);

            playerManager.changeWeaponBar.mainWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.Reloading;
            playerManager.changeWeaponBar.MainWeaponBarReloadingStatus();
            playerManager.changeWeaponBar.ReloadingTime(playerManager.playerWeaponManager.mainWeapon.reloadingTime);

            StartCoroutine(playerManager.changeWeaponBar.MainWeaponCooldownTimer());
            Invoke("ReloadFinished", playerManager.playerWeaponManager.mainWeapon.reloadingTime);
       // }
    }

    public void ReloadFinished()
    {
        if (playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount > playerManager.playerWeaponManager.mainWeapon.bulletsInClip)
        {
            playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount -= playerManager.playerWeaponManager.mainWeapon.bulletsInClip;
            playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount = playerManager.playerWeaponManager.mainWeapon.bulletsInClip;
        }
        else
        {
            playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount = playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount;
            playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount = 0;
        }

        playerManager.changeWeaponBar.mainWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
        playerManager.changeWeaponBar.MainWeaponBarReloadingStatus();
        playerManager.changeWeaponBar.ChangeMainWeaponBulletsCount(playerManager.playerWeaponManager.mainWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.mainWeapon.bulletsMaxCount);

        playerManager.playerWeaponManager.ChangeCrosshairColor(Color.white);
        playerManager.playerWeaponManager.mainWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
        canShoot = true;
    }

    public void ReloadingStop()
    {
        StopCoroutine(playerManager.changeWeaponBar.MainWeaponCooldownTimer());
        playerManager.changeWeaponBar.mainWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NeedReload;
        playerManager.changeWeaponBar.MainWeaponBarReloadingStatus();       

        CancelInvoke();
        playerManager.playerWeaponManager.mainWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.NeedReload;
    }

    public void DisableCurrentWeapon()
    {
        playerManager.shootJoystick.OnBeginDragEvent -= playerManager.crosshairManager.EnableCrosshair;
        switch (weaponName)
        {
            case WeaponData._WeaponName.Bazuka:
                playerManager.shootJoystick.OnUpEvent -= BazukaShoot;
                break;
            case WeaponData._WeaponName.AutomaticRifle:
                playerManager.shootJoystick.OnUpEvent -= AutomaticRifleShoot;
                break;
            case WeaponData._WeaponName.ShotGun:
                playerManager.shootJoystick.OnUpEvent -= ShootGunShoot;
                break;
        }

        //if (playerManager.playerWeaponManager.mainWeapon.weaponReloading)
        //{
        //    Debug.Log("ReloadingStop");
        //    ReloadingStop();
        //}
    }

    //public void Disable()
    //{
    //    Debug.Log("Disable");
    //   // playerManager.shootJoystick.OnBeginDragEvent -= playerManager.crosshairManager.EnableCrosshair;
    //    DisableCurrentWeapon();
    //}

    //public override void OnDisable()
    //{
    //    if (!playerManager.PV.IsMine) return;
    //    base.OnDisable();
    //}
}
