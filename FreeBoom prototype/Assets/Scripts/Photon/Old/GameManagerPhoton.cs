using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManagerPhoton : MonoBehaviourPunCallbacks //добавим фотон класс который обеспечивает обратные вызовы и события
    {
        [Tooltip("Префаб игрока")]
        public GameObject playerPrefab;

        public Text playersCountText;
        public Text playerNameText;

        public Transform team1SpawnPosition;
        public Transform team2SpawnPosition;


        private int playersInTeams = 5;
        [SerializeField]
        private int playersInTeam1;
        [SerializeField]
        private int playersInTeam2;

    [Tooltip("Cоздадим Singlton")]
    public static GameManagerPhoton gameManager;

    // public int playerPerRoom;
    //  public static GameManagerPhoton Instance;

    [SerializeField] private PhotonPlayer photonPlayer;

    public override void OnEnable()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }

        
    }

    void Start()
        {

   //     photonPlayer = PhotonPlayer.Instance;
        //     Instance = this;

        // in case we started this demo with the wrong scene being active, simply load the menu scene
        /*if (!PhotonNetwork.IsConnected)
        {
          //  SceneManager.LoadScene("PunBasics-Launcher");

            return;
        }*/

        /* if (playerPrefab == null)
         { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

             Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
         }
         else
         {


             if (PlayerControllerPhoton.LocalPlayerInstance == null)
             {
                 Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

             //    playersCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
             // Мы в комнате. Создать персонажа для текущего игрока. он синхронизируется с помощью PhotonNetwork.Instantiate
             //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 15f, 0f), Quaternion.identity, 0);
             //PhotonNetwork.Instantiate(playerPrefab.name, positions.transform.position, Quaternion.identity, 0);
             playersCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();

         }
             else
             {

                 Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
             print(2);
         }


         }*/

        playerNameText.text = PhotonNetwork.NickName;
        playerNameText.text += "\n";

    }

    //public void CreatePlayerTeam1()
    //{
    //    /*if (playersInTeam1 <= playersInTeams)
    //        PhotonNetwork.Instantiate(playerPrefab.name, team1SpawnPosition.transform.position, Quaternion.identity, 0);
    //        playersInTeam1++;*/

    //    photonPlayer.CreatePlayer();
    //}

    //public void CreatePlayerTeam2()
    //{
    //    if(playersInTeam2 <= playersInTeams)
    //        PhotonNetwork.Instantiate(playerPrefab.name, team2SpawnPosition.transform.position, Quaternion.identity, 0);
    //        playersInTeam1++;
    //}


    //Вызывается, когда текущий игрок(мы) покидает комнату. 
    public override void OnLeftRoom()
        {
            //Нам нужно загрузить сцену запуска.
            SceneManager.LoadScene(0);
        }

        //Вызов метода Покинуть игру с помощью кнопки Leave Game или других условий(например смерти)
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        //Когда мы вызовем этот метод, мы собираемся загрузить соответствующую комнату, основываясь на свойстве Player Count(количество игроков), в комнтае,в которой мы находимся.
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)// если это не хост
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);

        //Cледует вызывать только в том случае, если мы являемся хостом. Поэтому мы сначала проверяем это, используя
        //Чтобы загрузить желаемый уровень, мы не используем Unity напрямую, потому что мы хотим положиться на Photon для загрузки этого уровня на всех подключенных клиентах в комнате, 
        //поскольку мы включили PhotonNetwork.AutomaticsSyncScene для этой игры.
        //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

        //Игрок подключился к комнтае
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {

       // playerPerRoom++;
       // playersCountText.text = playerPerRoom.ToString();
        //playerNameText.text = newPlayer.NickName;

        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // не видно если ты игрок который подключается
             //playerPerRoom++;
           // playersCountText.text = playerPerRoom.ToString();
            //playerNameText.text = newPlayer.NickName;
            if (PhotonNetwork.IsMasterClient)//если это хост
            {
            playersCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            playerNameText.text += PhotonNetwork.NickName;
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // вызывается перед OnPlayerLeftRoom 
              //  LoadArena();
            }
        }

        //Игрок покидает комнату
        public override void OnPlayerLeftRoom(Player newPlayer)
        {

        //playerPerRoom++;
        //playersCountText.text = playerPerRoom.ToString();
        playersCountText.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        Debug.LogFormat("OnPlayerLeftRoom() {0}", newPlayer.NickName); // видим когда другие игроки отключаются

            if (PhotonNetwork.IsMasterClient) //если это хост
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // вызывается перед OnPlayerLeftRoom
                //LoadArena();
            }

        }
    }

    //Теперь у нас есть полная настройка.Каждый раз, когда игрок присоединяется или покидает комнату, мы будем проинформированы об этом и будем вызывать метод LoadArena (), 
    //который мы реализовали ранее. Однако мы будем вызывать LoadArena () ТОЛЬКО если мы являемся хостом с использованием PhotonNetwork.IsMasterClient.

