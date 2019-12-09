using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoPhoton : MonoBehaviour {

    //public enum CharacterSelectetStatus
    //{
    //    NotSelected,
    //    Selected
    //}

    //public enum TeamSelectetStatus
    //{
    //    NotSelected,
    //    Selected
    //}

  //  public CharacterSelectetStatus characterSelectetStatus = CharacterSelectetStatus.NotSelected;
   // public TeamSelectetStatus teamSelectetStatus = TeamSelectetStatus.NotSelected;

    public static PlayerInfoPhoton playerInfo;

   // public bool characterSelected;
    public int mySelectedTeam;
    public int mySelectedCharacter;
     
    public GameObject[] allCharacters;

	// Use this for initialization
    private void OnEnable()
    {
        if (playerInfo == null)
        {
            playerInfo = this;
        }

        else
        {
            if (playerInfo != this)
            {
                Destroy(playerInfo.gameObject);
                playerInfo = this;
            }

        }
     //   DontDestroyOnLoad(gameObject);
    }

    //void Start()
    //{
    //    //if(PlayerPrefs.HasKey("MyCharacter"))
    //    //{
    //    //    mySelectedCharacter = PlayerPrefs.GetInt("MyCharacter");
    //    //}
    //    //else
    //    //{
    //    //    mySelectedCharacter = 1;
    //    //    PlayerPrefs.SetInt("MyCharacter", mySelectedCharacter);
    //    //}
    //}
}
