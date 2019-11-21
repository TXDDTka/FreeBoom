using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton

    public static PlayerController Instance { get; private set; }

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

    private void Awake()
    {
        InitializeSingleton();

		team = "team_1";
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
		if (hp <= 0) {
			hp = 0;
			//hp can't less than zero
		}
        horizontal = moveJoystick.Horizontal;
        anim.SetBool("isMoving", horizontal != 0);

        if (isFacingRight && horizontal < 0) Flip(true);
        else if (!isFacingRight && horizontal > 0) Flip(false);

		if(hp <= 0){
			Die();
			//out of hp
			Debug.Log("Player Died!");
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
		if(coll.tag == "bulletTeam_2" && team == "team_1"){
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
	}
}
