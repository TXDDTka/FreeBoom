using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
	private bool exitGame = false;
	[SerializeField]private InputField roomName = null;


	public  void CreateRoom()
	{
		if (string.IsNullOrEmpty(roomName.text))
		{
			int value = Environment.TickCount % 99;
			roomName.text = "Room ¹  " + value.ToString();
		}

		PhotonLoading.Load(LoadingScene.Game);
		PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 1 });

	}

	public void JoinRandomRoom()
	{
		PhotonNetwork.LocalPlayer.CustomProperties.Clear();
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		PhotonLoading.Load(LoadingScene.Game);
		PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = 10});
	}

	public override void OnJoinedRoom()
	{
		//StartGame();
		PhotonLoading.Load(LoadingScene.Game);
	}

	//private void StartGame()
	//{
	//	PhotonLoading.Load(LoadingScene.Game);
	//}

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
