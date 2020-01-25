﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerAnimation : MonoBehaviour
{
    private PhotonPlayerMovement _photonPlayerMovement = null;

    private PhotonView PV = null;

    private Animator anim = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        _photonPlayerMovement = GetComponent<PhotonPlayerMovement>();
    }

    void Update()
    {
        if (!PV.IsMine) return;
        if (_photonPlayerMovement.isGrounded && _photonPlayerMovement.canMove)
            anim.SetFloat("Forward", _photonPlayerMovement.forward, 0.2f, Time.deltaTime);
    }

    public void OnAnimatorIK(int index)
    {
        if (!PV.IsMine) return;

        anim.SetLookAtWeight(1, 1);
        anim.SetLookAtPosition(_photonPlayerMovement.lookPosition);
    }
}