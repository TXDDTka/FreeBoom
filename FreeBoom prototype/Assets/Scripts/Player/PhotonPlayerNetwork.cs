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

	//[Tooltip("Наша текущая команда")]
	//public Team team = Team.None;
	//[Tooltip("Наша текущий персонаж")]
	//public Character character = Character.None;
	[Tooltip("Наш созданный игрок,изначально = null")]
	private GameObject playerInstantiate = null;

	//[Tooltip("Проверка жив ли игрок")]
	//public bool alive = false;
	[Tooltip("Добавлен ли игрок в в статистику")]
	private bool addedPlayerToList = false;

	//public bool changeCharacter = false;
	//[Tooltip("Проверка на то что игрок прыгнул")]
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
		OnButtonClick(); //Вызываем метод проверки нажатия кнопок игроком

	}


	//Метод проверки нажатия кнопок игроком
	private void OnButtonClick()
	{

		foreach (var panel in uiManager.panelsLists) //Ищем нужную панель среди всех панелей в скрипте UIManager
		{

			switch (panel.panelName) //Название текущей панели
			{
				case "ChooseTeamPanel": //Если данная панель ChooseTeamPanel
					for (int i = 0; i < panel.panelButtons.Length; i++) //Обращаемся ко всем кнопкам в данной панели
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //Если нажата кнопка выбора конкретной команды 
						{ 
							photonGame.ChooseTeam(panel.buttonValue[index], player); //Меняем игроку команду в скрипте PhotonGame, обновляем список команд для всех
							ChooseTeam(); //Вызываем метод выбора команды в данном скрипте
							RemovePlayer(); //Вызываем метод удаления игрока в данном скрипте
							uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // Меняем панель в скрипте UIManager
							uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
						});
					}
					break;
				case "ChooseCharacterPanel":
					for (int i = 0; i < panel.panelButtons.Length; i++)
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //Если нажата кнопка выбора конкретного персоанажа
						{ 
							photonGame.ChooseCharacter(panel.buttonValue[index], player); //Выбираем нужного персонажа в методе ChooseCharacter скрипта PhotonGame
							uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // Меняем панель на панель игры в скрипте UIManager
							uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
							CreatePlayer(); //Вызываем метод cоздания игрока в этом скрипте
						});
					}
					break;
				case "GamePanel":
					panel.panelButtons[0].onClick.AddListener(delegate //Если нажата кнопка меню
					{ 
						uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // Меняем панель на панель меню в скрипте UIManager
						uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					});
					break;
				case "MenuPanel":
					panel.panelButtons[0].onClick.AddListener(delegate //Если нажата кнопка сменить команду
					{ 
						photonGame.ChooseTeam(panel.buttonValue[0], player); // Меняем игроку команду в скрипте PhotonGame, обновляем список команд для всех
						ChooseTeam();//Вызываем метод смены команды в данном скрипте
						RemovePlayer(); //Вызываем метод удаления игрока в данном скрипте
						panel.panelObjects[2].SetActive(false); //Отключаем панель смены команды и персонажа
						uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel;  // Меняем панель на панель выбора персонажа в скрипте UIManager
						uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					}); 

					panel.panelButtons[1].onClick.AddListener(delegate //Если нажата кнопка сменить персонажа
					{ 
						photonGame.ChooseCharacter(panel.buttonValue[1], player); //Выбираем нужного персонажа в методе ChooseCharacter скрипта PhotonGame
						ChooseCharacter(); //Вызываем метод выбора персонажа в данном скрипте
						RemovePlayer(); //Вызываем метод удаления игрока в данном скрипте
						panel.panelObjects[2].SetActive(false); //Отключаем панель смены команды и персонажа
						uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // Меняем панель на панель выбора персонажа в скрипте UIManager
						uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					}); //Вызываем метод смены панели в скрипте UIManager

					panel.panelButtons[2].onClick.AddListener(delegate //Если нажата кнопка вернуться в игру
					{ 
						uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // Меняем панель на панель игры в скрипте UIManager
						uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					}); 
					break;
			}
		}
		
		uiManager.leaveGameBtn.onClick.AddListener(() => LeaveGame()); //Вызов метода LeaveGame кнопкой из меню LeaveGame
		uiManager.exitGameBtn.onClick.AddListener(() => ExitGame()); //Вызов метода ExitGame кнопкой из меню ExitGame



			boom.jumpButton.onClick.AddListener(() => //Игрок прыгает
			{
				boom.Deactivate(); //Отключаем возможность прыжка
				StartCoroutine(photonPlayerMovement.MakeBoom(boom.finalVelocity)); //Совершаем прыжек
																				   //sentBoom = true; //Игрок прыгнул
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

		if (addedPlayerToList) //Если игрок уже выбирал команду 
		{
			PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player); //Удалеям игрока из статистики
																		  
			//character = (Character)player.GetCharacter();
		}

		//if (playerInstantiate != null) //Если игрок создан нами
		//{
		////	if (!sentBoom) //Проверяем если игрок не прыгнул
		//	//	boom.canJump = false; //Отключаем возможность прыжка
		//	PhotonNetwork.Destroy(playerInstantiate); //Уничтожаем игрока
		//}


	//team = (Team)player.GetTeam(); //Определяем нашу команду

		//PV.RPC("SetPlayerTeam", RpcTarget.AllBuffered, (byte)player.GetTeam());

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);//Добавляем игрока в статистику
		if(!addedPlayerToList)
		addedPlayerToList = true; //Игрок добавлен в статистику
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

	//public delegate void TestCreatePlayer();//Сохжаем публичный делагат,который будет void и не будет иметь параметров
	////public TestCreatePlayer testCreatePlayer; //делаем экземляр этого делегата
	//public event TestCreatePlayer testCreatePlayer;///что бы превратить делегат в событие,надо добавить event



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
