using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerAnimation : MonoBehaviour
{


    private Animator animator = null;
    private PlayerManager playerManager = null;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!playerManager.PV.IsMine) return;
        if (playerManager.playerMovement.isGrounded && playerManager.playerMovement.canMove)
            animator.SetFloat("Forward", playerManager.playerMovement.forward, 0.2f, Time.deltaTime);
    }

    public void OnAnimatorIK(int index)
    {
        animator.SetLookAtWeight(1, 1);
        animator.SetLookAtPosition(playerManager.playerMovement.lookPosition);
    }

}
