using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonDemoman : PhotonPlayerController
{
    //void Start()
    //{
    //    PV.RPC("AddPlayerListing", RpcTarget.All);
    //}

    public void AddPlayerToList()
    {
        // Instantiate();

        player.SetScore(0);
        player.SetKills(0);
        player.SetDeaths(0);
        player.SetAssists(0);

        // PhotonView ph= PhotonView.Get(this);
        //photonView.RPC("ChatMessage", RpcTarget.All, "jup", "and jup.");

        PV.RPC("AddPlayerListing", RpcTarget.AllBufferedViaServer, player);
    }

    //  public PhotonView PV;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //private void Awake()
    //{
    ////    PV = GetComponent<PhotonView>();
    //}

    [PunRPC]
    void AddPlayerListing(Player player)
    {
        photonPlayerListingMenu.AddPlayerListing(player);
    }
}
