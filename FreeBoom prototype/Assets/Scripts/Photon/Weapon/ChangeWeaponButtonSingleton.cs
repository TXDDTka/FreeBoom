using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeaponButtonSingleton : MonoBehaviour {

    public static ChangeWeaponButtonSingleton Instance { get; private set; }


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
