using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : MonoBehaviourPunCallbacks
{
    // [Header("General Properties")]
    // [SerializeField] private float speed = 5;

    //public enum Team
    //{
    //    None,
    //    Red,
    //    Blue
    //}

    //public Team team;
    private float horizontal = 0;
    public float speed = 0f;
    private bool grounded = false;

    public float health = 100f;

    //private bool isGrounded = false;

    private MoveJoystick moveJoystick = null;
    private ShootJoystick shootJoystick = null;
   
    private Rigidbody rb = null;
    private Animator animator = null;

    //private PhotonPlayerListingMenu photonPlayerListingMenu;// { get; set; }
    private PhotonGame photonGame;

    private PhotonView PV;// { get; set; }
    private Player player;// { get; set; }
    //private CameraWork cameraWork;

    [SerializeField]
    private GameObject playerUiPrefab = null;
    // private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        player = PhotonNetwork.LocalPlayer;

        //photonPlayerListingMenu = FindObjectOfType<PhotonPlayerListingMenu>();
        photonGame = FindObjectOfType<PhotonGame>();


        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //cameraWork = gameObject.GetComponent<CameraWork>();
        //player.SetScore(0);
        //player.SetKills(0);
        //player.SetDeaths(0);
        //player.SetAssists(0);

        //  PV.RPC("AddPlayerListing", RpcTarget.All);//, PhotonNetwork.LocalPlayer);
    }


    private void Start()
    {
        //if (cameraWork != null)
        //{
        //    if (PV.IsMine)
        //    {
        //        cameraWork.OnStartFollowing();
        //    }
        //}
        //team = (Team)player.GetTeam();
        GameObject _uiGo = Instantiate(playerUiPrefab);
        _uiGo.SendMessage("SetPlayer", this, SendMessageOptions.RequireReceiver);

    }

    //private void Awake()
    //{


    //    //rb = GetComponent<Rigidbody2D>();




    //    //player = PhotonNetwork.LocalPlayer;


    //    //if (!PV.IsMine) return;
    //    //photonGame.buttons[8].onClick.AddListener(() => LeaveGame());

    //  //  transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    //}
    //  void Start()
    //{
    //    //  moveJoystick = MoveJoystick.Instance;
    //    //  shootJoystick = ShootJoystick.Instance;


    //    //  anim = GetComponent<Animator>();
    //    //  sr = GetComponent<SpriteRenderer>();
    //    ////  PV = GetComponent<PhotonView>();
    //    ///

    //    if (!PV.IsMine) return;
    //    Debug.Log("Start");

    //   // rb.isKinematic = true;
    //    //Physics.IgnoreLayerCollision(8,9);
    //    //Physics.ig
    //    //player.SetScore(0);
    //    //player.SetKills(0);
    //    //player.SetDeaths(0);
    //    //player.SetAssists(0);

    //}

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine) return;
        //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
        horizontal = moveJoystick.Horizontal != 0 ? Mathf.Sign(moveJoystick.Horizontal) : 0;

        //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
        //horizontal = moveJoystick.Horizontal;

        //isGrounded = Physics2D.OverlapBox(transform.position, groundCheckSize, 0, groundLayer);
    }

    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if (grounded)
        {
             rb.velocity = new Vector2(horizontal * speed, rb.velocity.x);

            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    rb.position = new Vector3(horizontal, 0.0f, 0.0f);
            //    rb.MovePosition(rb.position + transform.forward * 0.5f);
            //}
           // rb.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime);
        }
    }
    //public void AddPlayerToList()
    //{
    //     Instantiate();

    //    player.SetScore(0);
    //    player.SetKills(0);
    //    player.SetDeaths(0);
    //    player.SetAssists(0);

    //    // PhotonView ph= PhotonView.Get(this);
    //    //photonView.RPC("ChatMessage", RpcTarget.All, "jup", "and jup.");

    //    PV.RPC("AddPlayerListing", RpcTarget.AllBufferedViaServer, player);
    //   // Debug.Log("Hello world");
    //}

    //[PunRPC]
    //void AddPlayerListing(Player player)
    //{
    //    Debug.Log(2);
    //    photonPlayerListingMenu.AddPlayerListing(player);
    //}

    void OnCollisionStay(Collision other)
    {
        if (!PV.IsMine) return;
        if (other.gameObject.tag == "Ground")
            grounded = true;
    //    else if (other.gameObject.tag == "Player")
    //    {
    //        byte teamNumber = (byte)other.gameObject.GetComponent<PhotonPlayerController>().player.CustomProperties["Team"];
    //        byte thisPlayerTeamNumber = (byte)player.CustomProperties["Team"];
    //        Debug.Log("enemy " + teamNumber);
    //        Debug.Log("my" + thisPlayerTeamNumber);
    //        if (teamNumber == thisPlayerTeamNumber)
    //        {
    //            Physics.IgnoreCollision(GetComponent<Collider>(),other.gameObject.GetComponent<Collider>());
    //        }
    //            }
            
    }
}
