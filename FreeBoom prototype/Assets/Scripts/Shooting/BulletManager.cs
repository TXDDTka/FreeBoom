using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviourPunCallbacks/*, IPunPrefabPool*/
{
    public static BulletManager Instance { get; private set; }
    // public PhotonView PV;

    //Пулл объектов
    public GameObject bulletsPrefab;
    //public int spawnCount = 0;

    public List<GameObject> bulletList;
    public GameObject bullet;

    private const byte CustomManualInstantiationEventCode = 0;

    public override void OnEnable()
    {
        base.OnEnable();
       // PhotonNetwork.PrefabPool = this;
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    void Start()
    {
        //PV = GetComponent<PhotonView>();
        InitializeSingleton();
        //PhotonNetwork.PrefabPool = this;
        //CreateBullets();
        // BulletInstantiateEvent();

        
     //   PhotonNetwork.PrefabPool = this;
     //   _objectpool = new DictionaryObjectPool();
        //GameObject go = PhotonNetwork.Instantiate(bullet.name, transform.position, Quaternion.identity);
      //  _objectpool.AddObjectPool("Bullet", bullet, this.transform, 1); //Добавляет пулю в пул и на сцену дочерним объектом
    }

    //public void CreateBullets(int spawnCount, GameObject bulletsPrefab)
    //{
    //    for (int i = 0; i < spawnCount; i++)
    //    {

    //        /*GameObject*/
    //        bullet = Instantiate(bulletsPrefab, transform);
    //        bulletList.Add(bullet);
    //        bullet.SetActive(false);

    //    }
    //}



private void BulletInstantiateEvent()
{
    for (int i = 0; i < 3; i++)
    {
        GameObject bullet = Instantiate(bulletsPrefab, transform); //Инициализируем объект
        bulletList.Add(bullet);
        //bullet.transform.parent = transform;
        bullet.SetActive(false);
        PhotonView photonpv = bullet.GetComponent<PhotonView>(); //Обращаемся к его компоненту PhotonView(получаем ссылку)
                                                                 //ViewID является ключом к маршрутизации сетевых сообщений в нужный GameObject / Script
        if (PhotonNetwork.AllocateViewID(photonpv))//Выделяем новый ViewID для Инициализированного объекта
        {
            object[] data = new object[] //создаем массив объектов
            {
            bullet.transform.position, bullet.transform.rotation, /*bullet.transform.parent,*/ /*bullet.activeInHierarchy,*/photonpv.ViewID , bullet.activeInHierarchy
                //Положение,вращение объекта, родительский объект, состояние активности объекта, и выделенный ViewID
                //помещаем в массив и храним там данные, которые хотим отправить другим клиентам
            };


            //С помощью RaiseEventOptions мы удостоверяемся, что это событие добавляется в кэш комнаты и отправляется только другим клиентам,
            //потому что мы уже создали экземпляр нашего объекта локально.
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions //Создаем параметры события
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            //С помощью SendOptions мы просто определяем, что это событие передается надежно.
            SendOptions sendOptions = new SendOptions //Создаем отправку параметров
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(CustomManualInstantiationEventCode, data, raiseEventOptions, sendOptions);//Отправляем наше пользовательнское событие на сервер
                                                                                                               //В этом случае используем ,который является просто байтовым значением,предсталвяющим это определенное событие
        }

        else //Если выделение идентификатора для PhotonView не удается, мы регистрируем сообщение об ошибке и уничтожаем ранее созданный объект.
        {
            Debug.LogError("Failed to allocate a ViewId.");

            Destroy(bullet);
        }

    }

}

public void OnEvent(EventData photonEvent)
{
    if (photonEvent.Code == CustomManualInstantiationEventCode)
    {
        object[] data = (object[])photonEvent.CustomData;

        GameObject bullet = (GameObject)Instantiate(bulletsPrefab, (Vector3)data[0], (Quaternion)data[1]);
        PhotonView photonView = bullet.GetComponent<PhotonView>();

        photonView.ViewID = (int)data[2];
        bullet.SetActive((bool)data[3]);
    }
}
}

//[PunRPC]
//public void AddToList()
//{
//    //for (int i = 0; i < spawnCount; i++)
//    //{

//        GameObject bullet = PhotonNetwork.Instantiate(bulletsPrefab.name, transform.position, Quaternion.identity) as GameObject;
//       // bulletList.Add(bullet);
//        //bullet.transform.parent = transform;
//        //bullet.SetActive(false);

//    //}
//}





//public void OnPhotonInstantiate(PhotonMessageInfo info)
//{

//    bulletList.Add(info.photonView.gameObject);
//    info.photonView.transform.parent = transform;
//    info.photonView.gameObject.SetActive(false);
//}

//[PunRPC]
//public void AddToList(List<GameObject> newList,GameObject bulletHide,Transform playerTransform)
//{
//    newList.Add(bulletHide);
//    bulletHide.transform.parent = playerTransform;
//    bulletHide.SetActive(false);
//}

//[PunRPC]
//public void CreateBullet()
//{
//    for (int i = 0; i < spawnCount; i++)
//    {
//        GameObject bullet = PhotonNetwork.Instantiate(bulletsPrefab.name, transform.position, Quaternion.identity) as GameObject;
//        bulletList.Add(bullet);
//        bullet.transform.parent = transform;
//        bullet.SetActive(false);
//    }
//}

// Update is called once per frame
//public void AddBullet()
//{
//    GameObject newBullet = PhotonNetwork.Instantiate(bulletsPrefab.name, transform.position, Quaternion.identity) as GameObject;
//    newBullet.transform.parent = transform;
//    newBullet.SetActive(false);
//    bulletList.Add(newBullet);
//}

