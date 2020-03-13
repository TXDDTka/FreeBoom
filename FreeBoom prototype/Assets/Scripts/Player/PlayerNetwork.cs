using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerNetwork : MonoBehaviourPun//MonoBehaviourPunCallbacks
{
	public static PlayerNetwork Instance { get; private set; }
	[Tooltip("��� ��������� �����,���������� = null")]
	private PlayerManager playerManager = null;

	[Tooltip("�������� �� ����� � � ����������")]
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
		OnButtonClick(); //�������� ����� �������� ������� ������ �������

	}


		//public PhotonTeams.Team Team()
		//{

		//PhotonTeams.Team team = PhotonTeams.Team.None;

		//switch(_teamValue)
		//{
		//	case 0:
		//		team = PhotonTeams.Team.Red;
		//		break;
		//	case 1:
		//		team = PhotonTeams.Team.Blue;
		//		break;
		//	case 2:
		//		team = PhotonTeams.Team.Random;
		//		break;
		//}

		//return team;
		//}

		//public PhotonCharacters.Character Character()
		//{
		//PhotonCharacters.Character character = PhotonCharacters.Character.None;

		//switch (_characterValue)
		//{
		//	case 0:
		//		character = PhotonCharacters.Character.Demoman;
		//		break;
		//	case 1:
		//		character = PhotonCharacters.Character.Engineer;
		//		break;
		//	case 2:
		//		character = PhotonCharacters.Character.Soldier;
		//		break;
		//	case 3:
		//		character = PhotonCharacters.Character.Random;
		//		break;
		//}
		//return character;
		//}

	//����� �������� ������� ������ �������
	private void OnButtonClick()
	{

		foreach (var panel in _uiManager.panelsLists) //���� ������ ������ ����� ���� ������� � ������� UIManager
		{
			switch (panel.panelName) //�������� ������� ������
			{
				case UIManager.CurrentPanel.ChooseTeamPanel: //���� ������ ������ - ������ ������ �������
					for (int i = 0; i < panel.panelButtons.Length; i++) //���������� �� ���� ������� � ������ ������
					{
						int	index = i;

						panel.panelButtons[index].onClick.AddListener(delegate //���� ������ ������ ������ ���������� �������
						{
							_photonGame.ChooseTeam((PhotonTeams.Team)index + 1, _player); //������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
							ChooseTeam(); //�������� ����� ������ ������� � ������ �������
							RemovePlayer(); //�������� ����� �������� ������ � ������ �������
							_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // ������ ������ � ������� UIManager
							_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
						});
					}
					break;
				case UIManager.CurrentPanel.ChooseCharacterPanel: //���� ������ ������ - ������ ������ ��������
					for (int i = 0; i < panel.panelButtons.Length; i++)
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //���� ������ ������ ������ ����������� ����������
						{
							_photonGame.ChooseCharacter((PhotonCharacters.Character)index + 1, _player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
							_uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // ������ ������ �� ������ ���� � ������� UIManager
							_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
							CreatePlayer(); //�������� ����� c������� ������ � ���� �������

						});
					}
					break;
				case UIManager.CurrentPanel.GamePanel: //���� ������ ������ - ������ ����
					panel.panelButtons[0].onClick.AddListener(delegate //���� ������ ������ ����
					{
						//playerManager.JoysticksPointerUp();
					//	JoysticksPointerUp();
						_uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // ������ ������ �� ������ ���� � ������� UIManager
						_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					});
					break;
				case UIManager.CurrentPanel.MenuPanel: //���� ������ ������ - ������ ����
					panel.panelButtons[0].onClick.AddListener(delegate //���� ������ ������ ������� �������
					{
						//_photonGame.ChooseTeam(panel.buttonValue[0], _player); // ������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
						_photonGame.ChooseTeam(PhotonTeams.Team.AutoChoose, _player); // ������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
						ChooseTeam();//�������� ����� ����� ������� � ������ �������
						RemovePlayer(); //�������� ����� �������� ������ � ������ �������
						panel.panelObjects[2].SetActive(false); //��������� ������ ����� ������� � ���������
						_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel;  // ������ ������ �� ������ ������ ��������� � ������� UIManager
						_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					});

					panel.panelButtons[1].onClick.AddListener(delegate //���� ������ ������ ������� ���������
					{
						//	_photonGame.ChooseCharacter(panel.buttonValue[1], _player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
						_photonGame.ChooseCharacter(PhotonCharacters.Character.None, _player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
						ChooseCharacter(); //�������� ����� ������ ��������� � ������ �������
						RemovePlayer(); //�������� ����� �������� ������ � ������ �������
						panel.panelObjects[2].SetActive(false); //��������� ������ ����� ������� � ���������
						_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // ������ ������ �� ������ ������ ��������� � ������� UIManager
						_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					}); //�������� ����� ����� ������ � ������� UIManager

					panel.panelButtons[2].onClick.AddListener(delegate //���� ������ ������ ��������� � ����
					{
						_uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // ������ ������ �� ������ ���� � ������� UIManager
						_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					});
					break;
			}
		}

		_uiManager.leaveGameBtn.onClick.AddListener(() => LeaveGame()); //����� ������ LeaveGame ������� �� ���� LeaveGame
		_uiManager.exitGameBtn.onClick.AddListener(() => ExitGame()); //����� ������ ExitGame ������� �� ���� ExitGame

	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//playerManager.JoysticksPointerUp();
			JoysticksPointerUp();
			_uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // ������ ������ �� ������ ���� � ������� UIManager
			_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
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

		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2))
		{
			playerManager.playerWeaponManager.ChangeWeapon();
		}
	}




	private void ChooseTeam()
	{

		if (addedPlayerToList) //���� ����� ��� ������� �������
		{
			_PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, _player); //������� ������ �� ����������
		}
		else
		{
			addedPlayerToList = true; //����� �������� � ����������
		}

		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 0);//��������� ������ � ����������
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
				PhotonNetwork.Instantiate(_photonGame.redTeamCharacters[(byte)_player.GetCharacter() - 1].name, _photonGame.teamOneSpawnPoints[random].position,
				Quaternion.identity).GetComponent<PlayerManager>();
			//Quaternion.identity, 0, null).GetComponent<PlayerManager>();

			_uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerManager.playerMovement, 30);
		}
		else if (_player.GetTeam() == PhotonTeams.Team.Blue)
		{

			int random = Random.Range(0, _photonGame.teamTwoSpawnPoints.Length);
			playerManager =
				PhotonNetwork.Instantiate(_photonGame.blueTeamCharacters[(byte)_player.GetCharacter() - 1].name, _photonGame.teamTwoSpawnPoints[random].position,
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
		//playerManager.shootJoystick.OnBeginDragEvent -= playerManager.crosshairManager.EnableCrosshair;

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
			//playerManager.changeWeaponBar.secondWeaponCurrentBulletCountText.gameObject.SetActive(false);
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
