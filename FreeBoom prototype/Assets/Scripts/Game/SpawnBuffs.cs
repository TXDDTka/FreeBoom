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
    }
}
