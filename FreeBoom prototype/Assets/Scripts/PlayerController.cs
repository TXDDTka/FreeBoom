using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerController : MonoBehaviour
{
    #region Singleton

    //public static PlayerController Instance { get; private set; }

    //private void InitializeSingleton()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else if (Instance != this)
    //        Destroy(this);
    //}

    #endregion

    [Header("General Properties")]
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private Vector2 groundCheckSize = Vector2.one / 2;
    [SerializeField] private LayerMask groundLayer;

    private float horizontal = 0;
    protected bool isFacingRight = true;
    private bool isMoving = false;
    private bool previouslyMoved = false;

    private bool isGrounded = false;
    private bool previouslyGrounded = false;

    private MoveJoystick moveJoystick = null;
    protected ShootJoystick shootJoystick = null;
    private Rigidbody2D rb = null;
    private Animator anim = null;
    private SpriteRenderer sr = null;

    private ParticleSystem dustParticles = null;
    private ParticleSystem.VelocityOverLifetimeModule dustVelocity;
    private Button ultimateAbilityBtn = null;

    private void Awake()
    {
        //InitializeSingleton();
    }

    protected virtual void Start()
    {
        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        dustParticles = GetComponentInChildren<ParticleSystem>();
        dustVelocity = dustParticles.velocityOverLifetime;

        ultimateAbilityBtn = GameObject.Find("UltimateAbility Button").GetComponent<Button>();
        ultimateAbilityBtn.onClick.AddListener(() => UseUltimateAbility());
    }

    protected virtual void Update()
    {
        //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
        horizontal = moveJoystick.Horizontal != 0 ? Mathf.Sign(moveJoystick.Horizontal) : 0;

        //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
        //horizontal = moveJoystick.Horizontal;

        isMoving = horizontal != 0;
        if (!previouslyMoved && isMoving) CreateDust();
        //if (previouslyMoved && !isMoving) Debug.Log("stop");
        previouslyMoved = isMoving;

        anim.SetBool("isMoving", isMoving);

        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0) Flip();


        isGrounded = Physics2D.OverlapBox(transform.position, groundCheckSize, 0, groundLayer);
        if (!previouslyGrounded && isGrounded) CreateDust();
        previouslyGrounded = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) Jump();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        sr.flipX = !isFacingRight;

        CreateDust();
    }

    private void CreateDust()
    {
        ParticleSystem.MinMaxCurve rate = new ParticleSystem.MinMaxCurve { constantMax = isFacingRight ? -0.2f : 0.2f };
        dustVelocity.x = rate;
        dustParticles.Play();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    protected virtual void UseUltimateAbility()
    {
        //used for adding event on button
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, groundCheckSize);
    }
}
