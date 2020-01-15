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

	private bool exitGame = false;

	public void Awake()
	{
		photonTeams = GetComponent<PhotonTeams>();
		photonCharacters = GetComponent<PhotonCharacters>();
	}

	public override void OnJoinedRoom()
	{
		PhotonNetwork.Instantiate(photonNetworkPlayer.name, transform.position, Quaternion.identity, 0, null);
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		//base.OnPlayerEnteredRoom(newPlayer);
		//PhotonNetwork.Instantiate(photonNetworkPlayer.name, transform.position, Quaternion.identity, 0, null);
	}
	public void LeaveGame(Player player)
	{
		player.CustomProperties.Clear();

		PhotonNetwork.LeaveRoom();
		PhotonLoading.Load(LoadingScene.Lobby);
	}

	public void ChooseTeam(string team, Player player)
	{
		if (team == "Red")
		{
			player.SetTeam(PhotonTeams.Team.Red);
			photonTeams.UpdateTeams();

			buttons[0].interactable = false;
			if (!buttons[1].interactable)
				buttons[1].interactable = true;
				buttons[2].interactable = false;

			if (player.GetCharacter() != PhotonCharacters.Character.None)
			{
				player.SetCharacter(PhotonCharacters.Character.None);
				ChangeCharacter(player);
			}
		}
		else if (team == "Blue") 
		{
			player.SetTeam(PhotonTeams.Team.Blue);
			photonTeams.UpdateTeams();

			buttons[1].interactable = false;
			if (!buttons[0].interactable)
				buttons[0].interactable = true;
				buttons[2].interactable = false;

			if (player.GetCharacter() != PhotonCharacters.Character.None)
			{
				player.SetCharacter(PhotonCharacters.Character.None);
				ChangeCharacter(player);
			}
		}
		else if (team == "Random")
		{

				if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count == PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
				{
					int random = Random.Range(0, 1);
					if (random == 0)
					{
						player.SetTeam(PhotonTeams.Team.Red);
						photonTeams.UpdateTeams();
					}
					else
					{
						player.SetTeam(PhotonTeams.Team.Blue);
						photonTeams.UpdateTeams();
					}

				}
				else
				{
					if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count < PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
					{
						player.SetTeam(PhotonTeams.Team.Red);
						photonTeams.UpdateTeams();
						return;
					}
					if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count > PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
					{
						player.SetTeam(PhotonTeams.Team.Blue);
						photonTeams.UpdateTeams();
					}
				}

			if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count == maxPlayersInTeam)
				buttons[0].interactable = false;
			if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count == maxPlayersInTeam)
				buttons[1].interactable = false;
		}
		
	}

	public void ChangeCharacter(Player player)
	{

		if (player.GetCharacter() == PhotonCharacters.Character.Demoman)
		{
			buttons[3].interactable = false;
			if (!buttons[5].interactable)
				buttons[5].interactable = true;
			if (!buttons[4].interactable)
				buttons[4].interactable = true;
		}
		else if (player.GetCharacter() == PhotonCharacters.Character.Engineer)
		{
			buttons[4].interactable = false;
			if (!buttons[3].interactable)
				buttons[3].interactable = true;
			if (!buttons[5].interactable)
				buttons[5].interactable = true;
		}
		else if (player.GetCharacter() == PhotonCharacters.Character.Soldier)
		{
			buttons[5].interactable = false;
			if (!buttons[3].interactable)
				buttons[3].interactable = true;
			if (!buttons[4].interactable)
				buttons[4].interactable = true;
		}
		else if (player.GetCharacter() == PhotonCharacters.Character.None)
			{
				if (!buttons[3].interactable)
					buttons[3].interactable = true;
				if (!buttons[4].interactable)
					buttons[4].interactable = true;
				if (!buttons[5].interactable)
					buttons[5].interactable = true;
			}
	}

	public void ChooseCharacter(string character, Player player)
	{
		if(character == "Random")
		{
			int characterNumber = Random.Range(0, 2);
			switch (characterNumber)
			{
				case 0:

					if (player.GetCharacter() != PhotonCharacters.Character.Demoman)
						character = "Demoman";
					else
					{
						int newCharacterNumber = Random.Range(0, 1);
						if(newCharacterNumber == 0)
							character = "Engineer";
						else
							character = "Soldier";
					}
					break;
				case 1:
					if (player.GetCharacter() != PhotonCharacters.Character.Engineer)
						character = "Engineer";
					else
					{
						int newCharacterNumber = Random.Range(0, 1);
						if (newCharacterNumber == 0)
							character = "Demoman";
						else
							character = "Soldier";
					}
					break;
				case 2:
					if (player.GetCharacter() != PhotonCharacters.Character.Soldier)
						character = "Soldier";
					else
					{
						int newCharacterNumber = Random.Range(0, 1);
						if (newCharacterNumber == 0)
							character = "Demoman";
						else
							character = "Engineer";
					}
					break;
			}
		}
		switch (character)
		{
			case "Demoman":
				player.SetCharacter(PhotonCharacters.Character.Demoman);
				photonCharacters.UpdateCharacters();
				break;
			case "Engineer":
				player.SetCharacter(PhotonCharacters.Character.Engineer);
				photonCharacters.UpdateCharacters();
				break;
			case "Soldier":
				player.SetCharacter(PhotonCharacters.Character.Soldier);
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




	public void ExitGame()
	{
		exitGame = true;
		PhotonNetwork.Disconnect();

	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);

		if (exitGame == true)
			Application.Quit();
		else
			PhotonLoading.Load(LoadingScene.Login);
	}

}
