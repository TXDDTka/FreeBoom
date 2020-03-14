using Photon.Pun;
using Photon.Realtime;
using UnityEngine;


[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerBuffs))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerWeaponManager))]

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(BoxCollider2D))]

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonAnimatorView))]
[RequireComponent(typeof(PhotonRigidbody2DView))]
[RequireComponent(typeof(PhotonTransformView))]



public class PlayerManager : MonoBehaviourPunCallbacks
{
    /*[HideInInspector]*/ public PhotonView PV = null;
    /*[HideInInspector]*/ public Rigidbody2D rb = null;
    /*[HideInInspector]*/ public Animator animator = null;
    /*[HideInInspector]*/
    public BoxCollider2D boxCollider = null;
    /*[HideInInspector]*/ public SpriteRenderer spriteRenderer = null;

    /*[HideInInspector]*/ public PlayerMovement playerMovement;
    public MainWeaponShooting mainWeaponShooting = null;
    public SecondWeaponShooting secondWeaponShooting = null;
    /*[HideInInspector]*/ public PlayerWeaponManager playerWeaponManager = null;

    [HideInInspector] public CrosshairManager crosshairManager = null;
    [HideInInspector] public ChangeWeaponBar changeWeaponBar = null;
    [HideInInspector] public MoveJoystick moveJoystick = null;
    [HideInInspector] public ShootJoystick shootJoystick = null;

    public Player player = null;

    private void Awake()
    {
        //boxCollider = GetComponent<BoxCollider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
        //PV = GetComponent<PhotonView>();
        //rb = GetComponent<Rigidbody2D>();
        //playerMovement = GetComponent<PlayerMovement>();
        //playerWeaponManager = GetComponent<PlayerWeaponManager>();

        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;
        crosshairManager = CrosshairManager.Instance;
        changeWeaponBar = ChangeWeaponBar.Instance;

        player = PhotonNetwork.LocalPlayer;
    }
}
