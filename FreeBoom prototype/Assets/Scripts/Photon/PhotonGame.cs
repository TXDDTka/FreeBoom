using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotonGame : MonoBehaviourPunCallbacks
{

	public static PhotonGame Instance { get; private set; }

	private void InitializeSingleton()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy(this);
	}

	[SerializeField]
	private int maxPlayersInTeam = 5;

	private PhotonTeams photonTeams;

	private PhotonCharacters photonCharacters;

	public GameObject photonNetworkPlayer;

	public GameObject[] redTeamCharacters;

	public GameObject[] blueTeamCharacters;

	public Transform[] teamOneSpawnPoints;

	public Transform[] teamTwoSpawnPoints;

	private bool exitGame = false;

	private UIManager uiManager = null;

	public void Awake()
	{
		InitializeSingleton();
		photonTeams = GetComponent<PhotonTeams>();
		photonCharacters = GetComponent<PhotonCharacters>();
	}

	public override void OnJoinedRoom()
	{
		PhotonNetwork.Instantiate(photonNetworkPlayer.name, transform.position, Quaternion.identity, 0, null);
	}

	public void ChooseTeam(string team, Player player)
	{

		switch (team)
		{
			case "Red":
				player.SetTeam(PhotonTeams.Team.Red);
				photonTeams.UpdateTeams();


				if (player.GetCharacter() != CharacterData._CharacterClass.None)
				{
					player.SetCharacter(CharacterData._CharacterClass.None);
				}
				break;
			case "Blue":
				player.SetTeam(PhotonTeams.Team.Blue);
				photonTeams.UpdateTeams();


				if (player.GetCharacter() != CharacterData._CharacterClass.None)
				{
					player.SetCharacter(CharacterData._CharacterClass.None);
				}
				break;
			case "Random":

				if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count == PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
				{
					int random = Random.Range(0, 2);
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


					}
					if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count > PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
					{
						player.SetTeam(PhotonTeams.Team.Blue);
						photonTeams.UpdateTeams();
					}
				}

				break;
			case "AutoChoose":
				if (player.GetTeam() == PhotonTeams.Team.Red)
				{
					player.SetTeam(PhotonTeams.Team.Blue);
					photonTeams.UpdateTeams();

					if (player.GetCharacter() != CharacterData._CharacterClass.None)
					{
						player.SetCharacter(CharacterData._CharacterClass.None);
					}
				}
				else if (player.GetTeam() == PhotonTeams.Team.Blue)
				{
					player.SetTeam(PhotonTeams.Team.Red);
					photonTeams.UpdateTeams();

					if (player.GetCharacter() != CharacterData._CharacterClass.None)
					{
						player.SetCharacter(CharacterData._CharacterClass.None);
					}
				}
				break;
		}
		if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count == maxPlayersInTeam)
		{
			uiManager.panelsLists[0].panelButtons[0].interactable = false;
			uiManager.panelsLists[0].panelButtons[2].interactable = false;
		}
		if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count == maxPlayersInTeam)
		{
			uiManager.panelsLists[0].panelButtons[1].interactable = false;
			uiManager.panelsLists[0].panelButtons[2].interactable = false;
		}
		if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count == maxPlayersInTeam && PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count == maxPlayersInTeam)
			uiManager.panelsLists[3].panelButtons[0].interactable = false;

	}

	public void ChooseCharacter(string character, Player player)
	{
		if(character == "Random")
		{
			int characterNumber = Random.Range(0, 3);
			switch (characterNumber)
			{
				case 0:

					if (player.GetCharacter() != CharacterData._CharacterClass.Demoman)
						character = "Demoman";
					else
					{
						int newCharacterNumber = Random.Range(0, 2);
						if(newCharacterNumber == 0)
							character = "Engineer";
						else
							character = "Soldier";
					}
					break;
				case 1:
					if (player.GetCharacter() != CharacterData._CharacterClass.Engineer)
						character = "Engineer";
					else
					{
						int newCharacterNumber = Random.Range(0, 2);
						if (newCharacterNumber == 0)
							character = "Demoman";
						else
							character = "Soldier";
					}
					break;
				case 2:
					if (player.GetCharacter() != CharacterData._CharacterClass.Soldier)
						character = "Soldier";
					else
					{
						int newCharacterNumber = Random.Range(0, 2);
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
				player.SetCharacter(CharacterData._CharacterClass.Demoman);
				photonCharacters.UpdateCharacters();
				break;
			case "Engineer":
				player.SetCharacter(CharacterData._CharacterClass.Engineer);
				photonCharacters.UpdateCharacters();
				break;
			case "Soldier":
				player.SetCharacter(CharacterData._CharacterClass.Soldier);
				photonCharacters.UpdateCharacters();
				break;
			case "None":
				player.SetCharacter(CharacterData._CharacterClass.None);
				photonCharacters.UpdateCharacters();
				break;
		}
	}





	public void LeaveGame(Player player)
	{

		PhotonNetwork.LeaveRoom();
		PhotonNetwork.JoinLobby();
		PhotonLoading.Load(LoadingScene.Lobby);
	}

	public void ExitGame(bool exitStatus)
	{
		exitGame = exitStatus;
		PhotonNetwork.Disconnect();

	}


	public override void OnLeftRoom()
	{
		//Debug.Log(PhotonNetwork.LocalPlayer.NickName + " ��������� ����� ������� ����");
		//PhotonNetwork.LocalPlayer.CustomProperties.Clear();

	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		//Debug.Log(otherPlayer.NickName + " ������� ����");
		//otherPlayer.CustomProperties.Clear();
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
