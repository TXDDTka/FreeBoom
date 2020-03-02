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
	private PlayerManager playerInstantiate = null;

	[Tooltip("�������� �� ����� � � ����������")]
	private bool addedPlayerToList = false;

	private Player _player;
	private PhotonView _PV;
	private PhotonGame _photonGame = null;
	private PhotonPlayerListingMenu _photonPlayerListingMenu = null;
	private ChangeWeaponBar _changeWeaponBar = null;
	private UIManager _uiManager = null;

	[Header("Trajectory")]
	public GameObject pointsParent = null;
	public List<GameObject> points = new List<GameObject>();
	public int distance = 0;
	public int crosshairIndex = 0;
	public int crosshairPrefabPosition = 0;
	public Color teamColor;

	private void Awake()
	{
		_PV = GetComponent<PhotonView>();

		_photonGame = PhotonGame.Instance;
		_photonPlayerListingMenu = PhotonPlayerListingMenu.Instance;
		_changeWeaponBar = ChangeWeaponBar.Instance;
		_uiManager = UIManager.Instance;
		_player = PhotonNetwork.LocalPlayer;
	}

	void Start()
	{
		if (!_PV.IsMine) return;
		Instance = this;
		OnButtonClick(); //�������� ����� �������� ������� ������ �������

	}


	//����� �������� ������� ������ �������
	private void OnButtonClick()
	{

		foreach (var panel in _uiManager.panelsLists) //���� ������ ������ ����� ���� ������� � ������� UIManager
		{

			switch (panel.panelName) //�������� ������� ������
			{
				case "ChooseTeamPanel": //���� ������ ������ ChooseTeamPanel
					for (int i = 0; i < panel.panelButtons.Length; i++) //���������� �� ���� ������� � ������ ������
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //���� ������ ������ ������ ���������� ������� 
						{
							_photonGame.ChooseTeam(panel.buttonValue[index], _player); //������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
							ChooseTeam(); //�������� ����� ������ ������� � ������ �������
							RemovePlayer(); //�������� ����� �������� ������ � ������ �������
							_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel; // ������ ������ � ������� UIManager
							_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
						});
					}
					break;
				case "ChooseCharacterPanel":
					for (int i = 0; i < panel.panelButtons.Length; i++)
					{
						int index = i;
						panel.panelButtons[index].onClick.AddListener(delegate //���� ������ ������ ������ ����������� ����������
						{
							_photonGame.ChooseCharacter(panel.buttonValue[index], _player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
							_uiManager.currentPanel = UIManager.CurrentPanel.GamePanel; // ������ ������ �� ������ ���� � ������� UIManager
							_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
							CreatePlayer(); //�������� ����� c������� ������ � ���� �������
							
						});
					}
					break;
				case "GamePanel":
					panel.panelButtons[0].onClick.AddListener(delegate //���� ������ ������ ����
					{
						//playerInstantiate.JoysticksPointerUp();
						JoysticksPointerUp();
						_uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // ������ ������ �� ������ ���� � ������� UIManager
						_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					});
					break;
				case "MenuPanel":
					panel.panelButtons[0].onClick.AddListener(delegate //���� ������ ������ ������� �������
					{
						_photonGame.ChooseTeam(panel.buttonValue[0], _player); // ������ ������ ������� � ������� PhotonGame, ��������� ������ ������ ��� ����
						ChooseTeam();//�������� ����� ����� ������� � ������ �������
						RemovePlayer(); //�������� ����� �������� ������ � ������ �������
						panel.panelObjects[2].SetActive(false); //��������� ������ ����� ������� � ���������
						_uiManager.currentPanel = UIManager.CurrentPanel.ChooseCharacterPanel;  // ������ ������ �� ������ ������ ��������� � ������� UIManager
						_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
					}); 

					panel.panelButtons[1].onClick.AddListener(delegate //���� ������ ������ ������� ���������
					{
						_photonGame.ChooseCharacter(panel.buttonValue[1], _player); //�������� ������� ��������� � ������ ChooseCharacter ������� PhotonGame
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
			//playerInstantiate.JoysticksPointerUp();
			JoysticksPointerUp(); 
			_uiManager.currentPanel = UIManager.CurrentPanel.MenuPanel; // ������ ������ �� ������ ���� � ������� UIManager
			_uiManager.ChangePanel(); //�������� ����� ����� ������ � ������� UIManager
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			RemovePlayer();
			_uiManager.team = _player.GetTeam().ToString();
			_uiManager.RespawnPanelOn();
			StartCoroutine(RespawnPlayer());
		}

	}


	public void JoysticksPointerUp()
	{
		//  playerShootingTrajectory.EnablePoints(false);
		playerInstantiate.moveJoystick.MoveJoystickPointerUp();
		playerInstantiate.shootJoystick.ShootJoystickPointerUp();
	}

	private void ChooseTeam()
	{

		if (addedPlayerToList) //���� ����� ��� ������� ������� 
			_PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, _player); //������� ������ �� ����������
		else
		addedPlayerToList = true; //����� �������� � ����������																  

		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 0);//��������� ������ � ����������	
	}

	private void ChooseCharacter()
	{
		if (addedPlayerToList)
			_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 1);
	}



	public void CreatePlayer()
	{
		if (_player.GetTeam() == PhotonTeams.Team.Red)
		{
			int random = Random.Range(0, _photonGame.teamOneSpawnPoints.Length);
			playerInstantiate =
				PhotonNetwork.Instantiate(_photonGame.redTeamCharacters[(byte)_player.GetCharacter() - 1].name, _photonGame.teamOneSpawnPoints[random].position,
				Quaternion.identity, 0, null).GetComponent<PlayerManager>();

			//CameraFollow.Instance.SetTarget(playerInstantiate.transform);
			_uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerInstantiate.playerMovement, 30);
		}
		else if (_player.GetTeam() == PhotonTeams.Team.Blue)
		{

			int random = Random.Range(0, _photonGame.teamTwoSpawnPoints.Length);
			playerInstantiate =
				PhotonNetwork.Instantiate(_photonGame.blueTeamCharacters[(byte)_player.GetCharacter() - 1].name, _photonGame.teamTwoSpawnPoints[random].position,
				Quaternion.identity, 0, null).GetComponent<PlayerManager>();

			//CameraFollow.Instance.SetTarget(playerInstantiate.transform);
			_uiManager.panelsLists[2].panelObjects[3].SetActive(true);

			BoomJump.Instance.Activate(playerInstantiate.playerMovement, -30);
		}

		CameraFollow.Instance.SetTarget(playerInstantiate.transform);
		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 1);

	}


	public void PlayerDied(Player killer)
	{
		RemovePlayer();
		killer.AddKills(1);
		killer.AddScore(50);
		_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, killer, 2);
		_uiManager.team = _player.GetTeam().ToString();
		_uiManager.RespawnPanelOn();
		StartCoroutine(RespawnPlayer());
	}

	private void RemovePlayer()
	{
		if (playerInstantiate != null)
		{
			playerInstantiate.playerShooting.Disable();
			JoysticksPointerUp();
			_changeWeaponBar.HideBuff();
			if (_changeWeaponBar.secondWeaponActive)
				_changeWeaponBar.ChangeWeapon();
			_player.AddDeaths(1);

			_PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, _player, 2);
			PhotonNetwork.Destroy(playerInstantiate.gameObject);

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


	public void ClearProperties(Player playerSend)
	{
		playerSend.CustomProperties.Clear();
	}

	private void LeaveGame()
	{
		_photonGame.LeaveGame(_player);
	}

	private void ExitGame()
	{
		_photonGame.ExitGame(true);
	}
}
