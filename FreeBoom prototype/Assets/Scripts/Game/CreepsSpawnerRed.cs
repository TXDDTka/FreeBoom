using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepsSpawnerRed : MonoBehaviourPunCallbacks
{
    public GameObject creepRed;
    public PhotonTeams.Team botTeam = PhotonTeams.Team.Red;
    [SerializeField, Range (1f, 5f)] private float firstWave;
    [SerializeField, Range(1f, 20f)] private float regularWave;
    [SerializeField, Range(0.4f, 5f)] private float timeBetweenCreep;
    float timeFirstCreep = 1f;


    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnCreatedRoom()
    {
        Invoke("Wave", firstWave);
        timeBetweenCreep += timeFirstCreep;
    }

    private void Wave()
    {
        Invoke("SpawnCreep", timeFirstCreep);
        Invoke("SpawnCreep", timeBetweenCreep);
        Invoke("Wave", regularWave);
    }

    private void SpawnCreep()
    {
        BuffObject creepObject = PhotonNetwork.Instantiate(creepRed.name, transform.position, Quaternion.identity).GetComponent<BuffObject>();
    }
}
