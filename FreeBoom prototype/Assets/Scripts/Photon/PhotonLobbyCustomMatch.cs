using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class PhotonLobbyCustomMatch : MonoBehaviourPunCallbacks {

    [Tooltip("Cоздадим Singlton")]
    public static PhotonLobbyCustomMatch photonLobbyCustomMatch;

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


    //public enum Team : byte { none, red, blue };
    // public static Dictionary<Team, List<Player>> PlayersPerTeam;


    private void Awake()
    {
        photonLobbyCustomMatch = this;
    }

   /* public override void OnEnable()
    {
        if (LobbyManager.lobbyManager == null)
        {
            LobbyManager.lobbyManager = this;
        }
    }*/

    // Use this for initialization
    void Start()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
        {
          //  nameInputField.text = "Player " + Random.Range(1000, 9999);
        }
    }
	

    // Запустить процесс подключения к Master photon server.Кнопкой Login
    public void Connect()
    {
        PhotonNetwork.NickName = nameInputField.text;
        PhotonNetwork.GameVersion = gameVersion;
        //Подключаемся к Master photon server
        PhotonNetwork.ConnectUsingSettings();
    }

    //При подключение к серверу
    public override void OnConnectedToMaster()
    {
        Debug.Log("Игрок " + PhotonNetwork.NickName + " подключился к серверу");
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


    //Если попытка подключиться к комнтае удалась
   /* public override void OnJoinedRoom()
    {
        print("Игрок " + PhotonNetwork.NickName + " подключился к комнате");
        //lobbyPanel.SetActive(false);
        // gamePanel.SetActive(true);
        //Загружаем сцену
        PhotonNetwork.LoadLevel(1);
       // UpdateTeams();
    }*/



    //Если попытка подключиться к комнате не удалась
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Попытка подключиться к комнате не удалась,создаем новую комнату");
        //Мы создаем новую комнату.
          PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    //Если попытка cоздать комнату не удалась
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Попытка cоздать комнату не удалась,снова пытаемся подключиться к существующим комнатам");
        JoinRoom();
    }

    //Если создана комната
   /* public override void OnCreatedRoom()
    {
        print("Игрок " + PhotonNetwork.NickName + " создал комнату");
        lobbyPanel.SetActive(false);
      //  gamePanel.SetActive(true);
    }*/

}
