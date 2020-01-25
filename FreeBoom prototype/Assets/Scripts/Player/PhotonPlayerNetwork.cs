using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


public class PhotonPlayerNetwork : MonoBehaviour//MonoBehaviourPunCallbacks
{
	public static PhotonPlayerNetwork Instance { get; private set; }

	//public enum Team
	//{
	//	None,
	//	Red,
	//	Blue
	//}

	//public enum Character  { None, Demoman, Engineer, Soldier };

	//[Tooltip("���� ������� �������")]
	//public Team team = Team.None;
	//[Tooltip("���� ������� ��������")]
	//public Character character = Character.None;
	[Tooltip("��� ��������� �����,���������� = null")]
	private GameObject playerInstantiate = null;

	//[Tooltip("�������� ��� �� �����")]
	//public bool alive = false;
	[Tooltip("�������� �� ����� � � ����������")]
	private bool addedPlayerToList = false;

	//public bool changeCharacter = false;
	//[Tooltip("�������� �� �� ��� ����� �������")]
	//private bool sentBoom = false;

	public Player player;

	public PhotonView PV;
	private Boom boom;
	private PhotonGame photonGame;
	private PhotonPlayerListingMenu photonPlayerListingMenu;
	private PhotonPlayerMovement photonPlayerMovement;
	public UIManager uiManager;


	//private void InitializeSingleton()
	//{
	//	if (Instance == null)
	//		Instance = this;
	//	else if (Instance != this)
	//		Destroy(this);
	//}
	private void Awake()
	{
		PV = GetComponent<PhotonView>();
		photonGame = PhotonGame.Instance;
		photonPlayerListingMenu = PhotonPlayerListingMenu.Instance;
		boom = Boom.Instance;
		uiManager = UIManager.Instance;
		player = PhotonNetwork.LocalPlayer;

		if(PV.IsMine)
		Instance = this;
		//InitializeSingleton();
	}

	void Start()
	{
		if (!PV.IsMine) return;
		OnButtonClick(); //�������� ����� �������� ������� ������ �������

	}


	//����� �������� ������� ������ �������
	private void OnButtonClick()
	{

		foreach (var panel in uiManager.panelsLists) //���� ������ ������ ����� ���� ������� � ������� UIManager
		{

			switch (panel.panelName) //�������� ������� ������
			{
				case "ChooseTeamPanel": //���� ������ ������ ChooseTeamPanel
					for (int i = 0; i < panel.panelButtons.Length; i++) //���������� �� ���� ������� � ������ ������
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //���� ������ ������ ������ ���������� ������� 
						{ 
							photonGame.ChooseTeam(panel.buttonValue[index], player); //������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
							ChooseTeam(); //�������� ����� ������ ������� � ������ �������
							RemovePlayer(); //�������� ����� �������� ������ � ������ �������
							uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // ������ ������ � ������� UIManager
							uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
						});
					}
					break;
				case "ChooseCharacterPanel":
					for (int i = 0; i < panel.panelButtons.Length; i++)
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //���� ������ ������ ������ ����������� ����������
						{ 
							photonGame.ChooseCharacter(panel.buttonValue[index], player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
							uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // ������ ������ �� ������ ���� � ������� UIManager
							uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
							CreatePlayer(); //�������� ����� c������� ������ � ���� �������
						});
					}
					break;
				case "GamePanel":
					panel.panelButtons[0].onClick.AddListener(delegate //���� ������ ������ ����
					{ 
						uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // ������ ������ �� ������ ���� � ������� UIManager
						uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					});
					break;
				case "MenuPanel":
					panel.panelButtons[0].onClick.AddListener(delegate //���� ������ ������ ������� �������
					{ 
						photonGame.ChooseTeam(panel.buttonValue[0], player); // ������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
						ChooseTeam();//�������� ����� ����� ������� � ������ �������
						RemovePlayer(); //�������� ����� �������� ������ � ������ �������
						panel.panelObjects[2].SetActive(false); //��������� ������ ����� ������� � ���������
						uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel;  // ������ ������ �� ������ ������ ��������� � ������� UIManager
						uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					}); 

