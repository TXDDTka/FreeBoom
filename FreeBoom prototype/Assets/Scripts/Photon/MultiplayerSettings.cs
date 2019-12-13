using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour {

    public static MultiplayerSettings multiplayerSettings;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if (multiplayerSettings == null)
        {
            multiplayerSettings = this;
        }

        else
        {
            if (multiplayerSettings != this)
            {
                Destroy(gameObject);
            }
                
        }
        DontDestroyOnLoad(gameObject);
    }     
}
