using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondWeaponShooting : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerManager playerManager = null;
    public WeaponData._WeaponName weaponName = WeaponData._WeaponName.None;
    [SerializeField] private bool canShoot = true;
    //private void Awake()
    //{
    //    playerManager = GetComponentInParent<PlayerManager>();
    //}

    //public override void OnEnable()
    //{
    //    base.OnEnable();
    //    playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;

    //    EnableCurrentWeapon();
    //}

    //private void Start()
    //{
    //    if (!playerManager.PV.IsMine) return;
    //    playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;
    //    EnableCurrentWeapon();
    //}

    //private void Start()
    //{
    //    if (!playerManager.PV.IsMine) return;
    //    EnableCurrentWeapon();
    //}


    public void EnableCurrentWeapon()
    {
        playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;
        switch (weaponName)
        {
            case WeaponData._WeaponName.USP:
                playerManager.shootJoystick.OnUpEvent += USPShoot;
                break;
        }
    }

    private void USPShoot()
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(USPShootIEnumerator());
        }
    }

    private IEnumerator USPShootIEnumerator()
    {
        if (playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount > 0)// && !playerManager.playerWeaponManager.secondWeapon.weaponReloading)
        {
            Bullet bulletGameobject = PhotonNetwork.Instantiate(playerManager.playerWeaponManager.secondWeapon.bulletPrefab.name, playerManager.playerWeaponManager.secondWeaponshootPoint.position, playerManager.playerWeaponManager.secondWeaponshootPoint.rotation).GetComponent<Bullet>();
            bulletGameobject.Set(playerManager.rb, BulletDirection(), playerManager.playerWeaponManager.secondWeapon.bulletSpeed, playerManager.playerWeaponManager.secondWeapon.bulletDamage, playerManager.playerWeaponManager.secondWeapon.shootingDistance);
            playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount -= 1;

            playerManager.changeWeaponBar.ChangeSecondWeaponBulletsCount(playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount);

            if (playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount > 0)
            {
                Reload();
            }
            else if (playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount == 0 && playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount == 0)
            {
                playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);
            }
            else
            {
                yield return new WaitForSeconds(playerManager.playerWeaponManager.secondWeapon.shootDelay);
                canShoot = true;
            }
        }

    }

    public Vector2 BulletDirection()
    {
        Vector2 direction = new Vector2();
        if (playerManager.shootJoystick.Direction != Vector2.zero)
        {
            direction = playerManager.shootJoystick.Direction;
        }
        else
        {
            if (!playerManager.spriteRenderer.flipX)
                direction = transform.right;
            else
                direction = -transform.right;
        }

        return direction;
    }

    public IEnumerator ReloadSecondWeapon()
    {
        if (!playerManager.playerWeaponManager.secondWeapon.weaponReloading)
        {
            playerManager.playerWeaponManager.secondWeapon.weaponReloading = true;
            playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);
            playerManager.changeWeaponBar.secondWeaponCooldown = true;

            yield return new WaitForSeconds(playerManager.playerWeaponManager.secondWeapon.reloadingTime);


            if (playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount > playerManager.playerWeaponManager.secondWeapon.bulletsInClip)
            {
                playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount -= playerManager.playerWeaponManager.secondWeapon.bulletsInClip;
                playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount = playerManager.playerWeaponManager.secondWeapon.bulletsInClip;
            }
            else
            {
                playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount = playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount;
                playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount = 0;
            }

            playerManager.changeWeaponBar.secondWeaponCooldown = false;
          //  playerManager.changeWeaponBar.SecondWeaponCooldownBar(false, 0);
            playerManager.changeWeaponBar.ChangeSecondWeaponBulletsCount(playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount);
            playerManager.playerWeaponManager.ChangeCrosshairColor(Color.white);
            playerManager.playerWeaponManager.secondWeapon.weaponReloading = false;
        }
    }


    

    public void Reload()
    {
        //if (playerManager.playerWeaponManager.secondWeapon.reloadingStatus == PlayerWeaponManager.ReloadingStatus.NeedReload)
        //{
            playerManager.playerWeaponManager.secondWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.Reloading;
            playerManager.playerWeaponManager.ChangeCrosshairColor(Color.red);

            playerManager.changeWeaponBar.secondWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.Reloading;
            playerManager.changeWeaponBar.SecondWeaponBarReloadingStatus();
            playerManager.changeWeaponBar.ReloadingTime(playerManager.playerWeaponManager.secondWeapon.reloadingTime);

            StartCoroutine(playerManager.changeWeaponBar.SecondWeaponCooldownTimer());
            Invoke("ReloadFinished", playerManager.playerWeaponManager.secondWeapon.reloadingTime);
        //}
    }

    public void ReloadFinished()
    {
        if (playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount > playerManager.playerWeaponManager.secondWeapon.bulletsInClip)
        {
            playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount -= playerManager.playerWeaponManager.secondWeapon.bulletsInClip;
            playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount = playerManager.playerWeaponManager.secondWeapon.bulletsInClip;
        }
        else
        {
            playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount = playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount;
            playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount = 0;
        }

        playerManager.changeWeaponBar.secondWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
        playerManager.changeWeaponBar.SecondWeaponBarReloadingStatus();
        playerManager.changeWeaponBar.ChangeSecondWeaponBulletsCount(playerManager.playerWeaponManager.secondWeapon.bulletsCurrentCount, playerManager.playerWeaponManager.secondWeapon.bulletsMaxCount);

        playerManager.playerWeaponManager.ChangeCrosshairColor(Color.white);
        playerManager.playerWeaponManager.secondWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
        canShoot = true;
    }

    public void ReloadingStop()
    {

        StopCoroutine(playerManager.changeWeaponBar.SecondWeaponCooldownTimer());
        playerManager.changeWeaponBar.secondWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NeedReload;
        playerManager.changeWeaponBar.SecondWeaponBarReloadingStatus();

        CancelInvoke();
        playerManager.playerWeaponManager.secondWeapon.reloadingStatus = PlayerWeaponManager.ReloadingStatus.NeedReload;
    }

    public void DisableCurrentWeapon()
    {
        playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;
        switch (weaponName)
        {
            case WeaponData._WeaponName.USP:
                playerManager.shootJoystick.OnUpEvent -= USPShoot;
                break;
        }
    }
}
