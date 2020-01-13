using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class PhotonCharacters : MonoBehaviourPunCallbacks
{
    public enum Character : byte { None, Demoman, Engineer, Soldier };
    public static Dictionary<Character, List<Player>> PlayersChoosedCharacter;
    public const string CharacterPlayerProp = "Character";

    //public Text playersInReadTeam;
    //public Text playersInBlueTeam;
    //public Text spectators;
    //public Text playersInRoom;

    #region Events by Unity and Photon

    public void Start()
    {
        PlayersChoosedCharacter = new Dictionary<Character, List<Player>>();
        Array enumVals = Enum.GetValues(typeof(Character));
        foreach (var enumVal in enumVals)
        {
            PlayersChoosedCharacter[(Character)enumVal] = new List<Player>();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.Start();
    }

    /// <summary>Needed to update the team lists when joining a room.</summary>
    /// <remarks>Called by PUN. See enum MonoBehaviourPunCallbacks for an explanation.</remarks>
    public override void OnJoinedRoom()
    {

        this.UpdateCharacters();
    }

    public override void OnLeftRoom()
    {
        Start();
    }

    /// <summary>Refreshes the team lists. It could be a non-team related property change, too.</summary>
    /// <remarks>Called by PUN. See enum MonoBehaviourPunCallbacks for an explanation.</remarks>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        this.UpdateCharacters();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        this.UpdateCharacters();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        this.UpdateCharacters();
    }

    #endregion

    public void UpdateCharacters()
    {
        Array enumVals = Enum.GetValues(typeof(Character));
        foreach (var enumVal in enumVals)
        {
            PlayersChoosedCharacter[(Character)enumVal].Clear();
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player player = PhotonNetwork.PlayerList[i];
            Character playerCharacter = player.GetCharacter();
            PlayersChoosedCharacter[playerCharacter].Add(player);
        }
    }

}

public static class CharacterChange
{
    /// <summary>Extension for Player class to wrap up access to the player's custom property.</summary>
    /// <returns>PunTeam.Team.none if no team was found (yet).</returns>
    public static PhotonCharacters.Character GetCharacter(this Player player)
    {
        object characterId;
        if (player.CustomProperties.TryGetValue(PhotonCharacters.CharacterPlayerProp, out characterId))
        {
            return (PhotonCharacters.Character)characterId;
        }

        return PhotonCharacters.Character.None;
    }

    /// <summary>Switch that player's team to the one you assign.</summary>
    /// <remarks>Internally checks if this player is in that team already or not. Only team switches are actually sent.</remarks>
    /// <param name="player"></param>
    /// <param name="character"></param>
    public static void SetCharacter(this Player player, PhotonCharacters.Character character)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("ChooseCharacter was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
            return;
        }

        PhotonCharacters.Character currentCharacter= player.GetCharacter();
        if (currentCharacter != character)
        {
            player.SetCustomProperties(new Hashtable() { { PhotonCharacters.CharacterPlayerProp, (byte)character } });
        }
    }
}
