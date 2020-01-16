using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : MonoBehaviourPun
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
   
    public float speed = 0f;
    private bool grounded = false;


    private float horizontal = 0;
    public bool isFacingRight = true;
    private Vector3 lookPosition = Vector3.zero;
    //[SerializeField] private bool mirror = false;


    public float health = 100f;

    //private bool isGrounded = false;

    private MoveJoystick moveJoystick = null;
    private ShootJoystick shootJoystick = null;
   
    private Rigidbody rb = null;
    private Animator anim = null;

    //private PhotonPlayerListingMenu photonPlayerListingMenu;// { get; set; }
    //private PhotonGame photonGame;

    private PhotonView PV;// { get; set; }
    //private Player player;// { get; set; }
    //private CameraWork cameraWork;

    [SerializeField]
    private GameObject playerUiPrefab = null;
    // private PhotonView PV;

    //public GameObject _uiGo;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
      //  player = PhotonNetwork.LocalPlayer;

        //photonPlayerListingMenu = FindObjectOfType<PhotonPlayerListingMenu>();
        //photonGame = FindObjectOfType<PhotonGame>();


        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //cameraWork = gameObject.GetComponent<CameraWork>();
        //player.SetScore(0);
        //player.SetKills(0);
        //player.SetDeaths(0);
        //player.SetAssists(0);

        //  PV.RPC("AddPlayerListing", RpcTarget.All);//, PhotonNetwork.LocalPlayer);
    }


    private void Start()
    {
            //if (photonView.IsMine)
            //{
      CameraFollow.Instance.SetTarget(transform);
        // }
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
        if (PV.IsMine == false && PhotonNetwork.IsConnected == true) return;
        //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
        horizontal = moveJoystick.Horizontal != 0 ? Mathf.Sign(moveJoystick.Horizontal) : 0;

        lookPosition = GetLookPosition();
        //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
        //horizontal = moveJoystick.Horizontal;

        isFacingRight = lookPosition.x > transform.position.x;

        float forward = horizontal;
        if (!isFacingRight)
            forward = -forward;
        anim.SetFloat("Forward", forward, 0.2f, Time.deltaTime);

        float a = isFacingRight ? 90 : 270;
        //  Debug.Log(a);
        transform.rotation = Quaternion.AngleAxis(a, Vector3.up);

        //if (mirror)
        //{
        //    Vector3 scale = Vector3.one;

        //    scale.x = isFacingRight ? 1 : -1;
        //    transform.localScale = scale;
        //}
    }

    private Vector3 GetLookPosition()
    {
        Vector3 look = Vector3.zero;

        Vector3 aimPos = transform.position + Vector3.up * 1.3f;


        if (shootJoystick.HasInput)
        {
            look = aimPos + (Vector3)shootJoystick.Direction.normalized * 2;
        }
        else
        {
            look = aimPos;
            float x = isFacingRight ? 2 : -2;
            look.x += x;
        }


        return look;
    }

    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        if (grounded)
        {
             rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private void OnAnimatorIK(int index)
    {
        if (!PV.IsMine) return;
        anim.SetLookAtWeight(1, 1);
        anim.SetLookAtPosition(lookPosition);
    }

    private void OnDrawGizmos()
    {
        if (!PV.IsMine) return;
        Gizmos.DrawSphere(lookPosition, 0.2f);
    }

    void OnCollisionStay(Collision other)
    {
        if (!PV.IsMine) return;
        if (other.gameObject.tag == "Ground")
            grounded = true; 
            
    }
}
