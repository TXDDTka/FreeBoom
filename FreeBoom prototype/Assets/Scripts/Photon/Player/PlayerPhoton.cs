using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.UI;

public class PlayerPhoton : MonoBehaviour/*, IPunObservable*/
{

    //public static PlayerPhoton Instance { get; private set; }
    //private void InitializeSingleton()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //}

    public PhotonView photonview;
    public GameObject player;
    public int myTeam;
    public int characterValue;

    public bool teamSelected;
    public bool characterSelected;

    public int team;
   
    void Awake()
    {
       // InitializeSingleton();
        //chooseTeamButtons[0] = ChooseTeamOneButton.Instance.GetComponent<Button>();
        //chooseTeamButtons[1] = ChooseTeamTwoButton.Instance.GetComponent<Button>();

        photonview = GetComponent<PhotonView>();
        if (photonview.IsMine)
        {
            photonview.RPC("RPC_GetTeam", RpcTarget.MasterClient);
        }
    }
    void UpdateOld()
    {
        if (player == null && myTeam != 0)
        { 
            if (myTeam == 1)
            {
                int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamOneSpawnPoints.Length);
                if (photonview.IsMine)
                {
                    player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.allCharacters[PlayerInfoPhoton.playerInfo.mySelectedCharacter].name),
                    GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity, 0);
                }
                // photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
            }
            else
            {
                int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamTwoSpawnPoints.Length);
                if (photonview.IsMine)
                {
                    player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.allCharacters[PlayerInfoPhoton.playerInfo.mySelectedCharacter].name),
                        GameSetup.gameSetup.teamTwoSpawnPoints[spawnPosition].position, Quaternion.identity);
                    // photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
                }
            }
    }
        }


    // Update is called once per frame
    void Update()
    {

        // if(GameSetup.gameSetup.currentTeam != 0 && !teamSelected)
        if (photonview.IsMine)
        {
            //if (PlayerInfoPhoton.playerInfo.mySelectedTeam != 0 && !teamSelected)
            ////    if (team != 0 && !teamSelected)
            //    {
                
            
            //    photonview.RPC("RPC_GetTeam", RpcTarget.MasterClient);//Отправляем RPC функцию мастер клиенту(хосту)
            ////    //RPC_GetTeam();
            ////    teamSelected = true;
            //}
                    
            

            if (PlayerInfoPhoton.playerInfo.mySelectedCharacter != 0 && !characterSelected)
            {
                   // photonview.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfoPhoton.playerInfo.mySelectedCharacter);
                    characterSelected = true;
            }
        }

        if (player == null && myTeam != 0)// && characterValue != 0)
        {
            if (myTeam == 1)
            {
                int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamOneSpawnPoints.Length);
                if (photonview.IsMine)
                {
                    if (characterSelected)
                    {
                         player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.allCharacters[PlayerInfoPhoton.playerInfo.mySelectedCharacter].name),
                         GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity, 0);
                        //photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
                    }
                }
            }
            else if(myTeam == 2)
            {
                int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamTwoSpawnPoints.Length);
                if (photonview.IsMine)
                {
                    if (characterSelected)
                    {
                        player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.allCharacters[PlayerInfoPhoton.playerInfo.mySelectedCharacter].name),
                        GameSetup.gameSetup.teamTwoSpawnPoints[spawnPosition].position, Quaternion.identity);
                        //photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
                    }
                }
            }
        }
    }

    //[PunRPC]
    //void ChangeTeam()
    //{
    //    GameSetup.gameSetup.CheckTeam();
    //}

    //[PunRPC]
    //void RPC_AddCharacter(int whichCharacter)
    //{
    //    characterValue = whichCharacter;
    //}

    [PunRPC]
    void RPC_GetTeam()//Локальный игрок передает значение хосту,о том к какой команде он принадлежит
    {
       // teamSelected = true;
        //myTeam = PlayerInfoPhoton.playerInfo.mySelectedTeam;
        //GameSetup.gameSetup.CheckTeam(PlayerInfoPhoton.playerInfo.mySelectedTeam);
      //  myTeam = GameSetup.gameSetup.currentTeam;
        //GameSetup.gameSetup.currentTeam = 0;
        //photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
        myTeam = GameSetup.gameSetup.nextPlayersTeam;
         GameSetup.gameSetup.UpdateTeam();
        //photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
        photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;//Устанавливаем значение переменной myTeam равным значению переданному по сети 
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        // Мы владелец этого игрока: отправляем другим наши данные
    //        stream.SendNext(teamSelected);
    //        // stream.SendNext(isFacingRight);

    //    }
    //    else
    //    {
    //        // Не наш игрок,мы получаем данные от другого игрока
    //        //isFacingRight = (bool)stream.ReceiveNext();
    //        teamSelected = (bool)stream.ReceiveNext();
    //    }
    //}
}
