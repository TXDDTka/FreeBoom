using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuffs : MonoBehaviourPunCallbacks
{
    public Transform healthMin;
    public Transform healthMid;
    public Transform healthMax;
    public Transform shieldMin;
    public Transform shieldMid;
    public Transform shieldMax;
    
    public Transform healthMinSpawnPosition;
    public Transform healthMidSpawnPosition;
    public Transform healthMaxSpawnPosition;
    public Transform shieldMinSpawnPosition;
    public Transform shieldMidSpawnPosition;
    public Transform shieldMaxSpawnPosition;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnCreatedRoom()
    {
        SpawnHealthBuffs();
    }
    void Start()
    {
        
    }

    private void SpawnHealthBuffs()
    {
        BuffObject buffObject = PhotonNetwork.Instantiate(healthMin.name, healthMinSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
        BuffObject buffObject1 = PhotonNetwork.Instantiate(healthMid.name, healthMidSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
        BuffObject buffObject2 = PhotonNetwork.Instantiate(healthMax.name, healthMaxSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();

        BuffObject buffObject3 = PhotonNetwork.Instantiate(shieldMin.name, shieldMinSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
        BuffObject buffObject4 = PhotonNetwork.Instantiate(shieldMid.name, shieldMidSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
        BuffObject buffObject5 = PhotonNetwork.Instantiate(shieldMax.name, shieldMaxSpawnPosition.position, Quaternion.identity).GetComponent<BuffObject>();
    }
}
