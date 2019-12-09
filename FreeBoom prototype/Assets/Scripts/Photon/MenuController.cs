using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MenuController : MonoBehaviour {


    public void OnClickCharacterPick(int whichCharacter)
    {
        if(PlayerInfoPhoton.playerInfo != null)
        {
        // GameSetup.gameSetup.currentTeam = whichCharacter;

        
            PlayerInfoPhoton.playerInfo.mySelectedCharacter = whichCharacter;

        }
    }

    public void OnClickTeamPick(int whichTeam)
    {
        if (PlayerInfoPhoton.playerInfo != null)
        {
        //    PlayerInfoPhoton.playerInfo.mySelectedTeam = whichTeam;
        //
        //GameSetup.gameSetup.currentTeam = whichTeam;
        PlayerInfoPhoton.playerInfo.mySelectedTeam = whichTeam;
    }
}

    //public void OnClickCharacterPick(string whichCharacter)
    //{
    //    if (PlayerInfoPhoton.playerInfo != null)
    //    {
    //        PlayerInfoPhoton.playerInfo.mySelectedCharacter = whichCharacter;
    //        //PlayerPrefs.SetString("MyCharacter",whichCharacter);
    //        PlayerInfoPhoton.playerInfo.characterSelectetStatus = PlayerInfoPhoton.CharacterSelectetStatus.Selected;
    //    }
    //}


}
