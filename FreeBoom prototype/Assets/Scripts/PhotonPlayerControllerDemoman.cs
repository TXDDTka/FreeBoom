using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;


public class PhotonPlayerControllerDemoman : MonoBehaviourPunCallbacks, IPunObservable
{
	//public enum Team
	//{
	//	None,
	//	Red,
	//	Blue
	//}

	private Player player;

	[SerializeField]
	private JoystickController moveJoystick;

	[SerializeField]
	private ShootJoystick shootJoystick;

	[SerializeField, Space]
	private float speed = 0f;
	public float gravity = 10.0f;
	public float maxVelocityChange = 10.0f;
	public bool grounded = false;

	private float horizontal = 0;

	//public Team team;

	//public int kills;

	//public int deaths;

	//public int assists;

	//private Rigidbody2D rb;

	private Rigidbody rb;

	private Animator animator;

	private PhotonView PV;

	public PhotonPlayerListingMenu photonPlayerListingMenu;

	private PhotonGame photonGame;


	public bool addPlayerToListing = false;
	private void Awake()
	{
		
		moveJoystick = MoveJoystick.Instance;
		shootJoystick = ShootJoystick.Instance;

		//rb = GetComponent<Rigidbody2D>();

		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		PV = GetComponent<PhotonView>();

		player = PhotonNetwork.LocalPlayer;
		photonPlayerListingMenu = FindObjectOfType<PhotonPlayerListingMenu>();

		photonGame = FindObjectOfType<PhotonGame>();

		//if (!PV.IsMine) return;
		//photonGame.buttons[8].onClick.AddListener(() => LeaveGame());

		//rb.freezeRotation = true;
		//rb.useGravity = false;
	//	rb.constraints = RigidbodyConstraints.FreezePositionX;
	}

	private void Start()
	{
		
		if (!PV.IsMine) return;
		name = "Demoman " + player.NickName;
	//	byte teamNumber = (byte)player.CustomProperties["Team"];
		//team = (Team)teamNumber;
		//if (team == Team.Red)
		//{
		//	int gameObjectLayer = 8;
		//	PV.RPC("ChangeLayer", RpcTarget.AllBufferedViaServer, gameObjectLayer);
		//}
		//else
		//{
		//	int gameObjectLayer = 9;
		//	PV.RPC("ChangeLayer", RpcTarget.AllBufferedViaServer, gameObjectLayer);
		//}
		player.SetScore(0);
		player.SetKills(0); 
		player.SetDeaths(0);
		player.SetAssists(0);
		//if (!addPlayerToListing)
			//PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player);//, teamNumber);
	}

	public void AddPlayerToList()
	{
		Debug.Log("AddPlayerToList");
		PV.RPC("AddPlayerListing", RpcTarget.AllBuffered, player);
	}

	[PunRPC]
	private void AddPlayerListing(Player player)//, byte teamNumber)
	{
	//	photonPlayerListingMenu.AddPlayerListing(player);//, teamNumber);


	}

	//[PunRPC]
	//private void ChangeLayer(int layer)
	//{
	//	gameObject.layer = layer;
	//}


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
		if (grounded)
		{
			//// Calculate how fast we should be moving
			//Vector3 targetVelocity = new Vector3(horizontal, 0, 0);
			//targetVelocity = transform.TransformDirection(targetVelocity);
			//targetVelocity *= speed;

			//// Apply a force that attempts to reach our target velocity
			//Vector3 velocity = rb.velocity;
			//Vector3 velocityChange = (targetVelocity - velocity);
			//velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			//velocityChange.z = 0;
			//velocityChange.y = 0;
			//rb.AddForce(velocityChange, ForceMode.Impulse);

			rb.velocity = Vector2.right * horizontal * speed;
		}

		//rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

		//grounded = false;
		//rb.velocity = Vector2.right * horizontal * speed;
		//rb.AddForce(Vector2.right * horizontal * speed);
		//rigidbody.freezeRotation = true;
		//rb.velocity.x = horizontal * speed;
		//transform.position = new Vector3(horizontal, 0.0f, 0.0f);
		//	rb.velocity = new Vector3(horizontal * speed,0f, 0f);



	}

	 void OnCollisionStay(Collision other)
	{
		if (!PV.IsMine) return;
		if (other.gameObject.tag == "Ground")
		grounded = true;
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
		//if (stream.IsWriting)
		//{
		//	stream.SendNext(rb.position);
		//	stream.SendNext(rb.rotation);
		//	stream.SendNext(rb.velocity);
		//	//stream.SendNext(rb.freezeRotation);
		//	//stream.SendNext(rb.useGravity);
		//	//stream.SendNext(rb.constraints);
		//}
		//else
		//{
		//	rb.position = (Vector3)stream.ReceiveNext();
		//	rb.rotation = (Quaternion)stream.ReceiveNext();
		//	rb.velocity = (Vector3)stream.ReceiveNext();
		////	rb.freezeRotation = (bool)stream.ReceiveNext();
		////	rb.useGravity = (bool)stream.ReceiveNext();
		////	rb.constraints = (byte)stream.ReceiveNext();

		//	float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
		//	rb.position += rb.velocity * lag;
		//}
	}

	//private void LeaveGame()
	//{
	//	//PV.RPC("RemovePlayerListing", RpcTarget.AllBuffered, player);
	//	//photonPlayerListingMenu.RemovePlayerListing(player);
	//	photonGame.LeaveGame(player);
	//}
}
