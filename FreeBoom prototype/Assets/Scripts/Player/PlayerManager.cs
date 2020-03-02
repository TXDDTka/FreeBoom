using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerShooting))]

public class PlayerManager : MonoBehaviourPunCallbacks
{

    [HideInInspector]public Rigidbody rb = null;
    [HideInInspector] public PhotonView PV = null;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerShooting playerShooting;
    [HideInInspector] public MoveJoystick moveJoystick = null;
    [HideInInspector] public ShootJoystick shootJoystick = null;

    public Player player = null;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        player = PhotonNetwork.LocalPlayer;
    }

    //public void JoysticksPointerUp()
    //{
    //  //  playerShootingTrajectory.EnablePoints(false);
    //    moveJoystick.MoveJoystickPointerUp();
    //    shootJoystick.ShootJoystickPointerUp();
    //}
}
