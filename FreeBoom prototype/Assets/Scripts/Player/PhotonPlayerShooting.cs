using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotonPlayerShooting : MonoBehaviourPunCallbacks
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
    [SerializeField] private CurrentWeapon currentWeapon = CurrentWeapon.MainWeapon;

    //[Tooltip("Параметры основного оружия")]
    //[Header("MainWeapon")]
    //[SerializeField] private GameObject mainWeaponBulletPrefab = null;
    //[SerializeField] private float mainWeaponBulletSpeed = 0f;
    //[SerializeField] private float mainWeaponBulletDamage = 0f;
    //[SerializeField] private int mainWeaponBulletsInClip = 0;
    //[SerializeField] private int mainWeaponBulletsMaxCount = 0;
    //[SerializeField] private float mainWeaponBulletDestroyTime = 0f;
    //[SerializeField] private Transform mainWeaponShootPoint = null;
    //[SerializeField] private GameObject mainWeaponGameobject = null;
    //[SerializeField] private bool mainWeaponChoosen = false;

    //[Tooltip("Параметры дополнительного оружия")]
    //[Header("SecondWeapon")]
    //[SerializeField] private GameObject secondWeaponBulletPrefab = null;
    //[SerializeField] private float secondWeaponBulletSpeed = 0f;
    //[SerializeField] private float secondWeaponBulletDamage = 0f;
    //[SerializeField] private int secondWeaponBulletsInClip = 0;
    //[SerializeField] private int secondWeaponBulletsMaxCount = 0;
    //[SerializeField] private float secondWeaponBulletDestroyTime = 0f;
    //[SerializeField] private Transform secondWeaponShootPoint = null;
    //[SerializeField] private GameObject secondWeapon = null;
    //[SerializeField] private bool secondWeaponChoosen = false;


    private PhotonView PV = null;
    private PhotonChangeWeaponBar photonChangeWeaponBar = null;
    private PhotonPlayerMovement photonPlayerMovement = null;
    private ShootJoystick shootJoystick = null;
    private PlayerShootingTrajectory playerShootingTrajectory = null;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonPlayerMovement = GetComponent<PhotonPlayerMovement>();
        playerShootingTrajectory = GetComponent<PlayerShootingTrajectory>();
        shootJoystick = ShootJoystick.Instance;
        photonChangeWeaponBar = PhotonChangeWeaponBar.Instance;
    }



    private void Start()
    {

        if (!PV.IsMine) return;

           // mainWeapon.mainWeaponChoosen = false;///?
           // secondWeapon.secondWeaponChoosen = false;///?
            ChooseWeapon();
            shootJoystick.ResetPosition();
            photonChangeWeaponBar.mainWeaponButton.onClick.AddListener(() => ChangeWeapon());
            photonChangeWeaponBar.secondWeaponButton.onClick.AddListener(() => ChangeWeapon());
            shootJoystick.OnUpEvent += Shoot;   
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
                photonChangeWeaponBar.ChooseMainWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
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
                photonChangeWeaponBar.ChooseSecondWeapon(weapon.WeaponSprite, weapon.BulletsCountInClip, weapon.BulletsMaxCount);
            }

            if (mainWeapon.mainWeaponChoosen && secondWeapon.secondWeaponChoosen)
                return;

        }
    }

    private void ChangeWeapon()
    {
        photonChangeWeaponBar.ChangeWeapon();
        if(currentWeapon == CurrentWeapon.MainWeapon)
        {
            currentWeapon = CurrentWeapon.SecondWeapon;
            playerShootingTrajectory.ChangeSecondWeaponTrajectory();
            PV.RPC("ChangeWeaponForOthers", RpcTarget.AllBufferedViaServer, false, true);
        }
        else if(currentWeapon == CurrentWeapon.SecondWeapon)
        {
            currentWeapon = CurrentWeapon.MainWeapon;
            playerShootingTrajectory.ChangeMainWeaponTrajectory();
            PV.RPC("ChangeWeaponForOthers", RpcTarget.AllBufferedViaServer, true, false);
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
        if (photonPlayerMovement.canMove)
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
            shootJoystick.OnUpEvent -= Shoot;
    }
}
