using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTeamTwoButton : MonoBehaviour {

    public int teamNumber;

    public static ChooseTeamTwoButton Instance { get; private set; }
    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    void Awake()
    {
        InitializeSingleton();
    }

}
