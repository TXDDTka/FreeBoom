﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PhotonPlayerMovement : MonoBehaviour,IPunObservable
{

    [SerializeField] private float speed = 0;
    
    [SerializeField] private LayerMask groundMask = 0;
    private float horizontal = 0f;
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool canMove = false;
    public Vector3 lookPosition = Vector3.zero;
    [HideInInspector]public float forward = 0f;
    [SerializeField] private bool isFacingRight = false;

    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;

    private Rigidbody rb = null;

    private PhotonView PV;
    private MoveJoystick moveJoystick = null;
    private PhotonPlayerShooting photonPlayerShooting;
   // private ShootJoystick shootJoystick = null;

    private Player player;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        photonPlayerShooting = GetComponent<PhotonPlayerShooting>();

        moveJoystick = MoveJoystick.Instance;
      //  shootJoystick = ShootJoystick.Instance;


        player = PhotonNetwork.LocalPlayer;
    }

    private void Start()
    {
        if (!PV.IsMine) return;

        CheckCharacterSpeed();
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

         forward = horizontal;
        if (!isFacingRight)
            forward = -forward;

        float a = isFacingRight ? 90 : 270;
        transform.rotation = Quaternion.AngleAxis(a, Vector3.up);
    }

    void FixedUpdate()
    {
        if (!PV.IsMine) return;
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundMask);

        if (isGrounded & canMove)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public Vector3 GetLookPosition()
    {
        Vector3 look = Vector3.zero;

        Vector3 aimPos = transform.position + Vector3.up * 1.3f;


        if (photonPlayerShooting.shootJoystick.HasInput && canMove)
        {
            look = aimPos + photonPlayerShooting.shootJoystick.Direction.normalized * 2;
        }
        else
        {
            look = aimPos;
            float x = isFacingRight ? 2 : -2;
            look.x += x;
        }


        return look;
    }

    public IEnumerator MakeBoom(Vector3 velocity)
    {
        isGrounded = false;
        rb.velocity = velocity;
        yield return new WaitForFixedUpdate();
        canMove = true;
    }

    private void CheckCharacterSpeed()
    {
        switch (player.GetCharacter())
        {
            case PhotonCharacters.Character.Demoman:
                speed = charactersSettings[0].Speed;
                break;
            case PhotonCharacters.Character.Soldier:
                speed = charactersSettings[1].Speed;
                break;
            case PhotonCharacters.Character.Engineer:
                speed = charactersSettings[2].Speed;
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(lookPosition);
        }
        else
        {
            lookPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
