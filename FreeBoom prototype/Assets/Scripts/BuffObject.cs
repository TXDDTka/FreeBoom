using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffObject : MonoBehaviour
{

    public enum BuffType {None, FirstAid, Shield , Potions}
    // Start is called before the first frame update

    public BuffType buffType = BuffType.None;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void CheckBuffType()
    //{
    //    switch (buffType)
    //    {
    //        case BuffType.FirstAid:
    //            break;
    //    }

    //}

     void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (buffType)
            {
                case BuffType.FirstAid:
                    other.gameObject.GetComponent<PhotonView>().RPC("GetBuffs", RpcTarget.AllViaServer, PhotonPlayerBuffs.Buff.FirstAid);
                    break;
                case BuffType.Shield:
                    other.gameObject.GetComponent<PhotonView>().RPC("GetBuffs", RpcTarget.AllViaServer, PhotonPlayerBuffs.Buff.Shield);
                    break;
                case BuffType.Potions:
                    other.gameObject.GetComponent<PhotonView>().RPC("GetBuffs", RpcTarget.AllViaServer, PhotonPlayerBuffs.Buff.Potions);
                    break;
            }
            PhotonNetwork.Destroy(gameObject);
            }
        //else
        //{
        //    Debug.Log("Не игрок");
        //}
        //PhotonNetwork.Destroy(gameObject);
    }

    
    }


