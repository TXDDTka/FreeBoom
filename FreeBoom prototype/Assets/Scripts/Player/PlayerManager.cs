using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShooting))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerBuffs))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(BoxCollider2D))]

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonAnimatorView))]
[RequireComponent(typeof(PhotonRigidbody2DView))]
[RequireComponent(typeof(PhotonTransformView))]



public class PlayerManager : MonoBehaviourPunCallbacks
{
    [HideInInspector] public Rigidbody2D rb = null;
    [HideInInspector] public PhotonView PV = null;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerShooting playerShooting;
    [HideInInspector] public MoveJoystick moveJoystick = null;
    [HideInInspector] public ShootJoystick shootJoystick = null;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator = null;
    [HideInInspector] public BoxCollider2D boxCollider = null;
    public Player player = null;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        player = PhotonNetwork.LocalPlayer;
    }
}
