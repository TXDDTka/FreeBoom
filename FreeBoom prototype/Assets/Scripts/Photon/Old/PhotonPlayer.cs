using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class PhotonPlayer : MonoBehaviour {


   // public static PhotonPlayer Instance;

   // public bool characterSelected;
    //public int characterValue;

    public PhotonView photonview;
    public GameObject player;
    public int myTeam;
   // public string team;

 //   public bool playerAlive;

    //[Tooltip("UI c именем игрока")]
    //public GameObject PlayerUiPrefab;

    // Use this for initialization
    void Awake() {
      //  photonview = GetComponent<PhotonView>();
      //  InitializeSingleton();
        if (photonview.IsMine)//Ели этот объект принадлежит локальному игроку
        {
           // photonview.RPC("RPC_GetTeam", RpcTarget.MasterClient);//Отправляем RPC функцию мастер клиенту(хосту)
        }
    }

    //private void InitializeSingleton()
    //{
    //    if (Instance == null)
    //        Instance = this;
    //    else if (Instance != this)
    //        Destroy(this);
    //}


    public void CreatePlayer()
    {
        if (player == null && myTeam != 0)
        {
            if (myTeam == 1)
            {

            }
            else if (myTeam == 2)
            {

            }
            else //Если выбрана рандомная команда
            {

            }
        }
    }

    void Update()
    {

    }

    // Update is called once per frame
    void UpdateOld()
    {
        if (player == null && myTeam != 0)
        {
            if (myTeam == 1)
            {
                int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamOneSpawnPoints.Length);
                if (photonview.IsMine)
                {
                    //if (PlayerInfoPhoton.playerInfo.characterSelectetStatus == PlayerInfoPhoton.CharacterSelectetStatus.Selected && !characterSelected)
                    //{
                    //photonview.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfoPhoton.playerInfo.mySelectedCharacter);
                    player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
                     GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity, 0);
                    //  string mySelectedCharacter = PlayerInfoPhoton.playerInfo.allCharacters[PlayerInfoPhoton.playerInfo.mySelectedCharacter];
                    //player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.mySelectedCharacter),
                    // GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity);
                    //characterSelected = true;
                       // Debug.Log("Персонаж " + characterValue + " выбран");
                    //}


                    //player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerMain"),
                    // GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity, 0);

                    // player.transform.parent = gameObject.transform;
                }
            }
            else
            {
                //if (PlayerInfoPhoton.playerInfo.characterSelectetStatus == PlayerInfoPhoton.CharacterSelectetStatus.Selected && !characterSelected)
                //{
                    int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamTwoSpawnPoints.Length);
                    if (photonview.IsMine)
                    {
                    player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
                    GameSetup.gameSetup.teamTwoSpawnPoints[spawnPosition].position, Quaternion.identity, 0);
                    //player = PhotonNetwork.Instantiate(PlayerInfoPhoton.playerInfo.mySelectedCharacter,
                    //GameSetup.gameSetup.teamTwoSpawnPoints[spawnPosition].position, Quaternion.identity);
                }
                //}
            }
        }

    }

  //  public void UpdateOld()
  //  {
  //      if (!photonview.IsMine) return;
  //      if (player == null && myTeam != 0 && PlayerInfoPhoton.playerInfo.characterSelected)
  //      {
  //          print("Моя команда " + myTeam);
  //          if (myTeam == 1)
  //          {
  //              int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamOneSpawnPoints.Length);
  //              player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.mySelectedCharacter),
  //                       GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity);
  //          }
  //          else
  //          {
  //              int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamTwoSpawnPoints.Length);
  //              player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", PlayerInfoPhoton.playerInfo.mySelectedCharacter),
  //                      GameSetup.gameSetup.teamTwoSpawnPoints[spawnPosition].position, Quaternion.identity);
  //          }
  //      }
  //  }

  //public  void ChooseTeam()
  //  {
  //      if (photonview.IsMine)//Ели этот объект принадлежит локальному игроку
  //      {
  //          photonview.RPC("RPC_GetTeam", RpcTarget.MasterClient);//Отправляем RPC функцию мастер клиенту(хосту)
  //      }
  //  }


    //void Update()
    //{
    //    if (player == null && myTeam != 0)
    //    {
    //        print("Команда выбрана");
    //    }
    //}

    //[PunRPC]
    //void RPC_GetTeam()//Локальный игрок передает значение хосту,о том к какой команде он принадлежит
    //{
    //    // myTeam = GameSetup.gameSetup.currentTeam;
    //    //  GameSetup.gameSetup.UpdateTeam(myTeam);
    //    myTeam = GameSetup.gameSetup.nextPlayersTeam;
    //    GameSetup.gameSetup.UpdateTeam();
    //    photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    //}

    //[PunRPC]
    //void RPC_SentTeam(int whichTeam)
    //{
    //    myTeam = whichTeam;//Устанавливаем значение переменной myTeam равным значению переданному по сети
    //                       //  team = whichTeam;
    //}

    //[PunRPC]
    //void RPC_GetTeam()
    //{
    //    //  team = GameSetup.gameSetup.team;
    //    myTeam = GameSetup.gameSetup.nextPlayersTeam;//Переменной myTeam приравнивается переменная nextPlayersTeam из скрипта GameSetup
    //    GameSetup.gameSetup.UpdateTeam();//На мастер клиенте обновляем значение nextPlayersTeam используя UpdateTeam()
    //    //Отправляем RPC функцию назад другим клиентам от мастер клиента и передаем в переменную myTeam параметр как только другие клиенты получают RPC функцию,
    //    photonview.RPC("RPC_SentTeam", RpcTarget.OthersBuffered, myTeam);
    //}



    // [PunRPC]
    // void RPC_SentTeam(int whichTeam)

    //// void RPC_SentTeam(string whichTeam)
    // {
    //      myTeam = whichTeam;//Устанавливаем значение переменной myTeam равным значению переданному по сети
    //   //  team = whichTeam;
    // }

    /*  if (player == null && team != null)
  {
      if (team == "team_1")
      {
          int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamOneSpawnPoints.Length);
          if (photonview.IsMine)
          {
              player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
              GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity, 0);
             // player.GetComponent<PlayerControllerPhoton>().team = team;
              // player.transform.parent = gameObject.transform;

          }
      }
      else
      {
          int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamTwoSpawnPoints.Length);
          if (photonview.IsMine)
          {
              player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
              GameSetup.gameSetup.teamTwoSpawnPoints[spawnPosition].position, Quaternion.identity, 0);
             // player.GetComponent<PlayerControllerPhoton>().team = team;
          }
      }
  }*/

    //void Start()
    //{
    //    //GameObject _uiGo = Instantiate(PlayerUiPrefab);
    //    //_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    //}

    //[PunRPC]
    //void RPC_AddCharacter(int whichCharacter)
    //{
    //    characterValue = whichCharacter;
    //    //player = Instantiate(PlayerInfoPhoton.playerInfo.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
    //    int spawnPosition = Random.Range(0, GameSetup.gameSetup.teamOneSpawnPoints.Length);
    //    player = Instantiate(PlayerInfoPhoton.playerInfo.allCharacters[whichCharacter],
    //     GameSetup.gameSetup.teamOneSpawnPoints[spawnPosition].position, Quaternion.identity);
    //    characterSelected = true;
    //    Debug.Log("Персонаж " + characterValue + " выбран");
    //}

    //[PunRPC]
    //void RPC_GetTeamOld()
    //{
    //  //  team = GameSetup.gameSetup.team;
    //    myTeam = GameSetup.gameSetup.nextPlayersTeam;//Переменной myTeam приравнивается переменная nextPlayersTeam из скрипта GameSetup
    //    GameSetup.gameSetup.UpdateTeam();//На мастер клиенте обновляем значение nextPlayersTeam используя UpdateTeam()
    //    //Отправляем RPC функцию назад другим клиентам от мастер клиента и передаем в переменную myTeam параметр как только другие клиенты получают RPC функцию,
    //    photonview.RPC("RPC_SentTeam",RpcTarget.OthersBuffered,myTeam);
    //}
}
