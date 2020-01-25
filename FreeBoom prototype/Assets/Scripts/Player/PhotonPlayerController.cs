using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : MonoBehaviourPunCallbacks, IPunObservable
{

    //public static PhotonPlayerController Instance { get; private set; }
    public enum Team
    {
        None,
        Red,
        Blue
    }

    public enum Character
    {
        None,
        Demoman,
        Soldier,
        Engineer
    }


    

   /* [HideInInspector] */public Team team = Team.None;
    [HideInInspector] public Character character = Character.None;

    [HideInInspector] public PhotonView PV = null;
    [HideInInspector] public MoveJoystick moveJoystick = null;
    [HideInInspector] public ShootJoystick shootJoystick = null;
    [HideInInspector] public Boom boom;

    [HideInInspector] public Rigidbody rb = null;
    [HideInInspector] public Animator anim = null;
    ///*[HideInInspector]*/ public PhotonPlayerNetwork photonPlayerNetwork;// = PhotonPlayerNetwork.Instance;



    //public MasterManager masterManager;
    //public CharacterSettings characterSettings;
    //public WeaponSettings weaponSettings;
    //public CharacterSettingsDatabase characterSettingsDatabase;
    //public List<CharacterData> characterList;
    //public void Init(MasterManager _masterManager)
    //{
    //    masterManager = _masterManager;
    //}

    private void Awake()
    {

        PV = GetComponent<PhotonView>();

        moveJoystick = MoveJoystick.Instance;
        shootJoystick = ShootJoystick.Instance;
        boom = Boom.Instance;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        character = (Character)PhotonNetwork.LocalPlayer.GetCharacter();

        //photonPlayerNetwork = GetComponentInParent<PhotonPlayerNetwork>();// transform.parent.GetComponent<PhotonPlayerNetwork>();
        //photonPlayerNetwork = PhotonPlayerNetwork.Instance;

    }


    private void Start()
    {
        // if (!photonView.IsMine) return;
      //  team = (Team)photonPlayerNetwork.team;
        
        // CameraFollow.Instance.SetTarget(transform);      
    }



    //public void GetPhotonPlayerNetwork(PhotonPlayerNetwork getPhotonPlayerNetwork)
    //{
    // photonPlayerNetwork = getPhotonPlayerNetwork;
    // team = (Team)photonPlayerNetwork.team;
    //}


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(team);
            //stream.SendNext(anim.SetLookAtPosition(lookPosition));
        }
        else
        {
            //team = (Team)stream.ReceiveNext();
            //anim.SetLookAtPosition((Vector3)stream.ReceiveNext());
        }
    }
}
