using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerNetwork : MonoBehaviourPun//MonoBehaviourPunCallbacks
{
	public static PlayerNetwork Instance { get; private set; }
	[Tooltip("Наш созданный игрок,изначально = null")]
	private PlayerManager playerManager = null;

	[Tooltip("Добавлен ли игрок в в статистику")]
	private bool addedPlayerToList = false;

	private Player _player;
	private PhotonView _PV;
	private PhotonGame _photonGame = null;
	private PhotonPlayerListingMenu _photonPlayerListingMenu = null;
	private ChangeWeaponBar _changeWeaponBar = null;
	private UIManager _uiManager = null;
	private CameraFollow _cameraFollow = null;

	private void Awake()
	{
		_PV = GetComponent<PhotonView>();

		_photonGame = PhotonGame.Instance;
		_photonPlayerListingMenu = PhotonPlayerListingMenu.Instance;
		_changeWeaponBar = ChangeWeaponBar.Instance;
		_uiManager = UIManager.Instance;
		_player = PhotonNetwork.LocalPlayer;
		_cameraFollow = CameraFollow.Instance;
	}

	void Start()
	{
		if (!_PV.IsMine) return;
		Instance = this;
		OnButtonClick(); //Вызываем метод проверки нажатия кнопок игроком

	}


	//Метод проверки нажатия кнопок игроком
	private void OnButtonClick()
	{

		foreach (var panel in _uiManager.panelsLists) //Ищем нужную панель среди всех панелей в скрипте UIManager
		{
			switch (panel.panelName) //Название текущей панели
			{
				case UIManager.CurrentPanel.ChooseTeamPanel: //Если данная панель - панель выбора команды
					for (int i = 0; i < panel.panelButtons.Length; i++) //Обращаемся ко всем кнопкам в данной панели
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //Если нажата кнопка выбора конкретной команды 
						{
							_photonGame.ChooseTeam(panel.buttonValue[index], _player); //Меняем игроку команду в скрипте PhotonGame, обновляем список команд для всех
							ChooseTeam(); //Вызываем метод выбора команды в данном скрипте
							RemovePlayer(); //Вызываем метод удаления игрока в данном скрипте
							_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // Меняем панель в скрипте UIManager
							_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
						});
					}
					break;
				case UIManager.CurrentPanel.ChooseCharacterPanel: //Если данная панель - панель выбора песонажа
					for (int i = 0; i < panel.panelButtons.Length; i++)
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //Если нажата кнопка выбора конкретного персоанажа
						{
							_photonGame.ChooseCharacter(panel.buttonValue[index], _player); //Выбираем нужного персонажа в методе ChooseCharacter скрипта PhotonGame
							_uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // Меняем панель на панель игры в скрипте UIManager
							_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
							CreatePlayer(); //Вызываем метод cоздания игрока в этом скрипте
							
						});
					}
					break;
				case UIManager.CurrentPanel.GamePanel: //Если данная панель - панель игры
					panel.panelButtons[0].onClick.AddListener(delegate //Если нажата кнопка меню
					{
						//playerManager.JoysticksPointerUp();
					//	JoysticksPointerUp();
						_uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // Меняем панель на панель меню в скрипте UIManager
						_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					});
					break;
				case UIManager.CurrentPanel.MenuPanel: //Если данная панель - панель меню
					panel.panelButtons[0].onClick.AddListener(delegate //Если нажата кнопка сменить команду
					{
						_photonGame.ChooseTeam(panel.buttonValue[0], _player); // Меняем игроку команду в скрипте PhotonGame, обновляем список команд для всех
						ChooseTeam();//Вызываем метод смены команды в данном скрипте
						RemovePlayer(); //Вызываем метод удаления игрока в данном скрипте
						panel.panelObjects[2].SetActive(false); //Отключаем панель смены команды и персонажа
						_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel;  // Меняем панель на панель выбора персонажа в скрипте UIManager
						_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					}); 

					panel.panelButtons[1].onClick.AddListener(delegate //Если нажата кнопка сменить персонажа
					{
						_photonGame.ChooseCharacter(panel.buttonValue[1], _player); //Выбираем нужного персонажа в методе ChooseCharacter скрипта PhotonGame
						ChooseCharacter(); //Вызываем метод выбора персонажа в данном скрипте
						RemovePlayer(); //Вызываем метод удаления игрока в данном скрипте
						panel.panelObjects[2].SetActive(false); //Отключаем панель смены команды и персонажа
						_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // Меняем панель на панель выбора персонажа в скрипте UIManager
						_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					}); //Вызываем метод смены панели в скрипте UIManager

					panel.panelButtons[2].onClick.AddListener(delegate //Если нажата кнопка вернуться в игру
					{
						_uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // Меняем панель на панель игры в скрипте UIManager
						_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
					}); 
					break;
			}
		}

		_uiManager.leaveGameBtn.onClick.AddListener(() => LeaveGame()); //Вызов метода LeaveGame кнопкой из меню LeaveGame
		_uiManager.exitGameBtn.onClick.AddListener(() => ExitGame()); //Вызов метода ExitGame кнопкой из меню ExitGame
		
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//playerManager.JoysticksPointerUp();
			JoysticksPointerUp(); 
			_uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // Меняем панель на панель меню в скрипте UIManager
			_uiManager.ChangePanel(); //Вызываем метод смены панели в скрипте UIManager
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			RemovePlayer();
			_uiManager.team = _player.GetTeam();
			_uiManager.RespawnPanelOn();
			StartCoroutine(RespawnPlayer());
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			playerManager.rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}




	private void ChooseTeam()
	{

		if (addedPlayerToList) //Если игрок уже выбирал команду 
		{
			_PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, _player); //Удалеям игрока из статистики
		}
		else
		{
			addedPlayerToList = true; //Игрок добавлен в статистику																  
		}

		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 0);//Добавляем игрока в статистику	
	}

	private void ChooseCharacter()
	{
		if (addedPlayerToList)
		{
			_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 1);
		}
	}



	public void CreatePlayer()
	{
		if (_player.GetTeam() == PhotonTeams.Team.Red)
		{
			int random = Random.Range(0, _photonGame.teamOneSpawnPoints.Length);
			
			playerManager =
				PhotonNetwork.Instantiate(_photonGame.redTeamCharacters[(byte)_player.GetCharacter() - 2].name, _photonGame.teamOneSpawnPoints[random].position,
				Quaternion.identity).GetComponent<PlayerManager>();
			//Quaternion.identity, 0, null).GetComponent<PlayerManager>();

			_uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerManager.playerMovement, 30);
		}
		else if (_player.GetTeam() == PhotonTeams.Team.Blue)
		{

			int random = Random.Range(0, _photonGame.teamTwoSpawnPoints.Length);
			playerManager =
				PhotonNetwork.Instantiate(_photonGame.blueTeamCharacters[(byte)_player.GetCharacter() - 2].name, _photonGame.teamTwoSpawnPoints[random].position,
				Quaternion.identity).GetComponent<PlayerManager>();

			_uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerManager.playerMovement, -30);
		}

		_cameraFollow.SetTarget(playerManager.transform);
		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 1);

	}


	public void PlayerDied(Player killer)
	{
		RemovePlayer();
		killer.AddKills(1);
		killer.AddScore(50);
		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, killer, 2);
		_uiManager.team = _player.GetTeam();
		_uiManager.RespawnPanelOn();
		StartCoroutine(RespawnPlayer());
	}

	private void RemovePlayer()
	{
		if (playerManager != null)
		{
			
			_changeWeaponBar.HideBuff();
			if (_changeWeaponBar.secondWeaponActive)
			{
				_changeWeaponBar.ChangeWeapon();
			}
			DisableEvents();
			JoysticksPointerUp();
			_player.AddDeaths(1);

			_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 2);
			PhotonNetwork.Destroy(playerManager.gameObject);

		}

	}

	public void JoysticksPointerUp()
	{
		playerManager.moveJoystick.MoveJoystickPointerUp();
		playerManager.shootJoystick.ShootJoystickPointerUp();
	}

	private void DisableEvents()
	{
		playerManager.shootJoystick.OnBeginDragEvent -= playerManager.crosshairManager.EnableCrosshair;

		if (playerManager.playerWeaponManager.currentWeapon == WeaponData._WeaponGroup.MainWeapon)
		{
			playerManager.mainWeaponShooting.DisableCurrentWeapon();
			playerManager.changeWeaponBar.mainWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
			playerManager.changeWeaponBar.MainWeaponBarReloadingStatus();

		}
		else if(playerManager.playerWeaponManager.currentWeapon == WeaponData._WeaponGroup.SecondWeapon)
		{
			playerManager.secondWeaponShooting.DisableCurrentWeapon();
			playerManager.changeWeaponBar.secondWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
			playerManager.changeWeaponBar.SecondWeaponBarReloadingStatus();
		}
	}

	private IEnumerator RespawnPlayer()
	{
		yield return new WaitForSeconds(_uiManager.timer);
		if (_uiManager.respawn)
		{
			CreatePlayer();
			_uiManager.respawn = false;
		}
			
	}


	void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			// app moved to background
			_photonGame.ExitGame(false);

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
				_photonPlayerListingMenu.AddPlayerListing(playerSend);
				break;
			case 1:
				_photonPlayerListingMenu.UpdatePlayerListingClass(playerSend);
				break;
			case 2:
				_photonPlayerListingMenu.UpdatePlayerListingStatistics(playerSend);
				break;
		}

	}


	[PunRPC]
	public void RemovePlayerListing(Player playerSend)
	{
		_photonPlayerListingMenu.RemovePlayerListing(playerSend);
	}


	//public void ClearProperties(Player playerSend)
	//{
	//	playerSend.CustomProperties.Clear();
	//}

	private void LeaveGame()
	{
		_photonGame.LeaveGame(_player);
	}

	private void ExitGame()
	{
		_photonGame.ExitGame(true);
	}
}
