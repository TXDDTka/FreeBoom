using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSetting : MonoBehaviour {

    public static MultiplayerSetting multiplayerSetting;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int multiplayerScene;

    private void Awake()
    {
        if (multiplayerSetting == null)
        {
            multiplayerSetting = this;
        }

        else
        {
            if (multiplayerSetting != this)
            {
                Destroy(gameObject);
            }
                
        }
        DontDestroyOnLoad(gameObject);
    }     
}
