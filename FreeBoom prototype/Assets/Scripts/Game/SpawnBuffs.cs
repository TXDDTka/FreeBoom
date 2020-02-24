using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuffs : MonoBehaviourPunCallbacks
{
    public Transform firstAid;
    public Transform shield;
    public Transform potions;

    public Transform firstAidSpawnPosition;
    public Transform shieldSpawnPosition;
    public Transform potionsSpawnPosition;

    public Transform firstAidSpawnPosition2;
    public Transform shieldSpawnPosition2;
    public Transform potionsSpawnPosition2;

    public override void OnEnable()
    {
        base.OnEnable();
        
    }

    public override void OnCreatedRoom()
    {
        SpawnFirstAid();
        SpawnShield();
        SpawnPotion();
    }
    void Start()
    {
        
    }

    private void SpawnFirstAid()
    {
      BuffObject buffObject = PhotonNetwork.Instantiate(firstAid.name, firstAidSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
     // buffObject.buffType = BuffObject.BuffType.FirstAid;

        BuffObject buffObject2 = PhotonNetwork.Instantiate(firstAid.name, firstAidSpawnPosition2.position, Quaternion.identity).GetComponent<BuffObject>();
       // buffObject2.buffType = BuffObject.BuffType.FirstAid;
    }

    private void SpawnShield()
    {
        BuffObject buffObject = PhotonNetwork.Instantiate(shield.name, shieldSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
      //  buffObject.buffType = BuffObject.BuffType.Shield;

        BuffObject buffObject2 = PhotonNetwork.Instantiate(shield.name, shieldSpawnPosition2.position, Quaternion.identity).GetComponent<BuffObject>();
      //  buffObject2.buffType = BuffObject.BuffType.Shield;
    }

    private void SpawnPotion()
    {
        BuffObject buffObject = PhotonNetwork.Instantiate(potions.name, potionsSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
     //   buffObject.buffType = BuffObject.BuffType.Potions;

        BuffObject buffObject2 = PhotonNetwork.Instantiate(potions.name, potionsSpawnPosition2.position, Quaternion.identity).GetComponent<BuffObject>();
      //  buffObject2.buffType = BuffObject.BuffType.Potions;
    }
}
