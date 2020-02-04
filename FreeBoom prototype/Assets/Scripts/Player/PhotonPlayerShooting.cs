using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhotonPlayerShooting : MonoBehaviourPunCallbacks//PlayerController
{

    public enum Weapon {None,Bazuka,Pistol}
    public Weapon weaponName = Weapon.None;

    //[SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private GameObject bulletPrefab = null;
    [SerializeField] private float bulletSpeed = 0f;
    [SerializeField] private float bulletDamage = 0f;
    [SerializeField] private int bulletsInClip = 0;
    [SerializeField] private float bulletDestroyTime = 0f;

    [SerializeField] private Transform shootPoint = null;

    private PhotonView PV = null;



    public WeaponsSettingsDatabase weaponsSettingsDatabase;
    private PhotonPlayerMovement photonPlayerMovement;
    [HideInInspector]public ShootJoystick shootJoystick = null;

   // public BulletManager bulletManager;
    //Пулл объектов
   // public GameObject bulletsPrefab;
    public int spawnCount = 0;

    public List<GameObject> bulletList;

    //private const byte CustomManualInstantiationEventCode = 0;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonPlayerMovement = GetComponent<PhotonPlayerMovement>();
        shootJoystick = ShootJoystick.Instance;
       // bulletManager = BulletManager.Instance;
        //if (PV.IsMine)
        //{
        //    CheckWeapon();
        //    shootJoystick.ResetPosition();
        //    shootJoystick.OnUpEvent += Shoot;
        //}
    }

    private void Start()
    {
        ////for (int i = 0; i < spawnCount; i++)
        ////{
        ////    GameObject bullet = PhotonNetwork.Instantiate(bulletsPrefab.name, transform.position, Quaternion.identity) as GameObject;
        ////    bulletList.Add(bullet);
        ////    bullet.transform.parent = transform;
        ////    bullet.SetActive(false);
        ////}
        if (!PV.IsMine) return;
        {
         //   _objectPool.Init();
            CheckWeapon();
           // bulletManager.CreateBullets(bulletsInClip, bulletPrefab);
            shootJoystick.ResetPosition();
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

        foreach (var weaponInList in weaponsSettingsDatabase.weaponsList)
        {
            if (weaponInList.WeaponName.ToString() == weaponName.ToString())
            {
                bulletSpeed = weaponInList.BulletSpeed;
                bulletDamage = weaponInList.Damage;
                bulletsInClip = weaponInList.BulletsCountInClip;
                bulletPrefab = weaponInList.BulletPrefab;
                bulletDestroyTime = weaponInList.LifeTime;
            }
        }

        //switch (weapon)
        //{
        //    case Weapon.Bazuka:
        //        bulletSpeed = weaponSettingsDatabase[0].Speed;
        //        bulletDamage = weaponSettingsDatabase[0].Damage;
        //        break;
        //}
    }

    private void Shoot()
    {
        if (photonPlayerMovement.canMove)
        {
            //    //Debug.LogWarning("Shoot");

            //    //Bullet bulletGameobject = PhotonNetwork.Instantiate(bulletPrefab.name, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();//Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            //    //                                                                                                                                        //bulletGameobject.GetComponent<Bullet>().Set(shootPoint.forward * bulletSpeed, bulletDamage);
            //    //bulletGameobject.Set(shootPoint.forward * bulletSpeed, bulletDamage);

            //for(int i = 0; i < bulletManager.bulletList.Count; i++)
            //    {
            //        if(bulletManager.bulletList[i].activeInHierarchy == false)
            //        {
            //            bulletManager.bulletList[i].SetActive(true);
            //            bulletManager.bulletList[i].transform.position = shootPoint.position;
            //            bulletManager.bulletList[i].GetComponent<Bullet>().Set(shootPoint.forward * bulletSpeed, bulletDamage);
            //            break;
            //        }
            //        else
            //        {
            //            if(i == bulletList.Count - 1)
            //            {
            //                //GameObject newBullet = PhotonNetwork.Instantiate(bulletsPrefab.name, transform.position, Quaternion.identity) as GameObject;
            //                //newBullet.transform.parent = transform;
            //                //newBullet.SetActive(false);
            //                //bulletList.Add(newBullet);
            //               // bulletManager.AddBullet();
            //            }
            //        }
            //    }
            Bullet bulletGameobject = PhotonNetwork.Instantiate(bulletPrefab.name, shootPoint.position, Quaternion.identity).GetComponent<Bullet>();//Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
                                                                                                                                                    //bulletGameobject.GetComponent<Bullet>().Set(shootPoint.forward * bulletSpeed, bulletDamage);
            bulletGameobject.Set(shootPoint.forward * bulletSpeed, bulletDamage, bulletDestroyTime);
        }

        //private void CreateBulletPool()
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        GameObject bullet = Instantiate(bulletsPrefab, transform); //Инициализируем объект
        //        bulletList.Add(bullet);
        //        //bullet.transform.parent = transform;
        //        bullet.SetActive(false);
        //        PhotonView photonpv = bullet.GetComponent<PhotonView>(); //Обращаемся к его компоненту PhotonView(получаем ссылку)
        //                                                                 //ViewID является ключом к маршрутизации сетевых сообщений в нужный GameObject / Script
        //        if (PhotonNetwork.AllocateViewID(photonpv))//Выделяем новый ViewID для Инициализированного объекта
        //        {
        //            object[] data = new object[] //создаем массив объектов
        //            {
        //        bullet.transform.position, bullet.transform.rotation, /*bullet.transform.parent,*/ /*bullet.activeInHierarchy,*/photonpv.ViewID , bullet.activeInHierarchy
        //                //Положение,вращение объекта, родительский объект, состояние активности объекта, и выделенный ViewID
        //                //помещаем в массив и храним там данные, которые хотим отправить другим клиентам
        //            };


        //            //С помощью RaiseEventOptions мы удостоверяемся, что это событие добавляется в кэш комнаты и отправляется только другим клиентам,
        //            //потому что мы уже создали экземпляр нашего объекта локально.
        //            RaiseEventOptions raiseEventOptions = new RaiseEventOptions //Создаем параметры события
        //            {
        //                Receivers = ReceiverGroup.Others,
        //                CachingOption = EventCaching.AddToRoomCache
        //            };

        //            //С помощью SendOptions мы просто определяем, что это событие передается надежно.
        //            SendOptions sendOptions = new SendOptions //Создаем отправку параметров
        //            {
        //                Reliability = true
        //            };

        //            PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, data, raiseEventOptions, sendOptions);//Отправляем наше пользовательнское событие на сервер
        //                                                                                                               //В этом случае используем ,который является просто байтовым значением,предсталвяющим это определенное событие
        //        }

        //        else //Если выделение идентификатора для PhotonView не удается, мы регистрируем сообщение об ошибке и уничтожаем ранее созданный объект.
        //        {
        //            Debug.LogError("Failed to allocate a ViewId.");

        //            Destroy(bullet);
        //        }
        //    }
        //}

        //public void OnEvent(EventData photonEvent)
        //{
        //    if (photonEvent.Code == CustomManualInstantiationEventCode)
        //    {
        //        object[] data = (object[])photonEvent.CustomData;

        //        GameObject bullet = (GameObject)Instantiate(bulletsPrefab, (Vector3)data[0], (Quaternion)data[1]);
        //        PhotonView photonView = bullet.GetComponent<PhotonView>();

        //        photonView.ViewID = (int)data[2];
        //        bullet.SetActive((bool)data[3]);
        //    }
        //}



    }
}
