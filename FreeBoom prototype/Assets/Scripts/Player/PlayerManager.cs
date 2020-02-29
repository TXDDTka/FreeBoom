using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerMovement))]
//[RequireComponent(typeof(PlayerShootingTrajectory))]
[RequireComponent(typeof(PlayerShootingCrosshairs))]

public class PlayerManager : MonoBehaviourPunCallbacks
{

    public Rigidbody rb = null;
    public PhotonView PV = null;

    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;
    // public PlayerShootingTrajectory playerShootingTrajectory;

    public PlayerShootingCrosshairs playerShootingCrosshairs;

    public MoveJoystick moveJoystick = null;
    public ShootJoystick shootJoystick = null;

    public Player player = null;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        //   playerShootingTrajectory = GetComponent<PlayerShootingTrajectory>();

        playerShootingCrosshairs = GetComponent<PlayerShootingCrosshairs>();
        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;

        player = PhotonNetwork.LocalPlayer;
    }

    public void JoysticksPointerUp()
    {
      //  playerShootingTrajectory.EnablePoints(false);
        moveJoystick.MoveJoystickPointerUp();
        shootJoystick.ShootJoystickPointerUp();
    }
}
