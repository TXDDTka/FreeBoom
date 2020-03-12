using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepsSpawner : MonoBehaviourPunCallbacks
{
    public Transform spawnPoint;
    public GameObject creep;

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnCreatedRoom()
    {
        Invoke("Wave", 2f);
    }

    private void Wave()
    {
        Invoke("SpawnCreep", 1f);
        Invoke("SpawnCreep", 2f);
        Invoke("Wave", 10f);
    }

    private void SpawnCreep()
    {
        BuffObject creepObject = PhotonNetwork.Instantiate(creep.name, creep.transform.position, Quaternion.identity).GetComponent<BuffObject>();
    }
}
