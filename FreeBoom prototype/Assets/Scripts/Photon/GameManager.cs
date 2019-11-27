using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Photon
{
    public class GameManager : MonoBehaviourPunCallbacks //добавим фотон класс который обеспечивает обратные вызовы и события
    {
        [Tooltip("The prefab to use for representing the player")]//Префаб игрока
        public GameObject playerPrefab;

        public static GameManager Instance;

        void Awake()
        {
            if (PlayerControllerPhoton.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
        }

        //Вызывается, когда локальный игрок покидает комнату. 
        public override void OnLeftRoom()
        {
            //Нам нужно загрузить сцену запуска.
            SceneManager.LoadScene(0);
        }

        //Вызов метода Покинуть игру с помощью кнопки Leave Game
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
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        //Игрок подключился к комнтае
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // не видно если ты игрок который подключается

            if (PhotonNetwork.IsMasterClient)//если это хост
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // вызывается перед OnPlayerLeftRoom 
                LoadArena();
            }
        }

        //Игрок покидает комнату
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // видим когда другие игроки отключаются

            if (PhotonNetwork.IsMasterClient) //если это хост
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // вызывается перед OnPlayerLeftRoom
                LoadArena();
            }

        }
    }

    //Теперь у нас есть полная настройка.Каждый раз, когда игрок присоединяется или покидает комнату, мы будем проинформированы об этом и будем вызывать метод LoadArena (), 
    //который мы реализовали ранее. Однако мы будем вызывать LoadArena () ТОЛЬКО если мы являемся хостом с использованием PhotonNetwork.IsMasterClient.
}
