using Photon.Pun;
using Photon.Realtime;
//using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
	private enum Team
	{
		None,
		Red,
		Blue
	}

	private Team team;

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

	private bool addedPlayerToList = false;
	private void Awake()
	{
		PV = GetComponent<PhotonView>();
		photonGame = PhotonGame.Instance;
		photonPlayerListingMenu = PhotonPlayerListingMenu.Instance;
	}

	 void Start()
	{
		if (!PV.IsMine) return;
		
		player = PhotonNetwork.LocalPlayer;
		OnButtonClick();
	}



	private void OnButtonClick()
	{
		photonGame.buttons[0].onClick.AddListener(delegate { photonGame.ChooseTeam("Red", player); ChooseTeam(); });
		photonGame.buttons[1].onClick.AddListener(delegate { photonGame.ChooseTeam("Blue", player); ChooseTeam(); });
		photonGame.buttons[2].onClick.AddListener(delegate { photonGame.ChooseTeam("Random", player); ChooseTeam(); });
		photonGame.buttons[9].onClick.AddListener(delegate { photonGame.ChooseTeam("AutoChoose", player); ChooseTeam(); });

		photonGame.buttons[3].onClick.AddListener(delegate { ChooseCharacter("Demoman"); });
		photonGame.buttons[4].onClick.AddListener(delegate { ChooseCharacter("Engineer"); });
		photonGame.buttons[5].onClick.AddListener(delegate { ChooseCharacter("Soldier"); });
		photonGame.buttons[6].onClick.AddListener(delegate { ChooseCharacter("Random"); });


		photonGame.buttons[7].onClick.AddListener(() => LeaveGame());
		photonGame.buttons[8].onClick.AddListener(() => ExitGame());

		photonGame.buttons[10].onClick.AddListener(() => PauseGame(true));
		photonGame.buttons[11].onClick.AddListener(() => PauseGame(false));

	}


	private void PauseGame(bool pause)
	{
		Debug.Log(pause);
		if (pause)
			Time.timeScale = 0;
		else Time.timeScale = 1;
	}
	

	private void ChooseTeam()//(string teamName)
	{

		if (addedPlayerToList)
		{
			//photonPlayerListingMenu.RemovePlayerListing(player);
			PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player);
			addedPlayerToList = false;
		}

		if (playerInstantiate != null)
		{
			PhotonNetwork.Destroy(playerInstantiate);
			//Debug.LogWarning("Персонаж уничтожен");
		}

		//switch(teamName)
		//{
		//	case "Red":
		//		photonGame.ChooseTeam("Red", player);
		//		break;
		//	case "Blue":
		//		photonGame.ChooseTeam("Blue", player);
		//		break;
		//	case "Random":
		//		photonGame.ChooseTeam("Random", player);
		//		break;
		//	case "AutoChoose":
		//		if (team == Team.Red)
		//		{
		//			photonGame.ChooseTeam("Blue", player);
		//		}
		//		else if (team == Team.Blue)
		//		{
		//			photonGame.ChooseTeam("Red", player);
		//		}
		//		break;
		//}
			//if (teamName == "Red")
			//{
			//	photonGame.ChooseTeam("Red", player);
			//}
			//else if (teamName == "Blue")
			//{
			//	photonGame.ChooseTeam("Blue", player);
			//}
			//else if (teamName == "Random")
			//{
			//	photonGame.ChooseTeam("Random", player);
			//}
			//else if(teamName == "AutoChoose")
			//{
			//	if (team == Team.Red)
			//	{
			//		photonGame.ChooseTeam("Blue", player);
			//	}
			//	else if (team == Team.Blue)
			//	{
			//		photonGame.ChooseTeam("Red", player);
			//	}
			//}
		
		//byte currentTeam = (byte)player.CustomProperties["Team"];
		//team = (Team)currentTeam;
		//PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);
		//addedPlayerToList = true;
		//if (PV.IsMine)
		//{
		//PV.RPC("RPC_GetTeam", RpcTarget.MasterClient, player);

		//byte currentTeam = (byte)player.CustomProperties["Team"];
		team = (Team)player.GetTeam();//currentTeam;
		addedPlayerToList = true;

		//PV.RPC("RPC_GetTeam", RpcTarget.AllBuffered, player);//, addedPlayerToList);
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);
		//}
	}

	private void ChooseCharacter(string character)
	{
		photonGame.ChooseCharacter(character, player);
		if (playerInstantiate != null)
		{
			PhotonNetwork.Destroy(playerInstantiate);
			//Debug.LogWarning("Персонаж уничтожен");
		}

		//PV.RPC("CreatePlayer", RpcTarget.AllBuffered, player);
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
					Quaternion.identity, 0, null);
			//photonGame.redTeamCharacters[characterNumber].transform.rotation, 0, null);

			//photonGame.ChangeCharacter(player);
			//playerInstantiate.GetComponent<PhotonPlayerController>().isFacingRight = true;
		}
		else	if (team == Team.Blue)
			{
				int characterNumber = (byte)player.CustomProperties["Character"];
				characterNumber--;
				int random = Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
				playerInstantiate =
					PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[characterNumber].name, photonGame.teamTwoSpawnPoints[random].position,
					photonGame.blueTeamCharacters[characterNumber].transform.rotation, 0, null);

			//playerInstantiate.GetComponent<PhotonPlayerController>().isFacingRight = false;
			//PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
			//photonGame.ChangeCharacter(player);
		}

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
		photonGame.ChangeCharacter(player);
	}



	//[PunRPC]
	//void RPC_GetTeam(Player playerSend)
	//{
	//	byte currentTeam = (byte)playerSend.CustomProperties["Team"];
	//	team = (Team)currentTeam;
	//	addedPlayerToList = true;

	//	//PV.RPC("RPC_SentTeam", RpcTarget.AllViaServer, player, addedPlayerToList);
	//	//PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);
	//}



	[PunRPC]
	void AddPlayerListing(Player playerSend,int number)
	{
		switch(number)
		{
			case 0:
				photonPlayerListingMenu.AddPlayerListing(playerSend);
				break;
			case 1:
				photonPlayerListingMenu.UpdatePlayerListingClass(playerSend);
				break;
			case 2:
				photonPlayerListingMenu.UpdatePlayerListingStatistics(playerSend);
				break;
		}
		
	}

	//[PunRPC]
	//void RemovePlayerListingMasterClient(Player playerSend)
	//{
	//	photonPlayerListingMenu.RemovePlayerListing(playerSend);

	//}

	[PunRPC]
	void RemovePlayerListing(Player playerSend)
	{
		photonPlayerListingMenu.RemovePlayerListing(playerSend);

	}



	public void ClearProperties(Player playerSend)
	{
		playerSend.CustomProperties.Clear();
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
