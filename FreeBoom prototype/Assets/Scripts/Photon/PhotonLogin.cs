using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLogin : MonoBehaviourPunCallbacks
{

	[SerializeField]
	private InputField nameInputField = null;

	[SerializeField]
	private GameSettings gameSettings = null;

public void Connect()
	{
		if (string.IsNullOrEmpty(nameInputField.text))
		{
			nameInputField.text = gameSettings.NickName;
		}
		PhotonNetwork.NickName = nameInputField.text;
		PhotonNetwork.GameVersion = gameSettings.GameVersion;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Локальный игрок " + PhotonNetwork.NickName + " подключился к Master Server");
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.JoinLobby();
		PhotonLoading.Load(LoadingScene.Lobby);
	}


	public override void OnCustomAuthenticationFailed(string debugMessage)
	{
		Debug.Log("Не удалось подключиться к серверу, проверьте включен ли сервер");
	}


	public void ExitGame()
	{
		Application.Quit();
	}
}
