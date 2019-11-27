using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//Подключаем библиотеку Photon
using Photon.Realtime;

namespace Photon
{

    public class Launcher : MonoBehaviourPunCallbacks //добавим фотон класс который обеспечивает обратные вызовы и события
    {
        ///Номер версии клиента. Пользователи отделены друг от друга c помощью gameVersion (что позволяет вносить критические изменения).
        string gameVersion = "1";

        /// /// Максимальное количество игроков в комнате. Когда комната заполнена, к ней не могут присоединиться новые игроки, поэтому будет создана новая комната.
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        /// Отслеживать текущий процесс. Поскольку соединение асинхронное и основано на нескольких обратных вызовах от Photon,
        /// нам нужно следить за этим, чтобы правильно настроить поведение при получении обратного вызова от Photon.
        /// Обычно это используется для обратного вызова OnConnectedToMaster ().
        bool isConnecting;

        void Awake()
        {
            // это гарантирует, что мы можем использовать PhotonNetwork.LoadLevel () на главном клиенте, и все клиенты в одной комнате автоматически синхронизируют свой уровень
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        // Запустить процесс подключения.
        // - Если вы уже подключены, мы пытаемся присоединиться к случайной комнате
        // - если еще не подключен, подключите этот экземпляр приложения к сети Photon Cloud
        public void Connect()
        {
            // отслеживаем состояние подключения к комнате, потому что когда мы покинем игру, мы получим ответ, что мы подключены, поэтому нам нужно знать, что делать
            isConnecting = true;

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // мы проверяем, подключены мы или нет, мы присоединяемся, если подключены, иначе мы начнем соединение с сервером.
            if (PhotonNetwork.IsConnected)
            {
                // # На этом этапенам нужно  попытаться присоединиться к случайной комнате. Если это не удастся, мы получим уведомление в OnJoinRandomFailed() и создадим комнату.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // # Прежде всего мы должны  подключиться к Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        //Добавлены следующие два метода в конец класса в пределах области MonoBehaviourPunCallbacks Обратные вызовы для ясности
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // Первое, что мы пытаемся сделать, это присоединиться к потенциальной существующей комнате. Если есть, хорошо, в противном случае, мы вызовем OnJoinRandomFailed ()

         // мы не хотим ничего делать, если не пытаемся присоединиться к комнате.
         // в этом случае isConnecting имеет значение false, как правило, когда вы проигрываете или выходите из игры, когда загружается этот уровень, в этом случае будет вызываться OnConnectedToMaster
         // мы не хотим ничего делать.
            if (isConnecting)
            {
                // первое, что мы пытаемся сделать, это присоединиться к потенциальной существующей комнате. Если есть, хорошо, в противном случае мы вызовем метод OnJoinRandomFailed ()
                PhotonNetwork.JoinRandomRoom();
            }
        }

        //Подключились к комнате
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
            // Мы загружаемся только если мы первый игрок, иначе мы полагаемся на `PhotonNetwork.Automatics Sync Scene` для синхронизации нашей сцены экземпляра.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");


                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        //Покинули комнату
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        //Нам не удалось присоединиться к случайной комнате, возможно, ни одна из них не существует или все они заполнены. 
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            //Мы создаем новую комнату.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }


    }
}