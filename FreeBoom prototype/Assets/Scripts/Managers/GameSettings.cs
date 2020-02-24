using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{

    [SerializeField] private string _gameVersion = "0.0.1";
    public string GameVersion { get { return _gameVersion; } }
    [SerializeField] private string _nickName= "Player";
    public string NickName
    {
        get
        {
            int value = Environment.TickCount % 99;
            return _nickName + value.ToString();
        }
    }

}
