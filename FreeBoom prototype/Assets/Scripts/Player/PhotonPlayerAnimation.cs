using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerAnimation : MonoBehaviourPun/*,IPunObservable*/
{
    private PhotonPlayerMovement photonPlayerMovement = null;

    private PhotonView PV = null;

    private Animator animator = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        photonPlayerMovement = GetComponent<PhotonPlayerMovement>();
    }

    void Update()
    {
        if (!PV.IsMine) return;
        if (photonPlayerMovement.isGrounded && photonPlayerMovement.canMove)
            animator.SetFloat("Forward", photonPlayerMovement.forward, 0.2f, Time.deltaTime);
    }

    public void OnAnimatorIK(int index)
    {
        animator.SetLookAtWeight(1, 1);
        animator.SetLookAtPosition(photonPlayerMovement.lookPosition);
    }

}
