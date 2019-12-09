using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    [Tooltip("Список игроков которые заспавнены на карте")]
    private List<PlayerControllerPhoton> players = new List<PlayerControllerPhoton>();

    [Tooltip("Время последнего шага перемещения")]
    private float lastTickTime;

    public int nextPlayersTeam;
    public Transform spawnPointTeamOne;
    public Transform spawnPointTeamTwo;
    //Добавляем игроков в список
    public void AddPlayer(PlayerControllerPhoton player)
    {
        //Добавляем игрока в список с игроками
        players.Add(player);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateTeam()
    {
        if(nextPlayersTeam == 1)
        {
            nextPlayersTeam = 2;
        }
        else
        {
            nextPlayersTeam = 1;
        }
    }
}
