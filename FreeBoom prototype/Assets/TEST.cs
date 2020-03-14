using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{

    private PlayerManager playerManager = null;
    //public Transform weaponHolder = null;

    private void Awake()
    {
        playerManager = GetComponentInParent<PlayerManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        playerManager.PV.RPC("Test", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void Test()
    {
        Debug.Log("TEST");
    }
}
