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

	[SerializeField] private UIManager uiManager = null;

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

				//ButtonsActive("Red");

				if (player.GetCharacter() != PhotonCharacters.Character.None)
				{
					player.SetCharacter(PhotonCharacters.Character.None);
					//ChangeCharacterButtonActive(player);
				}
				break;
			case "Blue":
				player.SetTeam(PhotonTeams.Team.Blue);
				photonTeams.UpdateTeams();

				//ButtonsActive("Blue");

				if (player.GetCharacter() != PhotonCharacters.Character.None)
				{
					player.SetCharacter(PhotonCharacters.Character.None);
					//ChangeCharacterButtonActive(player);
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

						//ButtonsActive("Red");
					}
					else
					{
						player.SetTeam(PhotonTeams.Team.Blue);
						photonTeams.UpdateTeams();

						//ButtonsActive("Blue");
					}
				}
				else
				{
					if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count < PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
					{
						player.SetTeam(PhotonTeams.Team.Red);
						photonTeams.UpdateTeams();


						//ButtonsActive("Red");
					}
					if (PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Red].Count > PhotonTeams.PlayersPerTeam[PhotonTeams.Team.Blue].Count)
					{
						player.SetTeam(PhotonTeams.Team.Blue);
						photonTeams.UpdateTeams();

						//buttons[1].interactable = false;
						//if (!buttons[0].interactable)
						//	buttons[0].interactable = true;
						//buttons[2].interactable = false;
					}
				}

				break;
			case "AutoChoose":
				if (player.GetTeam() == PhotonTeams.Team.Red)
				{
					player.SetTeam(PhotonTeams.Team.Blue);
					photonTeams.UpdateTeams();

					if (player.GetCharacter() != PhotonCharacters.Character.None)
					{
						player.SetCharacter(PhotonCharacters.Character.None);
						//ChangeCharacterButtonActive(player);
					}
				}
				else if (player.GetTeam() == PhotonTeams.Team.Blue)
				{
					player.SetTeam(PhotonTeams.Team.Red);
					photonTeams.UpdateTeams();

					if (player.GetCharacter() != PhotonCharacters.Character.None)
					{
						player.SetCharacter(PhotonCharacters.Character.None);
						//ChangeCharacterButtonActive(player);
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
			Debug.Log(characterNumber);
			switch (characterNumber)
			{
				case 0:

					if (player.GetCharacter() != PhotonCharacters.Character.Demoman)
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
					if (player.GetCharacter() != PhotonCharacters.Character.Engineer)
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
					if (player.GetCharacter() != PhotonCharacters.Character.Soldier)
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
			case "None":
				player.SetCharacter(PhotonCharacters.Character.None);
				photonCharacters.UpdateCharacters();
				break;
		}
	}

	//public void ChangeCharacterButtonActive(Player player)
	//{
	//	switch (player.GetCharacter())
	//	{
	//		case PhotonCharacters.Character.Demoman:
	//			uiManager.panelsLists[1].panelButtons[0].interactable = false;
	//			if (!uiManager.panelsLists[1].panelButtons[1].interactable)
	//				uiManager.panelsLists[1].panelButtons[1].interactable = true;
	//			if (uiManager.panelsLists[1].panelButtons[2].interactable)
	//				uiManager.panelsLists[1].panelButtons[2].interactable = true;
	//			break;
	//		case PhotonCharacters.Character.Engineer:
	//			uiManager.panelsLists[1].panelButtons[1].interactable = false;
	//			if (!uiManager.panelsLists[1].panelButtons[0].interactable)
	//				uiManager.panelsLists[1].panelButtons[0].interactable = true;
	//			if (uiManager.panelsLists[1].panelButtons[2].interactable)
	//				uiManager.panelsLists[1].panelButtons[2].interactable = true;
	//			break;
	//		case PhotonCharacters.Character.Soldier:
	//			uiManager.panelsLists[1].panelButtons[2].interactable = false;
	//			if (!uiManager.panelsLists[1].panelButtons[0].interactable)
	//				uiManager.panelsLists[1].panelButtons[0].interactable = true;
	//			if (uiManager.panelsLists[1].panelButtons[1].interactable)
	//				uiManager.panelsLists[1].panelButtons[1].interactable = true;
	//			break;
	//		case PhotonCharacters.Character.None:
	//			if (!uiManager.panelsLists[1].panelButtons[0].interactable)
	//				uiManager.panelsLists[1].panelButtons[0].interactable = true;
	//			if (!uiManager.panelsLists[1].panelButtons[1].interactable)
	//				uiManager.panelsLists[1].panelButtons[1].interactable = true;
	//			if (uiManager.panelsLists[1].panelButtons[2].interactable)
	//				uiManager.panelsLists[1].panelButtons[2].interactable = true;
	//			break;
	//	}
	//}

	public override void OnLeftRoom()
	{
		//Debug.Log(PhotonNetwork.LocalPlayer.NickName + " локальный игрок покинул игру");
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		//Debug.Log(otherPlayer.NickName + " покинул игру");
	}


	public void LeaveGame(Player player)
	{
		player.CustomProperties.Clear();

		PhotonNetwork.LeaveRoom();
		PhotonLoading.Load(LoadingScene.Lobby);
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
