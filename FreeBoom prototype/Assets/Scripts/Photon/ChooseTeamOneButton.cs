using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTeamOneButton : MonoBehaviour {

    public int teamNumber;

    public static ChooseTeamOneButton Instance { get; private set; }

    public GameObject player;
    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    void Awake()
    {
        
       // InitializeSingleton();
    }

    //void Start()
    //{
    //    player = PlayerPhoton.Instance.gameObject;
    //}
    //public void AddTeam()
    //{
    //    player.GetComponent<PlayerPhoton>().team = teamNumber;
    //}

}
