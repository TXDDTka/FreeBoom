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
        public GameObject mainWeaponBulletPrefab = null;
        public float mainWeaponBulletSpeed = 0f;
        public float mainWeaponBulletDamage = 0f;
        public int mainWeaponBulletsInClip = 0;
        public int mainWeaponBulletsMaxCount = 0;
        public float mainWeaponBulletDestroyTime = 0f;
        public int mainWeaponShootingDistance = 0;
        public GameObject mainWeaponGameobject = null;
        public Transform mainWeaponShootPoint = null;
        public bool mainWeaponChoosen = false;
    }

    [Serializable]
    public class SecondWeapon
    {
        [Tooltip("Параметры дополнительного оружия")]
        public GameObject secondWeaponBulletPrefab = null;
        public float secondWeaponBulletSpeed = 0f;
        public float secondWeaponBulletDamage = 0f;
        public int secondWeaponBulletsInClip = 0;
        public int secondWeaponBulletsMaxCount = 0;
        public float secondWeaponBulletDestroyTime = 0f;
        public int secondWeaponShootingDistance = 0;
        public GameObject secondWeaponGameobject = null;
        public Transform secondWeaponShootPoint = null;
        public bool secondWeaponChoosen = false;
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

    public Color teamColor;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }



    private void Start()
    {
        changeWeaponBar = ChangeWeaponBar.Instance;

        if (!playerManager.PV.IsMine) return;

            ChooseWeapon();
            CheckTeamColor();


        changeWeaponBar.mainWeaponButton.onClick.AddListener(() => ChangeWeapon());
        changeWeaponBar.secondWeaponButton.onClick.AddListener(() => ChangeWeapon());

        playerManager.playerShootingTrajectory.PopulatePoints(mainWeapon.mainWeaponShootingDistance, teamColor);


        playerManager.shootJoystick.OnBeginDragEvent += playerManager.playerShootingTrajectory.EnablePoints;
        playerManager.shootJoystick.OnUpEvent += Shoot;   
    }

    public void CheckTeamColor()
    {
        teamColor = PhotonNetwork.LocalPlayer.GetTeam() == PhotonTeams.Team.Red ? Color.red : Color.blue;//new Color(1, 0, 0, 1) : new Color(0, 0, 1, 1); 
    }


    private void ChooseWeapon()
    {

        for (int i = 0; i < weaponsSettingsDatabase.weaponsList.Count; i++)
        {
            var weapon = weaponsSettingsDatabase.weaponsList[i];
            if (weapon.WeaponGroup == WeaponData._WeaponGroup.MainWeapon && weapon.CharacterClass.ToString() == PhotonNetwork.LocalPlayer.GetCharacter().ToString())
            {
                mainWeapon.mainWeaponChoosen = true;
                mainWeapon.mainWeaponBulletPrefab = weapon.BulletPrefab;
                mainWeapon.mainWeaponBulletSpeed = weapon.BulletSpeed;
                mainWeapon.mainWeaponBulletDamage = weapon.Damage;
                mainWeapon.mainWeaponBulletsInClip = weapon.BulletsCountInClip;
                mainWeapon.mainWeaponBulletsMaxCount = weapon.BulletsMaxCount;
                mainWeapon.mainWeaponBulletDestroyTime = weapon.LifeTime;
                mainWeapon.mainWeaponShootingDistance =  (int)weapon.ShootingDistance;
                changeWeaponBar.ChooseMainWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
            }
            else if (weapon.WeaponGroup == WeaponData._WeaponGroup.SecondWeapon)
            {
                secondWeapon.secondWeaponChoosen = true;
                secondWeapon.secondWeaponBulletPrefab = weapon.BulletPrefab;
                secondWeapon.secondWeaponBulletSpeed = weapon.BulletSpeed;
                secondWeapon.secondWeaponBulletDamage = weapon.Damage;
                secondWeapon.secondWeaponBulletsInClip = weapon.BulletsCountInClip;
                secondWeapon.secondWeaponBulletsMaxCount = weapon.BulletsMaxCount;
                secondWeapon.secondWeaponBulletDestroyTime = weapon.LifeTime;
                secondWeapon.secondWeaponShootingDistance = (int)weapon.ShootingDistance;
                changeWeaponBar.ChooseSecondWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
            }

            if (mainWeapon.mainWeaponChoosen && secondWeapon.secondWeaponChoosen)
                return;

        }
    }

    private void ChangeWeapon()
    {
        changeWeaponBar.ChangeWeapon();
        if(currentWeapon == CurrentWeapon.MainWeapon)
        {
            currentWeapon = CurrentWeapon.SecondWeapon;

            playerManager.playerShootingTrajectory.ChangeWeaponTrajectory(secondWeapon.secondWeaponShootingDistance);
            playerManager.PV.RPC("ChangeWeaponForOthers", RpcTarget.AllBufferedViaServer, false, true);
        }
        else if(currentWeapon == CurrentWeapon.SecondWeapon)
        {
            currentWeapon = CurrentWeapon.MainWeapon;

            playerManager.playerShootingTrajectory.ChangeWeaponTrajectory(mainWeapon.mainWeaponShootingDistance);
            playerManager.PV.RPC("ChangeWeaponForOthers", RpcTarget.AllBufferedViaServer, true, false);
        }        

    }
    
    [PunRPC]
    public void ChangeWeaponForOthers(bool mainWeaponObject, bool secondWeaponObject)
    {
        mainWeapon.mainWeaponGameobject.SetActive(mainWeaponObject);
        secondWeapon.secondWeaponGameobject.SetActive(secondWeaponObject);
    }





    private void Shoot()
    {
        if (playerManager.playerMovement.canMove)
        {
            if (currentWeapon == CurrentWeapon.MainWeapon)
            {
                Bullet bulletGameobject = PhotonNetwork.Instantiate(mainWeapon.mainWeaponBulletPrefab.name, mainWeapon.mainWeaponShootPoint.position, mainWeapon.mainWeaponShootPoint.rotation).GetComponent<Bullet>();
                //bulletGameobject.Set(mainWeaponShootPoint.forward * mainWeaponBulletSpeed, mainWeaponBulletDamage, mainWeaponBulletDestroyTime);
                bulletGameobject.Set(mainWeapon.mainWeaponBulletSpeed, mainWeapon.mainWeaponBulletDamage, mainWeapon.mainWeaponBulletDestroyTime);
                // GetComponent<Rigidbody2D>().transform.right * speed;
            }
            else if (currentWeapon == CurrentWeapon.SecondWeapon)
            {
                Bullet bulletGameobject = PhotonNetwork.Instantiate(secondWeapon.secondWeaponBulletPrefab.name, secondWeapon.secondWeaponShootPoint.position, secondWeapon.secondWeaponShootPoint.rotation).GetComponent<Bullet>();
                //bulletGameobject.Set(secondWeaponShootPoint.forward * secondWeaponBulletSpeed, secondWeaponBulletDamage, secondWeaponBulletDestroyTime);
                bulletGameobject.Set(secondWeapon.secondWeaponBulletSpeed, secondWeapon.secondWeaponBulletDamage, secondWeapon.secondWeaponBulletDestroyTime);
            }
        }

    }


    public void Disable()
    {
        playerManager.shootJoystick.OnBeginDragEvent -= playerManager.playerShootingTrajectory.EnablePoints;
        playerManager.shootJoystick.OnUpEvent -= Shoot;
         
    }
}
