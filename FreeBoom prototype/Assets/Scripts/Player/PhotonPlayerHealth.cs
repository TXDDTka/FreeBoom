using Photon.Pun;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;



public class PhotonPlayerHealth : MonoBehaviourPun
{

    public  float currentHp = 0;
    [SerializeField]
    private GameObject playerUiPrefab = null;
    [SerializeField] private CharactersSettingsDatabase charactersSettings = null;
   
    private PhotonPlayerNetwork photonPlayerNetwork;
    private PhotonView PV = null;
    private Player player;
    public PhotonPlayerUI playerBar;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        player = PhotonNetwork.LocalPlayer;
        photonPlayerNetwork = PhotonPlayerNetwork.Instance;
    }

    private void Start()
    {
        if (!PV.IsMine) return;
            CheckCharacterHealth();
            PV.RPC("CreatePlayerBar", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void CreatePlayerBar()
    {
        playerBar = Instantiate(playerUiPrefab).GetComponent<PhotonPlayerUI>(); ;
        //playerBar.SendMessage("SetPlayer", this, SendMessageOptions.RequireReceiver);
        playerBar.SetPlayer(this);
    }

   // [PunRPC]
    private void CheckCharacterHealth()
    {
        for (int i = 0; i < charactersSettings.charactersList.Count; i++)
        {
            var character = charactersSettings.charactersList[i];
            if (character.CharacterName == player.GetCharacter().ToString())
            {
                PV.RPC("SetPlayerHealth", RpcTarget.AllBuffered, character.Health);
                return;
            }
        }
    }

    [PunRPC]
    public void SetPlayerHealth(float health)
    {
        currentHp = health;
    }

    [PunRPC]
    public void GetDamage(float damage, Player killer)
    {
        currentHp -= damage;

        //playerBar.SendMessage("SetHealth", SendMessageOptions.RequireReceiver);
        playerBar.SetHealth(currentHp);

        if (currentHp <= 0)
        {
            //playerBar.DestroyBar();
            //currentHp = 0;
            if (!PV.IsMine) return;  
                photonPlayerNetwork.PlayerDied(killer);
                //PhotonNetwork.Destroy(gameObject);
        }
    }

    
}
