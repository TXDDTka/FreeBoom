using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhotonPlayerNetwork : MonoBehaviourPun//MonoBehaviourPunCallbacks
{
	public static PhotonPlayerNetwork Instance { get; private set; }
	[Tooltip("Наш созданный игрок,изначально = null")]
	private GameObject playerInstantiate = null;

	[Tooltip("Добавлен ли игрок в в статистику")]
	private bool addedPlayerToList = false;

	private Player player;
	private PhotonView PV;
	private PhotonGame photonGame;
	private PhotonPlayerListingMenu photonPlayerListingMenu;
	private PhotonChangeWeaponBar photonChangeWeaponBar = null;
	private UIManager uiManager;

	[Header("Trajectory")]
	public GameObject pointsParent = null;
	public List<GameObject> points = new List<GameObject>();
	public int distance = 0;
	public int crosshairIndex = 0;
	public int crosshairPrefabPosition = 0;
	public Color teamColor;

	private void Awake()
	{
		PV = GetComponent<PhotonView>();
		photonGame = PhotonGame.Instance;
		photonPlayerListingMenu = PhotonPlayerListingMenu.Instance;
		photonChangeWeaponBar = PhotonChangeWeaponBar.Instance;
		uiManager = UIManager.Instance;
		player = PhotonNetwork.LocalPlayer;
		
	}

	void Start()
	{
		if (!PV.IsMine) return;
		Instance = this;
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
		
	}



	private void RemovePlayer()
	{
		if (playerInstantiate != null)
		{
			playerInstantiate.GetComponent<PhotonPlayerShooting>().Disable();
			playerInstantiate.GetComponent<PhotonPlayerHealth>().playerBar.DestroyBar();
			//playerInstantiate.GetComponent<PlayerShootingTrajectory>().Disable();
			photonChangeWeaponBar.HideBuff();
			if (photonChangeWeaponBar.secondWeaponActive)
				photonChangeWeaponBar.ChangeWeapon();
			player.AddDeaths(1);
			PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 2);
			PhotonNetwork.Destroy(playerInstantiate);
		}
		
	}

	private void ChooseTeam()
	{

		if (addedPlayerToList) //Если игрок уже выбирал команду 
			PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player); //Удалеям игрока из статистики
		else
		addedPlayerToList = true; //Игрок добавлен в статистику																  

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 0);//Добавляем игрока в статистику	
	}

	private void ChooseCharacter()
	{
		if (addedPlayerToList)
			PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);
	}



	public void CreatePlayer()
	{
		if (player.GetTeam() == PhotonTeams.Team.Red)
		{
			int random = Random.Range(0, photonGame.teamOneSpawnPoints.Length);
			playerInstantiate =
				PhotonNetwork.Instantiate(photonGame.redTeamCharacters[(byte)player.GetCharacter() - 1].name, photonGame.teamOneSpawnPoints[random].position,
				Quaternion.identity, 0, null);

			//CameraFollow.Instance.SetTarget(playerInstantiate.transform);
			uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerInstantiate.GetComponent<PhotonPlayerMovement>(), 30);
		}
		else if (player.GetTeam() == PhotonTeams.Team.Blue)
		{

			int random = Random.Range(0, photonGame.teamTwoSpawnPoints.Length);
			playerInstantiate =
				PhotonNetwork.Instantiate(photonGame.blueTeamCharacters[(byte)player.GetCharacter() - 1].name, photonGame.teamTwoSpawnPoints[random].position,
				Quaternion.identity, 0, null);

			//CameraFollow.Instance.SetTarget(playerInstantiate.transform);
			uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerInstantiate.GetComponent<PhotonPlayerMovement>(), -30);
		}



		CameraFollow.Instance.SetTarget(playerInstantiate.transform);

		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, 1);

		//if (pointsParent != null)
		//{
		//	//playerInstantiate.GetComponent<PlayerShootingTrajectory>().GetTrajectory(points, pointsParent, crosshairIndex, crosshairPrefabPosition, teamColor);
		//}
	}


	public void PlayerDied(Player killer)
	{
		RemovePlayer();
		killer.AddKills(1);
		killer.AddScore(50);
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, killer, 2);
		uiManager.team = player.GetTeam().ToString();
		uiManager.RespawnPanelOn();
		//Invoke("RespawnPlayer", uiManager.timer);
		StartCoroutine(RespawnPlayer());
	}

	private IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds(uiManager.timer);
		if (uiManager.respawn)
		{
			CreatePlayer();
			uiManager.respawn = false;
		}
			
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			// app moved to background
			photonGame.ExitGame(false);

		}
		else
		{
			// app is foreground again
		}
	}

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

	//public void GetTrajectory(List<GameObject> savePoints, GameObject saveGameObject, int saveCrosshairIndex, int saveCrosshairPrefabPosition, Color saveTeamColor)
	//{
	//	points = savePoints;
	//	pointsParent = saveGameObject;
	//	crosshairIndex = saveCrosshairIndex;
	//	crosshairPrefabPosition = saveCrosshairPrefabPosition;
	//	teamColor = saveTeamColor;
	//}

	//public void SetTrajectory(PlayerShootingTrajectory playerShootingTrajectory)
	//{ 
	//	//playerInstantiate.GetComponent<PlayerShootingTrajectory>().GetTrajectory(points, pointsParent, crosshairIndex, crosshairPrefabPosition, teamColor);
	//	playerShootingTrajectory.GetTrajectory(points, pointsParent, crosshairIndex, crosshairPrefabPosition, teamColor);
	//}

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
		photonGame.ExitGame(true);
	}
}
