using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhotonPlayerShooting : MonoBehaviourPunCallbacks//PlayerController
{

    [Tooltip("Параметры дополнительного оружия")]
    [Header("WeaponParams")]
    [SerializeField] private Transform weaponHolder = null;
    public WeaponsSettingsDatabase weaponsSettingsDatabase = null;

    public enum SecondWeapon { None, Pistol }
    [SerializeField] private SecondWeapon secondWeapon = SecondWeapon.None;
    public enum CurrentWeapon { MainWeapon, SecondWeapon, Tool}
    [SerializeField] private CurrentWeapon currentWeapon = CurrentWeapon.MainWeapon;
    [SerializeField] private enum MainWeapon { None, Bazuka, Rifle, Lazer }
    [SerializeField] private MainWeapon mainWeapon = MainWeapon.None;

    [Tooltip("Параметры основного оружия")]
    [Header("MainWeapon")]
    [SerializeField] private GameObject mainWeaponBulletPrefab = null;
    [SerializeField] private float mainWeaponBulletSpeed = 0f;
    [SerializeField] private float mainWeaponBulletDamage = 0f;
    [SerializeField] private int mainWeaponBulletsInClip = 0;
    [SerializeField] private int mainWeaponBulletsMaxCount = 0;
    [SerializeField] private float mainWeaponBulletDestroyTime = 0f;
    [SerializeField] private Transform mainWeaponShootPoint = null;


    [Tooltip("Параметры дополнительного оружия")]
    [Header("SecondWeapon")]
    [SerializeField] private GameObject secondWeaponBulletPrefab = null;
    [SerializeField] private float secondWeaponBulletSpeed = 0f;
    [SerializeField] private float secondWeaponBulletDamage = 0f;
    [SerializeField] private int secondWeaponBulletsInClip = 0;
    [SerializeField] private int secondWeaponBulletsMaxCount = 0;
    [SerializeField] private float secondWeaponBulletDestroyTime = 0f;
    [SerializeField] private Transform secondWeaponShootPoint = null;

    

    private PhotonView PV = null;
    private PhotonChangeWeaponBar photonChangeWeaponBar = null;
    
    private PhotonPlayerMovement photonPlayerMovement = null;
    [HideInInspector]public ShootJoystick shootJoystick = null;


    //public List<GameObject> bulletList;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonPlayerMovement = GetComponent<PhotonPlayerMovement>();
        shootJoystick = ShootJoystick.Instance;
        photonChangeWeaponBar = PhotonChangeWeaponBar.Instance;
    }

    private void Start()
    {

        if (!PV.IsMine) return;
        {
            ChooseWeapon();
          //  CheckMainkWeapon();
           // CheckSecondkWeapon();
            shootJoystick.ResetPosition();
            photonChangeWeaponBar.mainWeaponButton.onClick.AddListener(() => ChangeWeapon());
            photonChangeWeaponBar.secondWeaponButton.onClick.AddListener(() => ChangeWeapon());
            shootJoystick.OnUpEvent += Shoot;
        }
    }

    private void ChangeWeapon()
    {
        //for(int i = 0; i < weaponHolder.)
        photonChangeWeaponBar.ChangeWeapon();
        if(currentWeapon == CurrentWeapon.MainWeapon)
        {
            weaponHolder.GetChild(0).gameObject.SetActive(false);
            weaponHolder.GetChild(1).gameObject.SetActive(true);
            currentWeapon = CurrentWeapon.SecondWeapon;
        }
        else if(currentWeapon == CurrentWeapon.SecondWeapon)
        {
            weaponHolder.GetChild(0).gameObject.SetActive(true);
            weaponHolder.GetChild(1).gameObject.SetActive(false);
            currentWeapon = CurrentWeapon.MainWeapon;
        }

    }


    public override void OnDisable()
    {
        base.OnDisable();
        if (!PV.IsMine) return;
        shootJoystick.OnUpEvent -= Shoot;
    }

    private void ChooseWeapon()
    {

        for(int i = 0; i < weaponsSettingsDatabase.weaponsList.Count; i++)
        {
            var weapon = weaponsSettingsDatabase.weaponsList[i];
            if (weapon.WeaponGroup == WeaponData._WeaponGroup.MainWeapon && weapon.CharacterClass.ToString() == PhotonNetwork.LocalPlayer.GetCharacter().ToString())
            {
               // if (weapon.CharacterClass.ToString() == PhotonNetwork.LocalPlayer.GetCharacter().ToString()  ) ; // || weapon.CharacterClass == WeaponData._CharacterClass.All)
               // {
                    Debug.Log(1);
                    Debug.Log(weapon.CharacterClass.ToString());
                    Debug.Log(PhotonNetwork.LocalPlayer.GetCharacter().ToString());
                    mainWeapon = (MainWeapon)(byte)weapon.MainWeaponName;
                    mainWeaponBulletPrefab = weapon.BulletPrefab;
                    mainWeaponBulletSpeed = weapon.BulletSpeed;
                    mainWeaponBulletDamage = weapon.Damage;
                    mainWeaponBulletsInClip = weapon.BulletsCountInClip;
                    mainWeaponBulletsMaxCount = weapon.BulletsMaxCount;
                    mainWeaponBulletDestroyTime = weapon.LifeTime;
                    photonChangeWeaponBar.ChooseMainWeapon(weapon.WeaponSprite, mainWeaponBulletsInClip, mainWeaponBulletsMaxCount);
               // }
            }
            else if (weapon.WeaponGroup == WeaponData._WeaponGroup.SecondWeapon)
            {
                Debug.Log(2);
                secondWeapon = (SecondWeapon)(byte)weapon.SecondWeaponName;
                secondWeaponBulletPrefab = weapon.BulletPrefab;
                secondWeaponBulletSpeed = weapon.BulletSpeed;
                secondWeaponBulletDamage = weapon.Damage;
                secondWeaponBulletsInClip = weapon.BulletsCountInClip;
                secondWeaponBulletsMaxCount = weapon.BulletsMaxCount;
                secondWeaponBulletDestroyTime = weapon.LifeTime;
                photonChangeWeaponBar.ChooseSecondWeapon(weapon.WeaponSprite, secondWeaponBulletsInClip, secondWeaponBulletsMaxCount);
            }

            if(mainWeapon != MainWeapon.None && secondWeapon != SecondWeapon.None)
            {
                Debug.Log(3);
                return;
            }
                
        }

        //switch(PhotonNetwork.LocalPlayer.GetCharacter())
        //{
        //    case PhotonCharacters.Character.Demoman:
        //        mainWeapon = MainWeapon.Bazuka;
        //        break;
        //}

        //secondWeapon = SecondWeapon.Pistol;
    }

    //private void CheckMainkWeapon()
    //{
    //    foreach (var weaponInList in weaponsSettingsDatabase.weaponsList)
    //    {
    //        if (weaponInList.MainWeaponName.ToString() == mainWeapon.ToString())
    //        {
    //            mainWeaponBulletPrefab = weaponInList.BulletPrefab;
    //            mainWeaponBulletSpeed = weaponInList.BulletSpeed;
    //            mainWeaponBulletDamage = weaponInList.Damage;
    //            mainWeaponBulletsInClip = weaponInList.BulletsCountInClip;
    //            mainWeaponBulletsMaxCount = weaponInList.BulletsMaxCount;
    //            mainWeaponBulletDestroyTime = weaponInList.LifeTime;
    //            photonChangeWeaponBar.mainWeaponImage.sprite = weaponInList.WeaponSprite;
    //            photonChangeWeaponBar.mainWeaponBulletCountText.text = $"{mainWeaponBulletsInClip} / {mainWeaponBulletsMaxCount}";
    //        }
    //    }
    //}

    //private void CheckSecondkWeapon()
    //{

    //    foreach (var weaponInList in weaponsSettingsDatabase.weaponsList)
    //    {
    //        if (weaponInList.SecondWeaponName.ToString() == secondWeapon.ToString())
    //        {
    //            secondWeaponBulletPrefab =  weaponInList.BulletPrefab;
    //            secondWeaponBulletSpeed = weaponInList.BulletSpeed; 
    //            secondWeaponBulletDamage = weaponInList.Damage; 
    //            secondWeaponBulletsInClip = weaponInList.BulletsCountInClip;
    //            secondWeaponBulletsMaxCount = weaponInList.BulletsMaxCount;
    //            secondWeaponBulletDestroyTime = weaponInList.LifeTime;
    //            photonChangeWeaponBar.secondWeaponImage.sprite = weaponInList.WeaponSprite;
    //            photonChangeWeaponBar.secondWeaponCurrentBulletCountText.text = $"{secondWeaponBulletsInClip} / {secondWeaponBulletsMaxCount}";
    //        }
    //    }
    //}

    private void Shoot()
    {
        if (photonPlayerMovement.canMove)
        {
            //if (currentWeapon == CurrentWeapon.MainWeapon)
            //{
            //    Bullet bulletGameobject = PhotonNetwork.Instantiate(mainWeaponBulletPrefab.name, mainWeaponShootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            //    bulletGameobject.Set(mainWeaponShootPoint.forward * mainWeaponBulletSpeed, mainWeaponBulletDamage, mainWeaponBulletDestroyTime);
            //}
            //else if(currentWeapon == CurrentWeapon.SecondWeapon)
            //{
            //    Bullet bulletGameobject = PhotonNetwork.Instantiate(secondWeaponBulletPrefab.name, secondWeaponShootPoint.position, Quaternion.identity).GetComponent<Bullet>();
            //    bulletGameobject.Set(secondWeaponShootPoint.forward * secondWeaponBulletSpeed, secondWeaponBulletDamage, secondWeaponBulletDestroyTime);
            //}
        }


    }
}
