using Photon.Pun;
using Photon.Realtime;
using System;
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

		photonGame.buttons[0].onClick.AddListener(delegate { TeamChoose("red"); });
		photonGame.buttons[1].onClick.AddListener(delegate { TeamChoose("blue"); });
		photonGame.buttons[2].onClick.AddListener(delegate { TeamChoose("random"); });

		photonGame.buttons[3].onClick.AddListener(delegate { CharacterChoose("demoman"); });
		photonGame.buttons[4].onClick.AddListener(delegate { CharacterChoose("engineer"); });
		photonGame.buttons[5].onClick.AddListener(delegate { CharacterChoose("soldier"); });
		photonGame.buttons[6].onClick.AddListener(delegate { CharacterChoose("random"); });

		//photonGame.buttons[3].onClick.AddListener(() => CharacterChoose(0));
		//photonGame.buttons[4].onClick.AddListener(() => CharacterChoose(1));
		//photonGame.buttons[5].onClick.AddListener(() => CharacterChoose(2));
		//photonGame.buttons[6].onClick.AddListener(() => CharacterChoose(3));

		photonGame.buttons[7].onClick.AddListener(() => LeaveGame());
		//photonGame.buttons[8].onClick.AddListener(() => LeaveGame());

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
		//if (!PV.IsMine) return;
		//switch (character)
		//{
		//case "demoman":
		//	photonGame.ChooseCharacter(0, player);
		//	SelectCharacter();
		//		break;
		//	case "engineer":
		//	photonGame.ChooseCharacter(1, player);
		//	SelectCharacter();
		//		break;
		//	case 2:
		//	photonGame.ChooseCharacter(2, player);
		//	SelectCharacter();
		//		break;
		//	case 3:
		//		photonGame.ChooseCharacter(3, player);
		//		SelectCharacter();
		//		break;
		//}
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
			int random = UnityEngine.Random.Range(0, photonGame.teamOneSpawnPoints.Length);
			//GameObject playerInstantiate = 
				PhotonNetwork.Instantiate(photonGame.redTeamCharacters[characterNumber].name, photonGame.teamOneSpawnPoints[random].position,
				photonGame.redTeamCharacters[characterNumber].transform.rotation, 0, null);

			//	Instantiate(photonGame.redTeamCharacters[characterNumber], photonGame.teamOneSpawnPoints[random].position, photonGame.redTeamCharacters[characterNumber].transform.rotation, gameObject.transform);

			//playerInstantiate.transform.parent = gameObject.transform;
		}
		if (team == Team.Blue)
		{
			byte characterNumber = (byte)player.CustomProperties["Character"];
			int random = UnityEngine.Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
			//GameObject playerInstantiate =  
				PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[characterNumber].name, photonGame.teamTwoSpawnPoints[random].position, 
				photonGame.blueTeamCharacters[characterNumber].transform.rotation, 0, null);

			//playerInstantiate.transform.parent = gameObject.transform;
		}
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		base.OnDisconnected(cause);
		photonGame.OnPlayerDisconnected(player);
	}
}
