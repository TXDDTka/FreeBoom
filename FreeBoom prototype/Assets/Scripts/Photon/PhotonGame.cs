using Photon.Pun;
//using Photon.Pun.UtilityScripts;
using Photon.Realtime;
//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotonGame : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private int maxPlayersInTeam = 5;

	private PhotonTeams photonTeams;

	private PhotonCharacters photonCharacters;

	public GameObject photonNetworkPlayer;

	//public List<GameObject> photonNetworkPlayerPrefabs;

	//public List<PhotonPlayerNetwork> playersList;

	public GameObject[] redTeamCharacters;

	public GameObject[] blueTeamCharacters;

	public Transform[] teamOneSpawnPoints;

	public Transform[] teamTwoSpawnPoints;

	public List<Button> buttons = new List<Button>();

	public void Awake()
	{
		photonTeams = GetComponent<PhotonTeams>();
		photonCharacters = GetComponent<PhotonCharacters>();
	}

	public override void OnJoinedRoom()
	{
		//PhotonNetwork.Instantiate(photonNetworkPlayer.name, transform.position, Quaternion.identity, 0, null);
		Instantiate(photonNetworkPlayer, transform.position, Quaternion.identity);
	}

	public void LeaveGame(Player player)
	{
		player.CustomProperties.Clear();

		PhotonNetwork.LeaveRoom();
		PhotonLoading.Load(LoadingScene.Lobby);
	}

	public void ChooseTeam(string team, Player player)
	{
		if (team == "red" )//&& PunTeams.PlayersPerTeam[PunTeams.Team.red].Count < maxPlayersInTeam)
		{
			player.SetTeam(PhotonTeams.Team.red);
			photonTeams.UpdateTeams();
		}
		else if (team == "blue") //&& PunTeams.PlayersPerTeam[PunTeams.Team.blue].Count < maxPlayersInTeam)
		{
			player.SetTeam(PhotonTeams.Team.blue);
			photonTeams.UpdateTeams();
		}
		else if (team == "random")
		{
			if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.red].Count == PhotonTeams.PlayersPerTeam[PhotonTeams.Team.blue].Count)
			{
				int random = Random.Range(0, 1);
				if (random == 0)
				{
					player.SetTeam(PhotonTeams.Team.red);
					photonTeams.UpdateTeams();
				}
				else
				{
					player.SetTeam(PhotonTeams.Team.blue);
					photonTeams.UpdateTeams();
				}
				
			}
			else
			{
				if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.red].Count < PhotonTeams.PlayersPerTeam[PhotonTeams.Team.blue].Count)
				{
					player.SetTeam(PhotonTeams.Team.red);
					photonTeams.UpdateTeams();
					return;
				}
				if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.red].Count > PhotonTeams.PlayersPerTeam[PhotonTeams.Team.blue].Count)
				{
					player.SetTeam(PhotonTeams.Team.blue);
					photonTeams.UpdateTeams();
				}
			}
		}
		if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.red].Count == maxPlayersInTeam)
			buttons[0].interactable = false;
		if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.blue].Count == maxPlayersInTeam)
			buttons[1].interactable = false;
	}

	public void ChooseCharacter(string character, Player player)
	{
		if(character == "random")
		{
			int characterNumber = Random.Range(0, 2);
			switch (characterNumber)
			{
				case 0:
					character = "demoman";
					break;
				case 1:
					character = "engineer";
					break;
				case 2:
					character = "soldier";
					break;
			}
		}
		switch (character)
		{
			case "demoman":
				player.SetCharacter(PhotonCharacters.Character.demoman);
				photonCharacters.UpdateCharacters();
				break;
			case "engineer":
				player.SetCharacter(PhotonCharacters.Character.demoman);
				photonCharacters.UpdateCharacters();
				break;
			case "soldier":
				player.SetCharacter(PhotonCharacters.Character.demoman);
				photonCharacters.UpdateCharacters();
				break;
		}
	}

	public override void OnLeftRoom()
	{
		Debug.Log(PhotonNetwork.LocalPlayer.NickName + " локальный игрок покинул игру");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		Debug.Log(otherPlayer.NickName + " покинул игру");
	}

	//public void OnPlayerDisconnected(Player player)
	//{
	//	Debug.Log(player.NickName + "дисконектнулся с сервера 1");
	//	PhotonLoading.Load(LoadingScene.Login);
	//}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		PhotonLoading.Load(LoadingScene.Login);
	}

}
