using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //подключаем Photon
using Photon; // подключаем cкрипты связанные с Photon


public class PlayerControllerPhoton : MonoBehaviourPunCallbacks, IPunObservable //меняем класс
{
    #region Singleton

    public static PlayerControllerPhoton Instance { get; private set; }
    public float hp = 100;
	public string team;

    private void InitializeSingleton()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    #endregion
    [SerializeField] private JoystickController moveJoystick;
    [SerializeField] private ShootJoystick shootJoystick;
    [Space]
    [SerializeField] private float speed;

    private float horizontal;
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public ShootJoystick ShootJoystick => shootJoystick;

    #region Photon
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")] //Используем это что бы узнать появился ли игрок на сцене
    public static GameObject LocalPlayerInstance;
    
    bool IsFiring;
    #endregion

    private void Awake()
    {
        InitializeSingleton();

		team = "team_1";

        // используется в GameManager.cs: мы отслеживаем экземпляр localPlayer, чтобы предотвратить создание экземпляров при синхронизации уровней
        if (photonView.IsMine)
        {
            PlayerControllerPhoton.LocalPlayerInstance = this.gameObject;
        }
        // мы помечаем как не разрушать при загрузке, чтобы экземпляр выдерживал синхронизацию уровней, что обеспечивает бесперебойную работу при загрузке уровней.
        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red>Missing</Color> CameraWork Component on playerPrefab.", this);
        }

        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {


        if (photonView.IsMine) //мы обрабатываем входные данные, только если мы являемся локальным игроком
        {

            horizontal = moveJoystick.Horizontal;
            anim.SetBool("isMoving", horizontal != 0);

            if (isFacingRight && horizontal < 0) Flip(true);
            else if (!isFacingRight && horizontal > 0) Flip(false);

            if (hp <= 0)
            {
                hp = 0;
                //hp can't less than zero
            }

            if (hp <= 0)
            {
                Die();
                //out of hp
                Debug.Log("Player Died!");
            }
        }
        else //if (photonView.IsMine == false && PhotonNetwork.IsConnected)
        {
            return; 
        }
       // else 
       // {
            //print("No connection");
                //}
    }

    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsFiring)
            {
                IsFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (IsFiring)
            {
                IsFiring = false;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * horizontal * speed;
    }

    private void Flip(bool rotatedToLeft)
    {
        isFacingRight = !isFacingRight;
        sr.flipX = rotatedToLeft;
    }
	public void OnTriggerEnter2D (Collider2D coll){
       
        if (!photonView.IsMine) // мы ничего не делаем, если не являемся локальным игроком.
        {
            return;
        }

        if (coll.tag == "bulletTeam_2"){// && team == "team_1"){ //нужно будет вернуть команду
			//team_1 shoots bulletTeam_1 ... team_2 shoots bulletTeam_2
			//bullet of your team can't hurt you
			hp -= 20;
			//enemy bullet hits player
		}
		if (coll.tag == "bulletTeam_1" && team == "team_2") {
			hp -= 20;
		}
	}
	public void Die (){
        //player dies
        GameManager.Instance.LeaveRoom(); //покидаем комнату
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Мы владелец этого игрока: отправляем другим наши данные
            stream.SendNext(IsFiring);
            stream.SendNext(hp);
        }
        else
        {
            // Network player, receive data
            this.IsFiring = (bool)stream.ReceiveNext();
            this.hp = (float)stream.ReceiveNext();
        }
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }

 /*   void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }*/



    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    public override void OnDisable()
    {
        // Always call the base to remove callbacks
        base.OnDisable();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
