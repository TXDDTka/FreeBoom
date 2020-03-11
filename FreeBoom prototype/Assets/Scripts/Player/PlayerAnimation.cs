using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{


    //private Animator animator = null;
    private PlayerManager playerManager = null;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
      //  animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!playerManager.PV.IsMine) return;
        if (playerManager.playerMovement.IsGrounded())
        {
            playerManager.animator.SetBool("isMoving", playerManager.playerMovement.horizontal != 0);
        }
    }

    //public void OnAnimatorIK(int index)
    //{
    //    animator.SetLookAtWeight(1, 1);
    //    animator.SetLookAtPosition(playerManager.playerMovement.lookPosition);
    //}

}