					panel.panelButtons[1].onClick.AddListener(delegate //���� ������ ������ ������� ���������
					{ 
						photonGame.ChooseCharacter(panel.buttonValue[1], player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
						ChooseCharacter(); //�������� ����� ������ ��������� � ������ �������
						RemovePlayer(); //�������� ����� �������� ������ � ������ �������
						panel.panelObjects[2].SetActive(false); //��������� ������ ����� ������� � ���������
						uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // ������ ������ �� ������ ������ ��������� � ������� UIManager
						uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					}); //�������� ����� ����� ������ � ������� UIManager

					panel.panelButtons[2].onClick.AddListener(delegate //���� ������ ������ ��������� � ����
					{ 
						uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // ������ ������ �� ������ ���� � ������� UIManager
						uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					}); 
					break;
			}
		}
		
		uiManager.leaveGameBtn.onClick.AddListener(() => LeaveGame()); //����� ������ LeaveGame ������� �� ���� LeaveGame
		uiManager.exitGameBtn.onClick.AddListener(() => ExitGame()); //����� ������ ExitGame ������� �� ���� ExitGame



			boom.jumpButton.onClick.AddListener(() => //����� �������
			{
				boom.Deactivate(); //��������� ����������� ������
				StartCoroutine(photonPlayerMovement.MakeBoom(boom.finalVelocity)); //��������� ������
																				   //sentBoom = true; //����� �������
			});

		
	}


	private void RemovePlayer()
	{
		if (playerInstantiate != null)
		{
			PhotonNetwork.Destroy(playerInstantiate);
		}
	}

	private void ChooseTeam()
	{

		if (addedPlayerToList) //���� ����� ��� ������� ������� 
		{
			PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player); //������� ������ �� ����������
																		  
			//character = (Character)player.GetCharacter();
		}

		//if (playerInstantiate != null) //���� ����� ������ ����
		//{
		////	if (!sentBoom) //��������� ���� ����� �� �������
		//	//	boom.canJump = false; //��������� ����������� ������
		//	PhotonNetwork.Destroy(playerInstantiate); //���������� ������
		//}


	//team = (Team)player.GetTeam(); //���������� ���� �������

		//PV.RPC("SetPlayerTeam", RpcTarget.AllBuffered, (byte)player.GetTeam());

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);//��������� ������ � ����������
		if(!addedPlayerToList)
		addedPlayerToList = true; //����� �������� � ����������
	}

	private void ChooseCharacter()
	{
		if (addedPlayerToList)
		//{
			//character = (Character)player.GetCharacter();
		//}
		//character = (Character)player.GetCharacter();
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
	}

	//private void ChangeCharacter()
	//{
	//	if (changeCharacter)
	//	{
	//		character = (Character)player.GetCharacter();
	//	}
	//	PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
	//}
	//private void ChooseCharacter()//(string character)
	//{
	//	//photonGame.ChooseCharacter(character, player);
	//	if (playerInstantiate != null)
	//	{
	//		PhotonNetwork.Destroy(playerInstantiate);

	//	}
	//	//uiManager.currentPanel = UIManager.CurrentPanel.GamePanel;
	//	//uiManager.ChangePanel();
	//	CreatePlayer();


	//}

	//public delegate void TestCreatePlayer();//������� ��������� �������,������� ����� void � �� ����� ����� ����������
	////public TestCreatePlayer testCreatePlayer; //������ �������� ����� ��������
	//public event TestCreatePlayer testCreatePlayer;///��� �� ���������� ������� � �������,���� �������� event



	public void CreatePlayer()
	{
		Debug.LogWarning("Creater");
		//if (team == Team.Red)
		if(player.GetTeam() == PhotonTeams.Team.Red)
		{
			Debug.Log("Red");
			//character = (Character)player.GetCharacter();
			//int characterNumber = (byte)player.GetCharacter();//(byte)player.CustomProperties["Character"];
			//characterNumber--;
			int random = Random.Range(0, photonGame.teamOneSpawnPoints.Length);
			playerInstantiate =
				PhotonNetwork.Instantiate(photonGame.redTeamCharacters[(byte)player.GetCharacter() - 1].name, photonGame.teamOneSpawnPoints[random].position,
				Quaternion.identity, 0, null);
			//playerInstantiate.GetComponent<PhotonPlayerHealth>().photonPlayerNetwork = this;

			photonPlayerMovement = playerInstantiate.GetComponent<PhotonPlayerMovement>();
			//playerInstantiate.GetComponent<PhotonPlayerController>().GetPhotonPlayerNetwork(this);
			CameraFollow.Instance.SetTarget(playerInstantiate.transform);
			//alive = true;

			boom.Activate(transform, 30);

		}
		else if (player.GetTeam() == PhotonTeams.Team.Blue)
		//if (team == Team.Blue)
		{
			Debug.Log("Red");
			//character = (Character)player.GetCharacter();
			//int characterNumber = (byte)player.GetCharacter();//(byte)player.CustomProperties["Character"];
			//characterNumber--;
			//Debug.LogWarning((byte)character - 1);
			int random = Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
			playerInstantiate =
				PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[(byte)player.GetCharacter() - 1].name, photonGame.teamTwoSpawnPoints[random].position,
				Quaternion.identity, 0, null);
			//playerInstantiate.GetComponent<PhotonPlayerHealth>().photonPlayerNetwork = this;
			//playerInstantiate.transform.parent = gameObject.transform;

			photonPlayerMovement = playerInstantiate.GetComponent<PhotonPlayerMovement>();
			//playerInstantiate.GetComponent<PhotonPlayerController>().GetPhotonPlayerNetwork(this);
			CameraFollow.Instance.SetTarget(playerInstantiate.transform);
			//alive = true;

			boom.Activate(transform, -30);
		}
		else
		{
			Debug.Log("None");
		}

		//PV.RPC("SetPlayerCharacter", RpcTarget.OthersBuffered, (byte)player.GetCharacter());
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
		//photonGame.ChangeCharacterButtonActive(player);
	}


	public void PlayerDied(Player killer)
	{
		player.AddDeaths(1);
		killer.AddKills(1);
		killer.AddScore(50);
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 2);
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, killer, 2);
		uiManager.team = player.GetTeam().ToString();
		uiManager.RespawnPanelOn();
		Invoke("RespawnPlayer", uiManager.timer);
	}

	private void RespawnPlayer()
	{
		if (uiManager.canRespawn)
			CreatePlayer();
	}
	//[PunRPC]
	//public void SetPlayerCharacter(byte curentCharacter)
	//{
	//	character = (Character)curentCharacter;
	//}

	//[PunRPC]
	//public void SetPlayerTeam(byte curentTeam)
	//{
	//	team = (Team)curentTeam;
	//}

	[PunRPC]
	public void AddPlayerListing(Player playerSend, int number)
	{
		switch (number)
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
	public void RemovePlayerListing(Player playerSend)
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
