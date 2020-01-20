using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
	//private enum Team
	//{
	//	None,
	//	Red,
	//	Blue
	//}

	//private Team team;

	private int team = 0;

	private GameObject playerInstantiate;

	//public static PhotonPlayerNetwork Instance { get; private set; }

	private PhotonView PV;
	public Player player;
	private PhotonGame photonGame;
	private PhotonPlayerListingMenu photonPlayerListingMenu;
	private PhotonPlayerController photonPlayerController;
	private bool addedPlayerToList = false;
	private bool sentBoom = false;

	public Boom boom;
	private void Awake()
	{
		
		PV = GetComponent<PhotonView>();

		
		photonGame = PhotonGame.Instance;
		photonPlayerListingMenu = PhotonPlayerListingMenu.Instance;
		boom = Boom.Instance;
		//	boom = Boom.Instance;
		//if (PV.IsMine)
		//{
		//Instance = this;
		player = PhotonNetwork.LocalPlayer;

		//if (!PV.IsMine) return;
		
	}

	void Start()
	{
		if (!PV.IsMine) return;
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

		boom.jumpButton.onClick.AddListener(() => 
		{
			//boom.Activate(false, null, 0);
			boom.Deactivate();
			StartCoroutine(photonPlayerController.MakeBoom(boom.finalVelocity));
			sentBoom = true;
		});
	}




	private void ChooseTeam()//(string teamName)
	{

		if (addedPlayerToList)
		{
			PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player);
			addedPlayerToList = false;
		}

		if (playerInstantiate != null)
		{
			if (!sentBoom)
				boom.isGame = false;
			PhotonNetwork.Destroy(playerInstantiate);
		}

		//team = (Team)player.GetTeam();
		team = (byte)player.GetTeam();
		addedPlayerToList = true;

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);
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

	//public delegate void TestCreatePlayer();//—охжаем публичный делагат,который будет void и не будет иметь параметров
	////public TestCreatePlayer testCreatePlayer; //делаем экземл€р этого делегата
	//public event TestCreatePlayer testCreatePlayer;///что бы превратить делегат в событие,надо добавить event



	public void CreatePlayer()
	{
		if (team == 1)//(team == Team.Red)
			{
				int characterNumber = (byte)player.CustomProperties["Character"];
				characterNumber--;
				int random = Random.Range(0, photonGame.teamOneSpawnPoints.Length);
				playerInstantiate =
					PhotonNetwork.Instantiate(photonGame.redTeamCharacters[characterNumber].name, photonGame.teamOneSpawnPoints[random].position,
					Quaternion.identity, 0, null);

				photonPlayerController = playerInstantiate.GetComponent<PhotonPlayerController>();
			//boom.Activate(true, transform, 30);
			boom.Activate(transform, 30);

		}
		else	if (team == 2)//(team == Team.Blue)
		{
				int characterNumber = (byte)player.CustomProperties["Character"];
				characterNumber--;
				int random = Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
				playerInstantiate =
					PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[characterNumber].name, photonGame.teamTwoSpawnPoints[random].position,
					photonGame.blueTeamCharacters[characterNumber].transform.rotation, 0, null);


			photonPlayerController = playerInstantiate.GetComponent<PhotonPlayerController>();
			//boom.Activate(true, transform, -30);
			boom.Activate(transform, -30);
		}

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
		photonGame.ChangeCharacter(player);
	}




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
