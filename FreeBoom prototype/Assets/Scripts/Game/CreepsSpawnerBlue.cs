﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepsSpawnerBlue : MonoBehaviourPunCallbacks
{
    public GameObject creepBlue;
    public PhotonTeams.Team botTeam = PhotonTeams.Team.Blue;

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
        Invoke("Wave", 5f);
    }

    private void SpawnCreep()
    {
        BuffObject creepObject = PhotonNetwork.Instantiate(creepBlue.name, transform.position, Quaternion.identity).GetComponent<BuffObject>();
    }
}
