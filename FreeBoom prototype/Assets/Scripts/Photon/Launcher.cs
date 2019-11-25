using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;//Подключаем библиотеку Photon

public class Launcher : MonoBehaviour
{
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    ///Номер версии клиента. Пользователи отделены друг от друга c помощью gameVersion (что позволяет вносить критические изменения).
    string gameVersion = "1";

    void Awake()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        // это гарантирует, что мы можем использовать PhotonNetwork.LoadLevel () на главном клиенте, и все клиенты в одной комнате автоматически синхронизируют свой уровень
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        Connect();
    }
    /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// Запустить процесс подключения.
    /// - Если вы уже подключены, мы пытаемся присоединиться к случайной комнате
    /// - если еще не подключен, подключите этот экземпляр приложения к сети Photon Cloud
    public void Connect()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        // мы проверяем, подключены мы или нет, мы присоединяемся, если подключены, иначе мы начнем соединение с сервером.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            // # На этом этапенам нужно  попытаться присоединиться к случайной комнате. Если это не удастся, мы получим уведомление в OnJoinRandomFailed() и создадим комнату.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            // # Прежде всего мы должны  подключиться к Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}