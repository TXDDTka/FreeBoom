using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
	private bool exitGame = false;



	//public override void OnJoinedLobby()
	//{
	//	base.OnJoinedLobby();
	//	//PhotonNetwork.LocalPlayer.CustomProperties.Clear();
	//}
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
