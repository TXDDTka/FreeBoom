using Photon.Pun;
using Photon.Realtime;
//using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
	public enum Team
	{
		None,
		Red,
		Blue
	}

	public Team team;

	private GameObject playerInstantiate;



	//[Header("Statistic Player Listing"), SerializeField]
	//private Text nameText;
	//[SerializeField] private Text classText;
	//[SerializeField] private Text killsText;
	//[SerializeField] private Text deathsText;
	//[SerializeField] private Text assistsText;
	//[SerializeField] private Text scoreText;
	//[SerializeField] private Text pingText;


	private PhotonView PV;
	private Player player;
	private PhotonGame photonGame;
	private PhotonPlayerListingMenu photonPlayerListingMenu;

	public bool addedPlayerToList = false;
	private void Awake()
	{
		PV = GetComponent<PhotonView>();
		photonGame = FindObjectOfType<PhotonGame>();
		//photonGame.playersList.Add(this);
		photonPlayerListingMenu = FindObjectOfType<PhotonPlayerListingMenu>();
	}

	 void Start()
	{
		
		player = PhotonNetwork.LocalPlayer;
		if (!PV.IsMine) return;
		OnButtonClick();
	}



	private void OnButtonClick()
	{
		photonGame.buttons[0].onClick.AddListener(delegate { ChooseTeam("Red"); });
		photonGame.buttons[1].onClick.AddListener(delegate { ChooseTeam("Blue"); });
		photonGame.buttons[2].onClick.AddListener(delegate { ChooseTeam("Random"); });

		photonGame.buttons[3].onClick.AddListener(delegate { ChooseCharacter("Demoman"); });
		photonGame.buttons[4].onClick.AddListener(delegate { ChooseCharacter("Engineer"); });
		photonGame.buttons[5].onClick.AddListener(delegate { ChooseCharacter("Soldier"); });
		photonGame.buttons[6].onClick.AddListener(delegate { ChooseCharacter("Random"); });


		photonGame.buttons[7].onClick.AddListener(() => LeaveGame());
		photonGame.buttons[8].onClick.AddListener(() => ExitGame());

		//photonGame.buttons[9].onClick.AddListener(() => ÑhangeCharacter());
	}


	private void ChooseTeam(string teamName)
	{

		if (addedPlayerToList)
		{
			//photonPlayerListingMenu.RemovePlayerListing(player);
			PV.RPC("RemovePlayerListing", RpcTarget.AllBufferedViaServer, player);
			addedPlayerToList = false;
		}

		if (teamName == "Red")
		{
			photonGame.ChooseTeam("Red", player);
		}
		else if (teamName == "Blue")
		{
			photonGame.ChooseTeam("Blue", player);
		}
		else if (teamName == "Random")
		{
			photonGame.ChooseTeam("Random", player);
		}

		byte currentTeam = (byte)player.CustomProperties["Team"];
		team = (Team)currentTeam;
		PV.RPC("AddPlayerListing", RpcTarget.AllBufferedViaServer, player, 0);
		addedPlayerToList = true;
	}

	private void ChooseCharacter(string character)
	{
		photonGame.ChooseCharacter(character, player);
		if (playerInstantiate != null)
		{
			PhotonNetwork.Destroy(playerInstantiate);
		}
			CreatePlayer();
	}



	public void CreatePlayer()
	{
		if (team == Team.Red)
			{
				int characterNumber = (byte)player.CustomProperties["Character"];
				characterNumber--;
				int random = Random.Range(0, photonGame.teamOneSpawnPoints.Length);
				playerInstantiate =
					PhotonNetwork.Instantiate(photonGame.redTeamCharacters[characterNumber].name, photonGame.teamOneSpawnPoints[random].position,
					photonGame.redTeamCharacters[characterNumber].transform.rotation, 0, null);

				PV.RPC("AddPlayerListing", RpcTarget.AllBufferedViaServer, player, 1);
				photonGame.ChangeCharacter(player);
		}
			if (team == Team.Blue)
			{
				int characterNumber = (byte)player.CustomProperties["Character"];
				characterNumber--;
				int random = Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
				playerInstantiate =
					PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[characterNumber].name, photonGame.teamTwoSpawnPoints[random].position,
					photonGame.blueTeamCharacters[characterNumber].transform.rotation, 0, null);

				PV.RPC("AddPlayerListing", RpcTarget.AllBufferedViaServer, player, 1);
				photonGame.ChangeCharacter(player);
		}

	}

	[PunRPC]
	void AddPlayerListing(Player player,int number)
	{
		switch(number)
		{
			case 0:
				photonPlayerListingMenu.AddPlayerListing(player);
				break;
			case 1:
				photonPlayerListingMenu.UpdatePlayerListingClass(player);
				break;
			case 2:
				photonPlayerListingMenu.UpdatePlayerListingStatistics(player);
				break;
		}
		
	}

	[PunRPC]
	void RemovePlayerListing(Player player)
	{
		photonPlayerListingMenu.RemovePlayerListing(player);

	}

	public void ClearProperties(Player player)
	{
		player.CustomProperties.Clear();
	}

	private void LeaveGame()
	{
		photonGame.LeaveGame(player);
	}

	private void ExitGame()
	{
		photonGame.ExitGame();
	}
}
