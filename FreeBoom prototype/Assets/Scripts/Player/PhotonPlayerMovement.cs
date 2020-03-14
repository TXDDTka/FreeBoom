using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class PhotonPlayerMovement : MonoBehaviourPun,IPunObservable
{

    [SerializeField] private float speed = 0;
    
    [SerializeField] private LayerMask groundMask = 0;
    private float horizontal = 0f;
    /*[HideInInspector]*/ public bool isGrounded = false;
    /*[HideInInspector]*/ public bool canMove = false;
    public Vector3 lookPosition = Vector3.zero;
    /*[HideInInspector]*/public float forward = 0f;
    public bool isFacingRight = false;

    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;

    private Rigidbody rb = null;

    private PhotonView PV;
    private MoveJoystick moveJoystick = null;
    private ShootJoystick shootJoystick = null;

    private Player player;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;


        player = PhotonNetwork.LocalPlayer;
    }

    private void Start()
    {
        if (!PV.IsMine) return;
        CheckCharacterSpeed();
        //Boom.Instance.Activate(this, direction); 
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

        float direction = isFacingRight ? 90 : 270;
        transform.rotation = Quaternion.AngleAxis(direction, Vector3.up);
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


        if (shootJoystick.HasInput && canMove)
        {
            look = aimPos + shootJoystick.Direction.normalized * 2;
        }
        else
        {
            look = aimPos;
            float x = isFacingRight ? 2 : -2;
            look.x += x;
        }


        return look;
    }


    public void MakeBoom(Vector3 velocity)
    {
        isGrounded = false;
        rb.velocity = velocity;
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
            if(character.CharacterName == player.GetCharacter().ToString())
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
            stream.SendNext(lookPosition);
        }
        else
        {
            lookPosition = (Vector3)stream.ReceiveNext();
        }
    }

}
