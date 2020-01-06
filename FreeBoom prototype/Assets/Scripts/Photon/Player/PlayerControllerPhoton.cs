using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //подключаем Photon
using System.IO;
using Photon.Pun.UtilityScripts;

public class PlayerControllerPhoton : MonoBehaviourPunCallbacks, IPunObservable //меняем класс
{
    #region Singleton

    //public static PlayerControllerPhoton Instance { get; private set; }


    //private void InitializeSingleton()
    //{
    //    if (Instance == null) Instance = this;
    //    else if (Instance != this) Destroy(this);
    //}

    #endregion
    [SerializeField] private JoystickController moveJoystick;
    [SerializeField] private ShootJoystick shootJoystick;
    [Space]
    [SerializeField] private float speed;

    private float horizontal;

    private Rigidbody2D rb;
    private Animator animator;
    private AnimationState[] animationState;
    private SpriteRenderer sr;

    public ShootJoystick ShootJoystick => shootJoystick;
    private PhotonView photonview;

    public const int playerHealthMax = 100;
    public int playerHealthCurrent = playerHealthMax;
    public int playerDamage;
    public string team;

    [Tooltip("UI c именем игрока")]
    public GameObject PlayerUiPrefab;
    public GameObject _uiGo;

    void Awake()
    {

        team = "team_1";

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        photonview = GetComponent<PhotonView>();
        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

       // InitializeSingleton();
    }


    void Start()
    {
         _uiGo = Instantiate(PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    void Update()
    {
        if (!photonview.IsMine) return;

            BasicMovement();

    }
    
    void BasicMovement()
    {

        horizontal = moveJoystick.Horizontal;
        animator.SetBool("isMoving", horizontal != 0);
        if (horizontal < 0) sr.flipX = true;//isFacingRight = false;
        else if (horizontal > 0) sr.flipX = false;//isFacingRight = true;
    }


    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * horizontal * speed;
    }


        public void GetDamage(int damage)
        {
      //  if (!photonview) return;
        playerHealthCurrent -= damage;
        _uiGo.GetComponent<PlayerUIPhoton>().playerHealthSlider.value = playerHealthCurrent;
        if (playerHealthCurrent <= 0)
           {
            playerHealthCurrent = 0;
               Die();
          }
    }
    public void Die (){
        //player dies
        // GameManagerPhoton.Instance.LeaveRoom(); //покидаем комнату
        print(PhotonNetwork.LocalPlayer.NickName + " умер");
        PhotonNetwork.Destroy(gameObject);
        _uiGo.GetComponent<PlayerUIPhoton>().Destroy();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Мы владелец этого игрока: отправляем другим наши данные
            stream.SendNext(sr.flipX);
           // stream.SendNext(isFacingRight);
            stream.SendNext(animator.GetBool("isMoving"));
            stream.SendNext(playerHealthCurrent);
        }
        else
        {
            // Не наш игрок,мы получаем данные от другого игрока
            //isFacingRight = (bool)stream.ReceiveNext();
            sr.flipX = (bool)stream.ReceiveNext();
            animator.SetBool("isMoving",(bool)stream.ReceiveNext());
             playerHealthCurrent = (int)stream.ReceiveNext();
        }
    }
    //   public void OnTriggerEnter2D (Collider2D coll){

    //       if (!photonView.IsMine) // мы ничего не делаем, если не являемся локальным игроком.
    //       {
    //           return;
    //       }

    //       //if (coll.tag == "bulletTeam_2"){// && team == "team_1"){ //нужно будет вернуть команду
    //       if (coll.tag == "bulletTeam_1")
    //       { 
    //           print("Попала 1");               //team_1 shoots bulletTeam_1 ... team_2 shoots bulletTeam_2
    //                                       //bullet of your team can't hurt you
    //           playerHealth -= 20;
    //		//enemy bullet hits player
    //           if(playerHealth <= 0)
    //           {
    //               Die();
    //           }
    //	}

    //else	if (coll.tag == "bulletTeam_1" && team == "team_2") {
    //           playerHealth -= 20;
    //           print("Попала 2");
    //       }
    //}
}
