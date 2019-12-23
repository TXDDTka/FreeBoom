using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class GameSetup : MonoBehaviour
{

    public static GameSetup gameSetup;

    public int nextPlayersTeam;
    public Transform[] teamOneSpawnPoints;
    public Transform[] teamTwoSpawnPoints;

    public int playersInTeamMax;
    public int playersInTeamOne;
    public int playersInTeamTwo;
    public int currentTeam;

    //public bool teamChoosed;
    // public string team;

    //  [SerializeField] private PhotonPlayer photonPlayer;

    //void Start()
    //{
    //    photonPlayer = PhotonPlayer.Instance;
    //}

    private void OnEnable()
    {
        if (gameSetup == null)
        {
            gameSetup = this;
        }

        }

        public void DisconnectPlayer()
         {
        StartCoroutine(DisconnectAndLoad());
          }

    IEnumerator DisconnectAndLoad()
    {
        //   PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        /// while (PhotonNetwork.IsConnected)
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(MultiplayerSetting.multiplayerSetting.menuScene);
    }


    public void CheckTeam(int team)
    {
        if (team == 1 && playersInTeamOne < playersInTeamMax)
        {
            //PlayerInfoPhoton.playerInfo.mySelectedTeam = currentTeam;
            currentTeam = team;
            playersInTeamOne =+ 1;
            Debug.LogError(currentTeam);
        }
        else if(team == 2 && playersInTeamTwo < playersInTeamMax)
        {
            //PlayerInfoPhoton.playerInfo.mySelectedTeam = currentTeam;
            currentTeam = team;
            playersInTeamTwo =+ 1;
            Debug.LogError(currentTeam);
        }
        else
        {
            currentTeam = 0;
            Debug.LogError(currentTeam);
        }
    }

    public void UpdateTeam()
    {
        if (nextPlayersTeam == 1)
        {
            nextPlayersTeam = 2;
        }
        else
        {
            nextPlayersTeam = 1;
        }
    }

    //void Update()
    //{
    //    if(PlayerInfoPhoton.playerInfo.mySelectedTeam != 0 && !teamChoosed)
    //        CheckTeam();
    //}

    //public void CheckTeam()
    //{
    //    if (PlayerInfoPhoton.playerInfo.mySelectedTeam == 1 && playersInTeamOne < playersInTeamMax)
    //    {
    //        currentTeam = PlayerInfoPhoton.playerInfo.mySelectedTeam;
    //        //playersInTeamOne++;
    //    }
    //    else if(PlayerInfoPhoton.playerInfo.mySelectedTeam == 2 && playersInTeamTwo < playersInTeamMax)
    //    {
    //        currentTeam = PlayerInfoPhoton.playerInfo.mySelectedTeam;
    //        //playersInTeamTwo++;
    //    }
    //    else if(PlayerInfoPhoton.playerInfo.mySelectedTeam == 3)
    //    {
    //        int team = Random.Range(1, 2);
    //        if(team == 1 && playersInTeamOne < playersInTeamMax)
    //        {
    //            currentTeam = team;
    //           // playersInTeamOne++;
    //        }
    //        else if(team == 2 && playersInTeamOne < playersInTeamMax)
    //        {
    //            currentTeam = team;
    //           // playersInTeamTwo++;
    //        }

    //    }
    //    else
    //    {
    //        currentTeam = 0;
    //    }
    //    //photonview.RPC("RPC_GetTeam", RpcTarget.MasterClient);//Отправляем RPC функцию мастер клиенту(хосту)
    //    if(currentTeam != 0)
    //    {
    //        teamChoosed = true;
    //        photonPlayer.ChooseTeam();
    //        print("CheckTeam проверка завершена,выбрана команда " + currentTeam);
    //    }

    //}

    //public void UpdateTeam(int teamNumber)
    //{
    //    if (teamNumber == 1)
    //    {
    //        playersInTeamOne += 1;
    //        teamNumber = 0;
    //    }
    //    else if (teamNumber == 2)
    //    {
    //        playersInTeamTwo++;
    //        teamNumber = 0;
    //    }
    //}





    //public void CreatePlayer()
    //{
    //    photonPlayer.CreatePlayer();
    //}


}
