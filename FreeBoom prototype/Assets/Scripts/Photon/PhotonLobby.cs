using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
	private bool exitGame;

	public void JoinRandomRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		PhotonLoading.Load(LoadingScene.Game);
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = 10
		}, null, null);
	}

	public override void OnJoinedRoom()
	{
		MonoBehaviour.print("Локальный игрок " + PhotonNetwork.LocalPlayer.NickName + " подключился к комнате");
		StartGame();
	}

	private void StartGame()
	{
		PhotonLoading.Load(LoadingScene.Game);
	}

	public void LoginMenu()
	{
		PhotonNetwork.Disconnect();
	}

	public void ExitGame()
	{
		exitGame = true;
		PhotonNetwork.Disconnect();
		
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);

		if(exitGame == true)
			Application.Quit();
		else
		PhotonLoading.Load(LoadingScene.Login);
	}
}
