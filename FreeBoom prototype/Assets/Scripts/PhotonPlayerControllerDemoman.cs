using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
public class PhotonPlayerControllerDemoman : MonoBehaviourPunCallbacks, IPunObservable
{
	public enum Team
	{
		None,
		Red,
		Blue
	}

	private Player player;

	[SerializeField]
	private JoystickController moveJoystick;

	[SerializeField]
	private ShootJoystick shootJoystick;

	[SerializeField, Space]
	private float speed = 0f;
	private float horizontal = 0;

	public Team team;

	//public int kills;

	//public int deaths;

	//public int assists;

	private Rigidbody2D rb;

	private Animator animator;

	private PhotonView PV;

	public PhotonPlayerListingMenu photonPlayerListingMenu;

	private PhotonGame photonGame;


	private void Awake()
	{
		moveJoystick = MoveJoystick.Instance;
		shootJoystick = ShootJoystick.Instance;

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		PV = GetComponent<PhotonView>();

		player = PhotonNetwork.LocalPlayer;
		photonPlayerListingMenu = FindObjectOfType<PhotonPlayerListingMenu>();

		photonGame = FindObjectOfType<PhotonGame>();

		if(PV.IsMine)
		photonGame.buttons[8].onClick.AddListener(() => LeaveGame());
	}

	private void Start()
	{
		if (!PV.IsMine) return;
		name = "Demoman " + player.NickName;
		byte teamNumber = (byte)player.CustomProperties["Team"];
		team = (Team)teamNumber;
		player.SetScore(0);
		player.SetKills(0); 
		player.SetDeaths(0);
		player.SetAssists(0);
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player, teamNumber);
	}

	[PunRPC]
	private void AddPlayerListing(Player player, byte teamNumber)
	{
		photonPlayerListingMenu.AddPlayerListing(player, teamNumber);
	}

	protected virtual void Update()
	{
		if (!PV.IsMine) return;

		//движение с фиксированной скоростью незавизимо от рассто€ни€ между стиком и центром джойстика
		horizontal = moveJoystick.Horizontal != 0 ? Mathf.Sign(moveJoystick.Horizontal) : 0;
		//скорость движени€ зависит от рассто€ни€ между подвижным стиком и центром джойстика
		//horizontal = moveJoystick.Horizontal;

		//	BasicMovement();

	}

	//private void BasicMovement() 
	//{
	//	horizontal = moveJoystick.Horizontal;
	//}

	private void FixedUpdate()
	{
		if (!PV.IsMine) return;
		rb.velocity = Vector2.right * horizontal * speed;
	}

	//public override void OnLeftRoom()
	//{
	//	PV.RPC("RemovePlayerListing", RpcTarget.All, PhotonNetwork.LocalPlayer);
	//}


	//[PunRPC]
	//private void RemovePlayerListing(Player player)
	//{
	//	//photonPlayerListingMenu.RemovePlayerListing(player);
	//}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		bool arg_06_0 = stream.IsWriting;
	}

	private void LeaveGame()
	{
		//PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player);
		//photonPlayerListingMenu.RemovePlayerListing(player);
		photonGame.LeaveGame(player);
	}
}
