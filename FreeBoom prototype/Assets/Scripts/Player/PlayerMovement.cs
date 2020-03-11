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

  //  public bool isMoving = false;

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

        if (IsGrounded())
            playerManager.rb.velocity = new Vector2(horizontal * speed, playerManager.rb.velocity.y);
    }


    private bool Flip()
    {
        playerManager.spriteRenderer.flipX = !playerManager.spriteRenderer.flipX;
        return playerManager.spriteRenderer.flipX;
    }

    public bool IsGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerManager.boxCollider.bounds.center, playerManager.boxCollider.size, groundCheckDistance, Vector2.down, extraHeight, layers[0]);
        return raycastHit.collider != null;
    }

    public bool IsJumperGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerManager.boxCollider.bounds.center, playerManager.boxCollider.size, groundCheckDistance, Vector2.down, extraHeight, layers[1]);
        return raycastHit.collider != null;
    }

    public void MakeBoom(Vector3 velocity)
    {
        playerManager.rb.velocity = velocity;
    }

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
