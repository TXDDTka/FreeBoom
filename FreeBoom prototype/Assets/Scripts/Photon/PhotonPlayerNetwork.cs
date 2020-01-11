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
		photonGame = FindObjectOfType<PhotonGame>();
		//photonGame.playersList.Add(this);
		photonPlayerListingMenu = FindObjectOfType<PhotonPlayerListingMenu>();
	}

	private void Start()
	{
		//if (!PV.IsMine) return;

		player = PhotonNetwork.LocalPlayer;
		name = player.NickName;

		OnButtonClick();
	}

	private void OnButtonClick()
	{

		photonGame.buttons[0].onClick.AddListener(delegate { TeamChoose("red"); });
		photonGame.buttons[1].onClick.AddListener(delegate { TeamChoose("blue"); });
		photonGame.buttons[2].onClick.AddListener(delegate { TeamChoose("random"); });

		photonGame.buttons[3].onClick.AddListener(delegate { CharacterChoose("demoman"); });
		photonGame.buttons[4].onClick.AddListener(delegate { CharacterChoose("engineer"); });
		photonGame.buttons[5].onClick.AddListener(delegate { CharacterChoose("soldier"); });
		photonGame.buttons[6].onClick.AddListener(delegate { CharacterChoose("random"); });


		photonGame.buttons[7].onClick.AddListener(() => LeaveGame());
		photonGame.buttons[8].onClick.AddListener(() => LeaveGame());
	}


	private void TeamChoose(string team)
	{
		if (team == "red")
		{
			photonGame.ChooseTeam("red", player);
			SelectTeam();
		}
		if (team == "blue")
		{
			photonGame.ChooseTeam("blue", player);
			SelectTeam();
		}
		if (team == "random")
		{
			photonGame.ChooseTeam("random", player);
			SelectTeam();
		}
		
	}

	private void CharacterChoose(string character)
	{
			photonGame.ChooseCharacter(character, player);
			SelectCharacter();
	}

	private void LeaveGame()
	{
		photonGame.LeaveGame(player);
	}

	public void ClearProperties(Player player)
	{
		player.CustomProperties.Clear();
	}

	public void SelectTeam()
	{
		byte currentTeam = (byte)player.CustomProperties["Team"];
		team = (Team)currentTeam;
	}

	public void SelectCharacter()
	{
		//if (!PV.IsMine)
		//{
		//	return;
		//}
		if (team == Team.Red)
		{
			int characterNumber = (byte)player.CustomProperties["Character"];
			int random = Random.Range(0, photonGame.teamOneSpawnPoints.Length);
			GameObject playerInstantiate =
				PhotonNetwork.Instantiate(photonGame.redTeamCharacters[characterNumber].name, photonGame.teamOneSpawnPoints[random].position,
				photonGame.redTeamCharacters[characterNumber].transform.rotation, 0, null);

			if (addedPlayerToList == false)
			{
				//playerInstantiate.GetComponent<PhotonDemoman>().AddPlayerToList();

				playerInstantiate.GetComponent<PhotonDemoman>().AddPlayerToList();
				//playerInstantiate.GetComponent<PhotonView>().RPC("AddPlayerListing", RpcTarget.AllBuffered);

				addedPlayerToList = true;
			}

		}
		if (team == Team.Blue)
		{
			byte characterNumber = (byte)player.CustomProperties["Character"];
			int random = UnityEngine.Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
			GameObject playerInstantiate =  
				PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[characterNumber].name, photonGame.teamTwoSpawnPoints[random].position, 
				photonGame.blueTeamCharacters[characterNumber].transform.rotation, 0, null);

			if (addedPlayerToList == false)
			{
				playerInstantiate.GetComponent<PhotonDemoman>().AddPlayerToList();
				addedPlayerToList = true;
			}
		}
	}

	//[PunRPC]
	//public void AddPlayerListing()
	//{
	//	Debug.Log(2);
	//	//photonPlayerListingMenu.AddPlayerListing(player);
	//}

}
