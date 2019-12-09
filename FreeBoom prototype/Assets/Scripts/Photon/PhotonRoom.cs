using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    [Tooltip("Cоздадим Singleton")]
    public static PhotonRoom room;
    private PhotonView photonview;

    public bool isGameLoaded;
    public int currentScene;
    public int multiplayerScene;

    //Player info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public int playersInGame;
    
    //Настроим Singleton
    private void Awake()
    {
       

        if (room == null)
        {
            room = this;
        }
        else
        {
            if(room != this)
            {
                Destroy(room.gameObject);
                room = this;
            }
        }
        DontDestroyOnLoad(gameObject);
       //   photonview = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }


    //Если попытка подключиться к комнтае удалась
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("Игрок " + PhotonNetwork.NickName + " подключился к комнате");
        photonPlayers = PhotonNetwork.PlayerList; //сохраняем список игроков в текущей игре
        playersInRoom = photonPlayers.Length; //сохраняем количество игроков в текущей комнате 
        myNumberInRoom = playersInRoom; //мой номер в комнате
        if (!PhotonNetwork.IsMasterClient) return;
        StartGame();
    }

    public override void OnCreatedRoom()
    {
        print("Игрок " + PhotonNetwork.NickName + " создал комнату");
    }

    void StartGame()
    {
        //if (!PhotonNetwork.IsMasterClient) //если мы не хост
        //    return;
        Debug.Log("Загрузка уровня");
        PhotonNetwork.LoadLevel(multiplayerScene);//если мы хост то загружаем сцену
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        currentScene = scene.buildIndex;
        print("Уровень " + currentScene + " загрузился");
        if (currentScene == multiplayerScene)
        {
            CreatePlayer();
           // UpdateParameters();
        }
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity,0);
        //Создан игрок
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " покинул игру");
        playersInGame--;
    }

    //public void LeaveRoom()
    //{
    //    //Покинуть текущую комнату(отстаться подключенным к мастер серверу) и вернуться в меню где можно создать или подключиться к существующей комнате
    //    PhotonNetwork.LeaveRoom();
    //}

    //Игрок покидает комнату
    //public override void OnPlayerLeftRoom(Player newPlayer)
    //{

    //    //playerPerRoom++;
    //    //playersCountText.text = playerPerRoom.ToString();
    //    //playersCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    //    // Debug.LogFormat("OnPlayerLeftRoom() {0}", newPlayer.NickName + " покинул комнату"); // видим когда другие игроки отключаются
    //    Debug.LogFormat(newPlayer.NickName + " покинул комнату");

    //    if (PhotonNetwork.IsMasterClient) //если это хост
    //    {
    //        Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", " хост " + PhotonNetwork.IsMasterClient + " покинул комнату"); // вызывается перед OnPlayerLeftRoom
    //        LoadArena();                                                                              
    //    }

    //}

    ////Когда мы вызовем этот метод, мы собираемся загрузить соответствующую комнату, основываясь на свойстве Player Count(количество игроков), в комнтае,в которой мы находимся.
    //void LoadArena()
    //{
    //    if (!PhotonNetwork.IsMasterClient)// если это не хост
    //    {
    //        Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
    //    }
    //    Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

    //    //Cледует вызывать только в том случае, если мы являемся хостом. Поэтому мы сначала проверяем это, используя
    //    //Чтобы загрузить желаемый уровень, мы не используем Unity напрямую, потому что мы хотим положиться на Photon для загрузки этого уровня на всех подключенных клиентах в комнате, 
    //    //поскольку мы включили PhotonNetwork.AutomaticsSyncScene для этой игры.
    //    //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    //}

    //void UpdateParameters()
    //{
    //        photonPlayers = PhotonNetwork.PlayerList; //сохраняем список игроков в текущей игре
    //        playersInRoom = photonPlayers.Length; //сохраняем количество игроков в текущей комнате 
    //        myNumberInRoom = playersInRoom; //мой номер в комнате
    //}

}
