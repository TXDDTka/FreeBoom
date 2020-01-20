using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : MonoBehaviourPunCallbacks, IPunObservable
{

    public static PhotonPlayerController Instance { get; private set; }

    [Header("General Properties")]
    [SerializeField] private float speed = 0f;
    [SerializeField] private LayerMask groundMask = 0;
    [SerializeField, Range(0, 1)] private float groundCheck = 1f;
    [SerializeField] private bool mirror = false;

    private float horizontal = 0f;
    public bool isFacingRight = true;
    [SerializeField] private bool isGrounded = false;
    public float timer = 0f;
    public bool canMove = false;

    private Vector3 lookPosition = Vector3.zero;

    public float health = 100f;


    private MoveJoystick moveJoystick = null;
    private ShootJoystick shootJoystick = null;

    private Rigidbody rb = null;
    private Animator anim = null;

    private PhotonView PV;

    [SerializeField]
    private GameObject playerUiPrefab = null;

  //  public static bool isFlying = false;
    public Boom boom;

    public bool send = false;

    public bool jumped = false;

    public bool isFlying = false;

    private void Awake()
    {
        if (photonView.IsMine)
            Instance = this;


        PV = GetComponent<PhotonView>();

        boom = Boom.Instance;

        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();


    }


    private void Start()
    {
        if (photonView.IsMine)
        {
        CameraFollow.Instance.SetTarget(transform);
            //if((byte)PhotonNetwork.LocalPlayer.GetTeam() == 1)
            //boom.Activate(true, transform,30);
            //else if ((byte)PhotonNetwork.LocalPlayer.GetTeam() == 2)
            //boom.Activate(true, transform, -30);

            //boom.jumpButton.onClick.AddListener(() => { boom.Activate(false, null, 0); MakeBoom(boom.finalVelocity, true); timer = 1; });


            // distToGround = GetComponent<Collider>().bounds.extents.y;
        }

              

        GameObject _uiGo = Instantiate(playerUiPrefab);
        _uiGo.SendMessage("SetPlayer", this, SendMessageOptions.RequireReceiver);

        
}


    void Update()
    {
        if (!PV.IsMine) return;
 
        //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
        horizontal = moveJoystick.Horizontal != 0 ? Mathf.Sign(moveJoystick.Horizontal) : 0;

        lookPosition = GetLookPosition();
        //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
        //horizontal = moveJoystick.Horizontal;

        isFacingRight = lookPosition.x > transform.position.x;

        float forward = horizontal;
        if (!isFacingRight)
            forward = -forward;
        if (isGrounded && canMove)
            anim.SetFloat("Forward", forward, 0.2f, Time.deltaTime);

        float a = isFacingRight ? 90 : 270;
        //  Debug.Log(a);
        transform.rotation = Quaternion.AngleAxis(a, Vector3.up);

        if (mirror)
        {
            Vector3 scale = Vector3.one;

            scale.x = isFacingRight ? 1 : -1;
            transform.localScale = scale;
        }


    }

    void FixedUpdate()
    {
        if (!PV.IsMine) return;
            isGrounded = Physics.CheckSphere(transform.position, groundCheck, groundMask);

        if (isGrounded & canMove)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }


    public IEnumerator MakeBoom(Vector3 velocity)
    {
            isGrounded = false;
            rb.velocity = velocity;
            yield return new WaitForFixedUpdate();
            canMove = true;
    }

    public Vector3 GetLookPosition()
    {
        Vector3 look = Vector3.zero;

        Vector3 aimPos = transform.position + Vector3.up * 1.3f;


        if (shootJoystick.HasInput && canMove)
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



    public void OnAnimatorIK(int index)
    {
        //if (PV.IsMine)
        //{

        if (!PV.IsMine) return;

        anim.SetLookAtWeight(1, 1);
        anim.SetLookAtPosition(lookPosition);
     //   PV.RPC("Test", RpcTarget.Others, lookPosition);
      // }
        //else
        //{
        //    anim.SetLookAtWeight(1, 1);
        //    anim.SetLookAtPosition(lookPosition);
        //}
    }

    //[PunRPC]
    //public void Test(Vector3 vector)
    //{
    //    anim.logWarnings = false;
    //    anim.SetLookAtWeight(1, 1);
    //    anim.SetLookAtPosition(lookPosition);
    //}

    //[SyncVar(hook = "hookOnPositionChanged")]
    //Vector3 targetPos;
    //void hookOnPositionChanged(Vector3 pos)
    //{
    //    //set value on all clients
    //    targetPos = pos;
    //    //IK stuff here, it will be executed locally

    //}

    //private void OnDrawGizmos()
    //{
    //    if (!PV.IsMine) return;
    //    Gizmos.DrawSphere(lookPosition, 0.2f);//lookPosition visual debug
    //    Gizmos.DrawWireSphere(transform.position, groundCheck);//isGrounded visual debug
    //}

    //void OnCollisionStay(Collision other)
    //{
    //    if (!PV.IsMine) return;
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"));
    //        isGrounded = true;

    //}

    //void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) ;
    //    isGrounded = false;
    //    //canMove = false;
    //}

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) ;
    //    isGrounded = true;
    //    //canMove = true;
    //}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       // int i, b;
        if (stream.IsWriting)
        {
           // stream.SendNext(lookPosition);
            //stream.SendNext(anim.SetLookAtPosition(lookPosition));
        }
        else
        {
           // lookPosition = (Vector3)stream.ReceiveNext();
            //anim.SetLookAtPosition((Vector3)stream.ReceiveNext());
        }
    }
}
