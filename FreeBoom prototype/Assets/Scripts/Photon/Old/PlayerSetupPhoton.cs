using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetupPhoton : MonoBehaviour {

    private PhotonView photonview;
    public int characterValue;
    public GameObject myCharacter;
    //public bool characterSelected;

    void Start()
    {
        photonview = GetComponent<PhotonView>();
        if (photonview.IsMine)
            {
           // photonview.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfoPhoton.playerInfo.mySelectedCharacter);
            }

    }

    //void Update()
    //{
    //    if (PlayerInfoPhoton.playerInfo.characterSelectetStatus == PlayerInfoPhoton.CharacterSelectetStatus.Selected && !characterSelected)
    //    {
    //        if (photonview.IsMine)
    //        {
    //            photonview.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfoPhoton.playerInfo.mySelectedCharacter);
    //            characterSelected = true;
    //        }
    //    }
    //}

    //[PunRPC]
    //void RPC_AddCharacter(int whichCharacter)
    //{
    //    characterValue = whichCharacter;
    //    myCharacter = Instantiate(PlayerInfoPhoton.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    //    Debug.Log("Персонаж " + characterValue + " выбран");
    //}
}
