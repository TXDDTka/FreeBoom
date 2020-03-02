using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerManager))]
public class PlayerShooting : MonoBehaviour
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
        public float shootingDistance = 0;
        public float reloadingTime = 0f;
        public GameObject weaponGameobject = null;
        public Transform shootPoint = null;
        public bool weaponChoosen = false;
        public bool weaponReloading = false;
        public IEnumerator enumeratorWeaponReloading;
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
        public float shootingDistance = 0;
        public float reloadingTime = 0f;
        public GameObject weaponGameobject = null;
        public Transform shootPoint = null;
        public bool weaponChoosen = false;
        public bool weaponReloading = false;
        public IEnumerator enumeratorWeaponReloading;
    }


    public MainWeapon mainWeapon = null;
    public SecondWeapon secondWeapon = null;

    [Tooltip("Параметры дополнительного оружия")]
    [Header("WeaponParams")]
    public WeaponsSettingsDatabase weaponsSettingsDatabase = null;

    public enum CurrentWeapon { MainWeapon, SecondWeapon, Tool}
    public CurrentWeapon currentWeapon = CurrentWeapon.MainWeapon;


    private PlayerManager playerManager = null;
    private ChangeWeaponBar changeWeaponBar = null;
    private CrosshairManager crosshairManager = null;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        changeWeaponBar = ChangeWeaponBar.Instance;
        crosshairManager = CrosshairManager.Instance;
    }



    private void Start()
    {
        if (!playerManager.PV.IsMine) return;
            ChooseWeapon();

        changeWeaponBar.mainWeaponButton.onClick.AddListener(() => ChangeWeapon());
        changeWeaponBar.secondWeaponButton.onClick.AddListener(() => ChangeWeapon());



        playerManager.shootJoystick.OnBeginDragEvent += crosshairManager.EnableCrosshair;
        playerManager.shootJoystick.OnUpEvent += Shoot;   

    }

    private void Update()
    {
        if (!playerManager.PV.IsMine) return;
        if (playerManager.shootJoystick.HasInput)
        {
            crosshairManager.crosshairActive = true;
            if (currentWeapon == CurrentWeapon.MainWeapon)
            {
                crosshairManager.SetParemeters(playerManager.playerShooting.mainWeapon.shootPoint.position, playerManager.shootJoystick.Direction.normalized, playerManager.playerShooting.mainWeapon.shootingDistance);
            }
            else if(currentWeapon == CurrentWeapon.SecondWeapon)
            {
                crosshairManager.SetParemeters(playerManager.playerShooting.secondWeapon.shootPoint.position, playerManager.shootJoystick.Direction.normalized, playerManager.playerShooting.secondWeapon.shootingDistance);
            }

        }
        else if(!playerManager.shootJoystick.HasInput)
        {
            crosshairManager.crosshairActive = false;
        }
    }

    private void ChooseWeapon()
    {

        for (int i = 0; i < weaponsSettingsDatabase.weaponsList.Count; i++)
        {
            var weapon = weaponsSettingsDatabase.weaponsList[i];
            if (weapon.WeaponGroup == WeaponData._WeaponGroup.MainWeapon && weapon.CharacterClass.ToString() == PhotonNetwork.LocalPlayer.GetCharacter().ToString())
            {
                mainWeapon.weaponChoosen = true;
                mainWeapon.bulletPrefab = weapon.BulletPrefab;
                mainWeapon.bulletSpeed = weapon.BulletSpeed;
                mainWeapon.bulletDamage = weapon.Damage;
                mainWeapon.bulletsInClip = weapon.BulletsCountInClip;
                mainWeapon.bulletsMaxCount = weapon.BulletsMaxCount;
                mainWeapon.bulletsCurrentCount = mainWeapon.bulletsInClip;
                mainWeapon.reloadingTime = weapon.ReloadTime;
                mainWeapon.shootingDistance =  weapon.ShootingDistance;
                crosshairManager.currentCrosshair = (CrosshairManager.CrosshairType)(byte)weapon.CrosshairType;
                crosshairManager.mainWeaponCrosshair = (CrosshairManager.CrosshairType)(byte)weapon.CrosshairType;
                
                ChangeCrosshairColor(Color.white);
                mainWeapon.enumeratorWeaponReloading = ReloadMainWeapon();
                changeWeaponBar.ChooseMainWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
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
                secondWeapon.reloadingTime = weapon.ReloadTime;
                secondWeapon.shootingDistance = weapon.ShootingDistance;
                crosshairManager.secondWeaponCrosshair = (CrosshairManager.CrosshairType)(byte)weapon.CrosshairType;

                secondWeapon.enumeratorWeaponReloading = ReloadSecondWeapon();
                changeWeaponBar.ChooseSecondWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
            }

            if (mainWeapon.weaponChoosen && secondWeapon.weaponChoosen)
                return;

        }
    }

    private void ChangeWeapon()
    {
        changeWeaponBar.ChangeWeapon();
        if(currentWeapon == CurrentWeapon.MainWeapon)
        {
            currentWeapon = CurrentWeapon.SecondWeapon;
            crosshairManager.currentCrosshair = crosshairManager.secondWeaponCrosshair;
            if (mainWeapon.weaponReloading)
            {
                changeWeaponBar.MainWeaponCooldownBar(false, 0);
                StopAllCoroutines();
                mainWeapon.weaponReloading = false;;
            }

            if (secondWeapon.bulletsCurrentCount == 0 && secondWeapon.bulletsMaxCount > 0)
            {
                StartCoroutine(ReloadSecondWeapon());
            }
            else if (secondWeapon.bulletsCurrentCount == 0 && secondWeapon.bulletsMaxCount == 0)
            {
                ChangeCrosshairColor(Color.red);
            }
            else
            {
                ChangeCrosshairColor(Color.white);
            }


            playerManager.PV.RPC("ChangeWeaponForOthers", RpcTarget.AllBufferedViaServer, false, true);
        }
        else if(currentWeapon == CurrentWeapon.SecondWeapon)
        {
            currentWeapon = CurrentWeapon.MainWeapon;
            crosshairManager.currentCrosshair = crosshairManager.mainWeaponCrosshair;

            if (secondWeapon.weaponReloading)
            {
                changeWeaponBar.SecondWeaponCooldownBar(false, 0);
                StopAllCoroutines();
                secondWeapon.weaponReloading = false;
            }


            if (mainWeapon.bulletsCurrentCount == 0 && mainWeapon.bulletsMaxCount > 0)
            {
                StartCoroutine(ReloadMainWeapon());
            }
           else if (mainWeapon.bulletsCurrentCount == 0 && mainWeapon.bulletsMaxCount == 0)
            {
                ChangeCrosshairColor(Color.red);
            }
            else
            {
                ChangeCrosshairColor(Color.white);
            }


            playerManager.PV.RPC("ChangeWeaponForOthers", RpcTarget.AllBufferedViaServer, true, false);
        }        

    }
    
    [PunRPC]
    public void ChangeWeaponForOthers(bool mainWeaponObject, bool secondWeaponObject)
    {
        mainWeapon.weaponGameobject.SetActive(mainWeaponObject);
        secondWeapon.weaponGameobject.SetActive(secondWeaponObject);
    }





    private void Shoot()
    {
        if (playerManager.playerMovement.canMove)
        {
            if (currentWeapon == CurrentWeapon.MainWeapon)
            {
                if (mainWeapon.bulletsCurrentCount > 0 && !mainWeapon.weaponReloading)
                {

                    Bullet bulletGameobject = PhotonNetwork.Instantiate(mainWeapon.bulletPrefab.name, mainWeapon.shootPoint.position, mainWeapon.shootPoint.rotation).GetComponent<Bullet>();
                    bulletGameobject.Set(mainWeapon.shootPoint.position, mainWeapon.shootingDistance, mainWeapon.bulletSpeed, mainWeapon.bulletDamage);
                    mainWeapon.bulletsCurrentCount -= 1;

                    changeWeaponBar.ChangeMainWeaponBulletsCount(mainWeapon.bulletsCurrentCount, mainWeapon.bulletsMaxCount);

                    if (mainWeapon.bulletsCurrentCount == 0 && mainWeapon.bulletsMaxCount > 0)
                    {
                        StartCoroutine(ReloadMainWeapon());
                    }
                    else if (mainWeapon.bulletsCurrentCount == 0 && mainWeapon.bulletsMaxCount == 0)
                    {
                        ChangeCrosshairColor(Color.red);
                    }
                }
            }
            else if (currentWeapon == CurrentWeapon.SecondWeapon)
            {
                if (secondWeapon.bulletsCurrentCount > 0 && !secondWeapon.weaponReloading)
                {
                    Bullet bulletGameobject = PhotonNetwork.Instantiate(secondWeapon.bulletPrefab.name, secondWeapon.shootPoint.position, secondWeapon.shootPoint.rotation).GetComponent<Bullet>();
                    bulletGameobject.Set(secondWeapon.shootPoint.position, secondWeapon.shootingDistance, secondWeapon.bulletSpeed, secondWeapon.bulletDamage);
                    secondWeapon.bulletsCurrentCount -= 1;

                    changeWeaponBar.ChangeSeconWeaponBulletsCount(secondWeapon.bulletsCurrentCount, secondWeapon.bulletsMaxCount);

                    if (secondWeapon.bulletsCurrentCount == 0 && secondWeapon.bulletsMaxCount > 0)
                    {
                        StartCoroutine(ReloadSecondWeapon());
                    }
                }
                else if (secondWeapon.bulletsCurrentCount == 0 && secondWeapon.bulletsMaxCount == 0)
                {
                    ChangeCrosshairColor(Color.red);
                }

            }
            }

        }
   
   

    private IEnumerator ReloadMainWeapon()
    {
        if (!mainWeapon.weaponReloading)
        {
            mainWeapon.weaponReloading = true;
            ChangeCrosshairColor(Color.red);
            changeWeaponBar.mainWeaponCooldown = true;
            changeWeaponBar.MainWeaponCooldownBar(true, mainWeapon.reloadingTime);
            yield return new WaitForSeconds(mainWeapon.reloadingTime);

            if (mainWeapon.bulletsMaxCount > mainWeapon.bulletsInClip)
            {

                mainWeapon.bulletsMaxCount -= mainWeapon.bulletsInClip;
                mainWeapon.bulletsCurrentCount = mainWeapon.bulletsInClip;
            }
            else
            {
                mainWeapon.bulletsCurrentCount = mainWeapon.bulletsMaxCount;
                mainWeapon.bulletsMaxCount = 0;
            }

            changeWeaponBar.mainWeaponCooldown = false;
            changeWeaponBar.MainWeaponCooldownBar(false, 0);
            changeWeaponBar.ChangeMainWeaponBulletsCount(mainWeapon.bulletsCurrentCount, mainWeapon.bulletsMaxCount);
            ChangeCrosshairColor(Color.white);
            mainWeapon.weaponReloading = false;
        }
    }

    private IEnumerator ReloadSecondWeapon()
    {
        if (!secondWeapon.weaponReloading)
        {
            secondWeapon.weaponReloading = true;
            ChangeCrosshairColor(Color.red);
            changeWeaponBar.secondWeaponCooldown = true;
            changeWeaponBar.SecondWeaponCooldownBar(true, secondWeapon.reloadingTime);
            yield return new WaitForSeconds(secondWeapon.reloadingTime);

            
            if (secondWeapon.bulletsMaxCount > secondWeapon.bulletsInClip)
            {
                secondWeapon.bulletsMaxCount -= secondWeapon.bulletsInClip;
                secondWeapon.bulletsCurrentCount = secondWeapon.bulletsInClip;
            }
            else
            {
                secondWeapon.bulletsCurrentCount = secondWeapon.bulletsMaxCount;
                secondWeapon.bulletsMaxCount = 0;
            }

            changeWeaponBar.secondWeaponCooldown = false;
            changeWeaponBar.SecondWeaponCooldownBar(false, 0);
            changeWeaponBar.ChangeSeconWeaponBulletsCount(secondWeapon.bulletsCurrentCount, secondWeapon.bulletsMaxCount);
            ChangeCrosshairColor(Color.white);
            secondWeapon.weaponReloading = false;
        }
    }


    public void ChangeCrosshairColor(Color color)
    { 
        crosshairManager.ChangeColor(color);
    }


    public void Disable()
    {
        playerManager.shootJoystick.OnBeginDragEvent -= crosshairManager.EnableCrosshair;
        playerManager.shootJoystick.OnUpEvent -= Shoot;
         
    }
}
