using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton

    public static PlayerController Instance { get; private set; }

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
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        horizontal = moveJoystick.Horizontal;
        anim.SetBool("isMoving", horizontal != 0);

        if (isFacingRight && horizontal < 0) Flip(true);
        else if (!isFacingRight && horizontal > 0) Flip(false);
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
}
