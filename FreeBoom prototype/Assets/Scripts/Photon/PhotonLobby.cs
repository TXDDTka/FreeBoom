using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PhotonLobby : MonoBehaviourPunCallbacks
{

    [Tooltip("Cоздадим Singlton")]
    public static PhotonLobby lobby;

    [Tooltip("The Ui Panel для ввода имени и подключения к серверу")]
    [SerializeField]
    private GameObject loginPanel;
    [Tooltip(" UI Panel где игрок будет видеть статистику и выбирать команду")]
    [SerializeField]
    private GameObject lobbyPanel;
    [Tooltip(" UI Panel c игрой")]

    public InputField nameInputField;

    [Tooltip("Номер версии клиента. Пользователи отделены друг от друга c помощью gameVersion (что позволяет вносить критические изменения)")]
    string gameVersion = "1";
    [Tooltip("Максимальное количество игроков в комнате")]
    byte maxPlayersPerRoom = 10;


    private void Awake()
    {
        lobby = this;
    }


    // Use this for initialization
    //void Start()
    //{
    //    if (string.IsNullOrEmpty(nameInputField.text))
    //    {
    //        //  nameInputField.text = "Player " + Random.Range(1000, 9999);
    //    }
    //}


    // Запустить процесс подключения к Master photon server.Кнопкой Login
    public void Connect()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
            nameInputField.text = "Player " + Random.Range(1000, 9999);
        }
        PhotonNetwork.NickName = nameInputField.text;
        PhotonNetwork.GameVersion = gameVersion;
        //Подключаемся к Master photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    //При подключение к серверу
    public override void OnConnectedToMaster()
    {
        print("Игрок " + PhotonNetwork.NickName + " подключился к Master Server");
        loginPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        // это гарантирует, что мы можем использовать PhotonNetwork.LoadLevel () на главном клиенте, и все клиенты в одной комнате автоматически синхронизируют свой уровень
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //Вызываем метод подключения к комнате Кнопкой "Join 5x5 mode"
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    //Если попытка подключиться к комнате не удалась
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //Мы создаем новую комнату.
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    //Если попытка cоздать комнату не удалась
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //Cнова пытаемся подключиться к существующим комнатам")
        JoinRoom();
    }

    //Если создана комната
    //public override void OnCreatedRoom()
    //{
    //    print("Игрок " + PhotonNetwork.NickName + " создал комнату");
    //    //lobbyPanel.SetActive(false);
    //  //  gamePanel.SetActive(true);
    //}

    //Если попытка подключиться к комнтае удалась
    //public override void OnJoinedRoom()
    //{
    //    print("Игрок " + PhotonNetwork.NickName + " подключился к комнате");
    //   // //lobbyPanel.SetActive(false);
    //   // // gamePanel.SetActive(true);
    //   // //Загружаем сцену
    //   // PhotonNetwork.LoadLevel(1);
    //   //// UpdateTeams();
    //}

    //public void LeaveRoom()
    //{
    //    //Покинуть текущую комнату(отстаться подключенным к мастер серверу) и вернуться в меню где можно создать или подключиться к существующей комнате
    //    PhotonNetwork.LeaveRoom();
    //}

    //public override void OnJoinedRoom()
    //{
    //    if (!PhotonNetwork.IsMasterClient) //если мы не хост
    //        return;
    //    PhotonNetwork.LoadLevel(1);//если мы хост то загружаем сцену
    //    print("Игрок " + PhotonNetwork.NickName + " подключился к комнате");
    //}

    //public override void OnCreatedRoom()
    //{
    //    print("Игрок " + PhotonNetwork.NickName + " создал комнату");
    //}
}
