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
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        player = PhotonNetwork.LocalPlayer;
        photonPlayerNetwork = PhotonPlayerNetwork.Instance;
    }

    private void Start()
    {
        if (!PV.IsMine) return;
            PV.RPC("CheckCharacterHealth", RpcTarget.AllBuffered,player);
            PV.RPC("CreatePlayerBar", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void CreatePlayerBar()
    {
        GameObject _uiGo = Instantiate(playerUiPrefab);
        _uiGo.SendMessage("SetPlayer", this, SendMessageOptions.RequireReceiver);
    }

    [PunRPC]
    private void CheckCharacterHealth(Player currentPlayer)
    {
        switch (currentPlayer.GetCharacter())
        {
            case PhotonCharacters.Character.Demoman:
                currentHp = charactersSettings[0].Health;
                break;
            case PhotonCharacters.Character.Soldier:
                currentHp = charactersSettings[1].Health;
                break;
            case PhotonCharacters.Character.Engineer:
                currentHp = charactersSettings[2].Health;
                break;
        }
    }



    [PunRPC]
    public void GetDamage(float damage, Player killer)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            if (PV.IsMine)
            {
                photonPlayerNetwork.PlayerDied(killer);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
