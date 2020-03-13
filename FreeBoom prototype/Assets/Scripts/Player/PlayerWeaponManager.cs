using Photon.Pun;
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerWeaponManager : MonoBehaviour, IPunObservable
{
    [Serializable]
    public class MainWeapon
    {
        [Tooltip("Параметры основного оружия")]
        public GameObject bulletPrefab = null;
        public float bulletSpeed = 0f;
        public float bulletDamage = 0f;
        public int bulletsInClip = 0;
        public int bulletsMaxCount = 0;
        public int bulletsCurrentCount = 0;
        public int bulletsPerShoot = 0;
        public float shootDelay = 0;
        public float shootDelayPerShoot = 0;
        public float shootingDistance = 0;
        public float reloadingTime = 0f;
        public bool weaponChoosen = false;
        public bool weaponReloading = false;
        public bool needReload = false;
        public ReloadingStatus reloadingStatus = ReloadingStatus.NoReloadNeeded;
    }

    [Serializable]
    public class SecondWeapon
    {
        [Tooltip("Параметры дополнительного оружия")]
        public GameObject bulletPrefab = null;
        public float bulletSpeed = 0f;
        public float bulletDamage = 0f;
        public int bulletsInClip = 0;
        public int bulletsMaxCount = 0;
        public int bulletsCurrentCount = 0;
        public int bulletsPerShoot = 0;
        public float shootDelay = 0;
        public float shootDelayPerShoot = 0;
        public float shootingDistance = 0;
        public float reloadingTime = 0f;
        public bool weaponChoosen = false;
        public bool weaponReloading = false;
        public bool needReload = false;
        public ReloadingStatus reloadingStatus = ReloadingStatus.NoReloadNeeded;
    }

    public enum ReloadingStatus { NoReloadNeeded, NeedReload, Reloading }

    public GameObject mainWeaponGameobject = null;
    public Transform mainWeaponShootPoint = null;

    public GameObject secondWeaponGameobject = null;
    public Transform secondWeaponshootPoint = null;

    public Transform weaponHolder = null;
    public MainWeapon mainWeapon = null;
    public SecondWeapon secondWeapon = null;

    [Tooltip("Параметры дополнительного оружия")]
    [Header("WeaponParams")]
    [SerializeField]private WeaponsSettingsDatabase weaponsSettingsDatabase = null;

    public WeaponData._WeaponGroup currentWeapon = WeaponData._WeaponGroup.MainWeapon;

    private PlayerManager playerManager = null;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }



    private void Start()
    {
        if (!playerManager.PV.IsMine) return;
        ChooseWeapon();

        playerManager.changeWeaponBar.mainWeaponButton.onClick.AddListener(() => ChangeWeapon());
        playerManager.changeWeaponBar.secondWeaponButton.onClick.AddListener(() => ChangeWeapon());

     //   playerManager.shootJoystick.OnBeginDragEvent += playerManager.crosshairManager.EnableCrosshair;

    }


    private void Update()
    {
        if (!playerManager.PV.IsMine) return;
        if (playerManager.shootJoystick.HasInput)
        {
            playerManager.crosshairManager.crosshairActive = true;
            if (currentWeapon == WeaponData._WeaponGroup.MainWeapon)
            {
                playerManager.crosshairManager.SetParemeters(mainWeaponShootPoint.position, playerManager.shootJoystick.Direction.normalized, mainWeapon.shootingDistance);
            }
            else if (currentWeapon == WeaponData._WeaponGroup.SecondWeapon)
            {
                playerManager.crosshairManager.SetParemeters(secondWeaponshootPoint.position, playerManager.shootJoystick.Direction.normalized, secondWeapon.shootingDistance);
            }
            RotateGun();
        }
        else if (!playerManager.shootJoystick.HasInput)
        {
            playerManager.crosshairManager.crosshairActive = false;
        }
    }

    private void ChooseWeapon()
    {
            for (int i = 0; i < weaponsSettingsDatabase.weaponsList.Count; i++)
            {
            var weapon = weaponsSettingsDatabase.weaponsList[i];
            if (weapon.WeaponGroup == WeaponData._WeaponGroup.MainWeapon && weapon.CharacterClass == playerManager.player.GetCharacter())
            {
                mainWeapon.weaponChoosen = true;
                mainWeapon.bulletPrefab = weapon.BulletPrefab;
                mainWeapon.bulletSpeed = weapon.BulletSpeed;
                mainWeapon.bulletDamage = weapon.Damage;
                mainWeapon.bulletsInClip = weapon.BulletsCountInClip;
                mainWeapon.bulletsMaxCount = weapon.BulletsMaxCount;
                mainWeapon.bulletsCurrentCount = mainWeapon.bulletsInClip;
                mainWeapon.bulletsPerShoot = weapon.BulletsPerShoot;
                mainWeapon.shootDelay = weapon.ShotDelay;
                mainWeapon.shootDelayPerShoot = weapon.ShootDelayPerShoot;
                mainWeapon.reloadingTime = weapon.ReloadTime;
                mainWeapon.shootingDistance = weapon.ShootingDistance;
                playerManager.crosshairManager.currentCrosshair = weapon.CrosshairType;
                playerManager.crosshairManager.mainWeaponCrosshair = weapon.CrosshairType;

                ChangeCrosshairColor(Color.white);
                playerManager.changeWeaponBar.ChooseMainWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
            }
            else if (weapon.WeaponGroup == WeaponData._WeaponGroup.SecondWeapon)
            {
                secondWeapon.weaponChoosen = true;
                secondWeapon.bulletPrefab = weapon.BulletPrefab;
                secondWeapon.bulletSpeed = weapon.BulletSpeed;
                secondWeapon.bulletDamage = weapon.Damage;
                secondWeapon.bulletsInClip = weapon.BulletsCountInClip;
                secondWeapon.bulletsMaxCount = weapon.BulletsMaxCount;
                secondWeapon.bulletsCurrentCount = secondWeapon.bulletsInClip;
                secondWeapon.bulletsPerShoot = weapon.BulletsPerShoot;
                secondWeapon.shootDelay = weapon.ShotDelay;
                secondWeapon.shootDelayPerShoot = weapon.ShootDelayPerShoot;
                secondWeapon.reloadingTime = weapon.ReloadTime;
                secondWeapon.shootingDistance = weapon.ShootingDistance;
                playerManager.crosshairManager.secondWeaponCrosshair = weapon.CrosshairType;

                playerManager.changeWeaponBar.ChooseSecondWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
            }

            if (mainWeapon.weaponChoosen && secondWeapon.weaponChoosen)
                return;

        }

    }

    public void ChangeWeapon()
    {
        playerManager.changeWeaponBar.ChangeWeapon();
        if (currentWeapon == WeaponData._WeaponGroup.MainWeapon)
        {
            currentWeapon = WeaponData._WeaponGroup.SecondWeapon;
            playerManager.crosshairManager.currentCrosshair = playerManager.crosshairManager.secondWeaponCrosshair;

            if (mainWeapon.reloadingStatus == ReloadingStatus.Reloading)
            {
                playerManager.mainWeaponShooting.ReloadingStop();
            }

            playerManager.mainWeaponShooting.DisableCurrentWeapon();

            mainWeaponGameobject.SetActive(false);
            secondWeaponGameobject.SetActive(true);

            playerManager.PV.RPC("RPC_ChangeWeaponForOthers", RpcTarget.OthersBuffered, false, true);

            //playerManager.PV.RPC("RPC_ChangeWeapon", RpcTarget.MasterClient, false, true);
            //if (playerManager.PV.Owner.IsMasterClient)
            //{
            //    playerManager.PV.RPC("RPC_ChangeWeaponForOthers", RpcTarget.OthersBuffered, false, true);
            //}
            //else
            //{
            //    Debug.Log("Не хост 1");
            //    playerManager.PV.RPC("RPC_ChangeWeapon", RpcTarget.MasterClient, false, true);
            //}
            playerManager.secondWeaponShooting.EnableCurrentWeapon();

            if (secondWeapon.reloadingStatus == ReloadingStatus.NeedReload)
            {
                playerManager.secondWeaponShooting.Reload();
            }
            else if (secondWeapon.bulletsCurrentCount == 0 && secondWeapon.bulletsMaxCount == 0)
            {
                ChangeCrosshairColor(Color.red);
            }
            else
            {
                ChangeCrosshairColor(Color.white);
            }



        }
        else if (currentWeapon == WeaponData._WeaponGroup.SecondWeapon)
        {
            currentWeapon = WeaponData._WeaponGroup.MainWeapon;
            playerManager.crosshairManager.currentCrosshair = playerManager.crosshairManager.mainWeaponCrosshair;

            if (secondWeapon.reloadingStatus == ReloadingStatus.Reloading)
            {
                playerManager.secondWeaponShooting.ReloadingStop();
            }

            playerManager.secondWeaponShooting.DisableCurrentWeapon();

            mainWeaponGameobject.SetActive(true);
            secondWeaponGameobject.SetActive(false);

            playerManager.PV.RPC("RPC_ChangeWeaponForOthers", RpcTarget.OthersBuffered, true, false);

            //if (playerManager.PV.Owner.IsMasterClient)
            //{
            //    playerManager.PV.RPC("RPC_ChangeWeaponForOthers", RpcTarget.OthersBuffered, true, false);
            //}
            //else
            //{
            //    Debug.Log("Не хост 2");
            //    playerManager.PV.RPC("RPC_ChangeWeapon", RpcTarget.MasterClient, true, false);
            //}

            playerManager.mainWeaponShooting.EnableCurrentWeapon();

            if (mainWeapon.reloadingStatus == ReloadingStatus.NeedReload)
            {
                playerManager.mainWeaponShooting.Reload();
            }
            else if (mainWeapon.bulletsCurrentCount == 0 && mainWeapon.bulletsMaxCount == 0)
            {
                ChangeCrosshairColor(Color.red);
            }
            else
            {
                ChangeCrosshairColor(Color.white);
            }


        }

    }

    //[PunRPC]
    //public void RPC_ChangeWeapon(bool mainWeaponObject, bool secondWeaponObject)
    //{
    //    mainWeaponGameobject.SetActive(mainWeaponObject);
    //    secondWeaponGameobject.SetActive(secondWeaponObject);
    //    playerManager.PV.RPC("RPC_ChangeWeaponForOthers", RpcTarget.OthersBuffered, mainWeaponObject, secondWeaponObject);
    //}


    [PunRPC]
    public void RPC_ChangeWeaponForOthers(bool mainWeaponObject, bool secondWeaponObject)
    {
        mainWeaponGameobject.SetActive(mainWeaponObject);
        secondWeaponGameobject.SetActive(secondWeaponObject);
    }

    private void RotateGun()
    {
        float rotationAngle = Mathf.Atan2(playerManager.shootJoystick.Vertical, playerManager.shootJoystick.Horizontal) * Mathf.Rad2Deg;

        weaponHolder.eulerAngles = new Vector3(0, 0, rotationAngle);

        Vector3 localScale = Vector3.one;
        if (rotationAngle > 90 || rotationAngle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = +1f;
        }

        weaponHolder.localScale = localScale;
    }

    public void ChangeCrosshairColor(Color color)
    {
        playerManager.crosshairManager.ChangeColor(color);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(weaponHolder.eulerAngles);
            stream.SendNext(weaponHolder.localScale);
        }
        else
        {
            weaponHolder.eulerAngles = (Vector3)stream.ReceiveNext();
            weaponHolder.localScale = (Vector3)stream.ReceiveNext();
        }
    }
}
