using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInstantiatePlayer : MonoBehaviourPunCallbacks
{

    public GameObject photonNetworkPlayer;
    public PhotonView PV;
    // Start is called before the first frame update

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void OnJoinedRoom()
    {

        //PV.RPC("CreatePlayer",RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    // Update is called once per frame
    void CreatePlayer()
    {
       // PhotonNetwork.Instantiate(photonNetworkPlayer.name, transform.position, Quaternion.identity, 0, null);
    }
}
