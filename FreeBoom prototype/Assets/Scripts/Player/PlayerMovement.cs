using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour, IPunObservable
{

    [SerializeField] private float speed = 0;
    
    [SerializeField] private LayerMask groundMask = 0;
    private float horizontal = 0f;
    /*[HideInInspector]*/ public bool isGrounded = false;
    /*[HideInInspector]*/ public bool canMove = false;
    public Vector3 lookPosition = Vector3.zero;
    /*[HideInInspector]*/public float forward = 0f;
    public bool isFacingRight = false;

  //  public Rigidbody rb = null;

    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;
    private PlayerManager playerManager = null;


    [SerializeField] private LayerMask groundJumpMask = 0;
    public bool isJumpedGounded = false;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (!playerManager.PV.IsMine) return;
        CheckCharacterSpeed();
    }
    //void Update()
    //{
    //    if (!playerManager.PV.IsMine) return;

    //    //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
    //    horizontal = playerManager.moveJoystick.Horizontal != 0 ? Mathf.Sign(playerManager.moveJoystick.Horizontal) : 0;

    //    //lookPosition = GetLookPosition();
    //    //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
    //    //horizontal = moveJoystick.Horizontal;

    //    isFacingRight = lookPosition.x > transform.position.x;

    //    forward = horizontal;
    //    if (!isFacingRight)
    //        forward = -forward;

    ////    float direction = isFacingRight ? 90 : 270;
    // //   transform.rotation = Quaternion.AngleAxis(direction, Vector3.up);
    //}

      void Update()
    {
        if (!playerManager.PV.IsMine) return;
        //движение с фиксированной скоростью незавизимо от расстояния между стиком и центром джойстика
        horizontal = playerManager.moveJoystick.Horizontal != 0 ? Mathf.Sign(playerManager.moveJoystick.Horizontal) : 0;
        //скорость движения зависит от расстояния между подвижным стиком и центром джойстика
        //horizontal = moveJoystick.Horizontal;

        if (!playerManager.spriteRenderer.flipX && playerManager.shootJoystick.Horizontal < 0 || playerManager.spriteRenderer.flipX && playerManager.shootJoystick.Horizontal > 0) Flip();

    }

    void FixedUpdate()
    {
        if (!playerManager.PV.IsMine) return;
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundMask);

        isJumpedGounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundJumpMask);

        // if (isGrounded & canMove)
        if (IsGrounded() & canMove)
            playerManager.rb.velocity = new Vector2(horizontal * speed, playerManager.rb.velocity.y);
    }



    public Transform CurrentWeapon()
    {
        Transform weaponTransform = playerManager.playerShooting.currentWeapon == PlayerShooting.CurrentWeapon.MainWeapon ? 
            playerManager.playerShooting.mainWeapon.weaponGameobject.transform : playerManager.playerShooting.mainWeapon.weaponGameobject.transform;
        return weaponTransform;
    }

    //public Vector3 GetLookPosition()
    //{
    //    Vector3 look = Vector3.zero;

    //    Vector3 aimPos = transform.position + Vector3.up * 1.3f;


    //    if (playerManager.shootJoystick.direction != Vector2.zero && canMove)
    //    {
    //        look = aimPos + playerManager.shootJoystick.Direction.normalized * 2;
    //    }
    //    else
    //    {
    //        look = aimPos;
    //        float x = !playerManager.spriteRenderer.flipX ? -2 : 2;//isFacingRight ? 2 : -2;
    //        look.x += x;
    //    }


    //    return look;
    //}

    private bool Flip()
    {
        playerManager.spriteRenderer.flipX = !playerManager.spriteRenderer.flipX;
        return playerManager.spriteRenderer.flipX;
    }

    private bool IsGrounded()
    {
        float extraHeight = 1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerManager.boxCollider.bounds.center, playerManager.boxCollider.size, 0f, Vector2.down, extraHeight, groundMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        { 
            rayColor = Color.red;
        }

        return raycastHit.collider != null;
}

    public void MakeBoom(Vector3 velocity)
    {
        isGrounded = false;
        playerManager.rb.velocity = velocity;
        StartCoroutine(StartMoving());
    }

    public IEnumerator StartMoving()
    {
        yield return new WaitForFixedUpdate();
        canMove = true;
    }

    private void CheckCharacterSpeed()
    {

        for(int i = 0; i < charactersSettings.charactersList.Count; i++)
        {
            var character = charactersSettings.charactersList[i];
            if(character.CharacterName == playerManager.player.GetCharacter().ToString())
            {
               
                speed = character.Speed;
                Debug.Log(speed);
                return;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(lookPosition);
        //}
        //else
        //{
        //    lookPosition = (Vector3)stream.ReceiveNext();
        //}
    }


}
