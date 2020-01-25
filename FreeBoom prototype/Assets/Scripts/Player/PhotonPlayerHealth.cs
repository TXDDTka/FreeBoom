using Photon.Pun;
using Photon.Realtime;
using Unity.Collections;
using UnityEngine;



public class PhotonPlayerHealth : MonoBehaviourPun//PhotonPlayerController
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

        

        if (PV.IsMine)
        {
            PV.RPC("CheckCharacterHealth", RpcTarget.AllBuffered,player);
            PV.RPC("CreatePlayerBar", RpcTarget.AllBuffered);
        }



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
                //  maxHp = charactersSettings[0].Health;
                currentHp = charactersSettings[0].Health;
                break;
            case PhotonCharacters.Character.Soldier:
                //  maxHp = charactersSettings[1].Health;
                currentHp = charactersSettings[1].Health;
                break;
            case PhotonCharacters.Character.Engineer:
                //  maxHp = charactersSettings[2].Health;
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
            //Died(killer);
            if (PV.IsMine)
            {
                photonPlayerNetwork.PlayerDied(killer);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


    //public void Died(Player killer)
    //{
    //    if (!PV.IsMine) return;
    //    //photonPlayerNetwork.player.AddDeaths(1);
    //    photonPlayerNetwork.PlayerDied(); 
    //    PhotonNetwork.Destroy(gameObject);

    //}

    //[PunRPC]
    //public void SetData()//(float maxHealth)//(Player player)
    //{
    //    //currentHp = maxHp;
    //    //maxHp = maxHp;
    //    //return maxHp;
    //    // team = PhotonNetwork.LocalPlayer.GetTeam().ToString();
    //}

    //public void TakeDamage(int amount)
    //{
    //    Debug.LogWarning("TakeDamage");
    //    //if (!PV.IsMine) return;
    //    currentHp -= amount;
    //    // PV.RPC("ShowHp", RpcTarget.AllBuffered, amount);
    //    if (currentHp <= 0 && isAlive)
    //    {
    //        //dead
    //        Invoke("Respawn", 2);
    //        isAlive = false;
    //        //gameObject.SetActive(false);
    //    }
    //}


    //public void GetPhotonPlayerNetwork(PhotonPlayerNetwork getPhotonPlayerNetwork)
    //{
    //    photonPlayerNetwork = getPhotonPlayerNetwork;
    //    team = (Team)photonPlayerNetwork.team;
    //}


    //private void Respawn()
    //{
    //    //isAlive = true;
    //    //   currentHp = maxHp;
    //    //gameObject.SetActive(true);
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //      //  stream.SendNext(currentHp);

    //    }
    //    else
    //    {
    //      //  currentHp = (int)stream.ReceiveNext();
    //    }
    //}
}
