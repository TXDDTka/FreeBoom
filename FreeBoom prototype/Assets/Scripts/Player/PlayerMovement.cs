using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour, IPunObservable
{

    private float speed = 0;

    [SerializeField] private LayerMask[] layers = null;
    public float horizontal = 0f;
   // public float forward = 0f;
    [SerializeField] private float groundCheckDistance = 0f;

    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;
    private PlayerManager playerManager = null;

    public bool roofAllowed = false;
    public bool canMove = false;
    //  public bool isMoving = false;
    [SerializeField] private float extraHeight = 0.1f;

    //   public bool isJumpedGounded = false;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (!playerManager.PV.IsMine) return;
        CheckCharacterSpeed();
    }

      void Update()
    {
        if (!playerManager.PV.IsMine) return;
        //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
        horizontal = playerManager.moveJoystick.Horizontal != 0 ? Mathf.Sign(playerManager.moveJoystick.Horizontal) : 0;
        //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
        //horizontal = moveJoystick.Horizontal;
        //isMoving = horizontal != 0;

        if (!playerManager.spriteRenderer.flipX && playerManager.shootJoystick.Horizontal < 0 || playerManager.spriteRenderer.flipX && playerManager.shootJoystick.Horizontal > 0)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (!playerManager.PV.IsMine) return;

        //isJumpedGounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundJumpMask);
        if (canMove)
        {
            if (IsGrounded())
            {
                playerManager.rb.velocity = new Vector2(horizontal * speed, playerManager.rb.velocity.y);
            }
            else if (OnRoof())
            {
                playerManager.rb.velocity = new Vector2(horizontal * speed, playerManager.rb.velocity.y);
            }
        }

    }


    private bool Flip()
    {
        playerManager.spriteRenderer.flipX = !playerManager.spriteRenderer.flipX;
        return playerManager.spriteRenderer.flipX;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerManager.boxCollider.bounds.center, playerManager.boxCollider.size, groundCheckDistance, Vector2.down, extraHeight, layers[0]);
        return raycastHit.collider != null;
    }

    public bool OnRoof()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerManager.boxCollider.bounds.center, playerManager.boxCollider.size, groundCheckDistance, Vector2.down, extraHeight, layers[2]);
        return raycastHit.collider != null;
    }

    public bool IsJumperGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerManager.boxCollider.bounds.center, playerManager.boxCollider.size, groundCheckDistance, Vector2.down, extraHeight, layers[1]);
        return raycastHit.collider != null;
    }

    public bool IsJumperGrounded()
    {
        playerManager.rb.velocity = velocity;
        Invoke("MoveAllow",1f);
    }

    private void MoveAllow()
    {
        canMove = true;
      //  playerManager.PV.RPC("RPC_MoveAllow", RpcTarget.AllBuffered,true);
    }

    //[PunRPC]
    //public void RPC_MoveAllow(bool active)
    //{
    //    canMove = active;
    //}

    private void CheckCharacterSpeed()
    {

        for(int i = 0; i < charactersSettings.charactersList.Count; i++)
        {
            var character = charactersSettings.charactersList[i];
            if(character.CharacterClass == playerManager.player.GetCharacter())
            {

                speed = character.Speed;
                return;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerManager.spriteRenderer.flipX);
            //stream.SendNext(lookPosition);
        }
        else
        {
            playerManager.spriteRenderer.flipX = (bool)stream.ReceiveNext();
            //lookPosition = (Vector3)stream.ReceiveNext();
        }
    }


}
