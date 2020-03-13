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
    //public enum Character : byte { None, Demoman, Engineer, Soldier };
  //  public WeaponData._CharacterClass сharacterClass;
    //public CharacterData._CharacterClass character;
    public static Dictionary<CharacterData._CharacterClass, List<Player>> PlayersChoosedCharacter;
    public const string CharacterPlayerProp = "Character";


    public void Start()
    {
        PlayersChoosedCharacter = new Dictionary<CharacterData._CharacterClass, List<Player>>();
        Array enumVals = Enum.GetValues(typeof(CharacterData._CharacterClass));
        foreach (var enumVal in enumVals)
        {
            PlayersChoosedCharacter[(CharacterData._CharacterClass)enumVal] = new List<Player>();
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

        UpdateCharacters();
    }

    public override void OnLeftRoom()
    {
        Start();
    }

    /// <summary>Refreshes the team lists. It could be a non-team related property change, too.</summary>
    /// <remarks>Called by PUN. See enum MonoBehaviourPunCallbacks for an explanation.</remarks>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        UpdateCharacters();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateCharacters();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateCharacters();
    }

    public void UpdateCharacters()
    {
        Array enumVals = Enum.GetValues(typeof(CharacterData._CharacterClass));
        foreach (var enumVal in enumVals)
        {
            PlayersChoosedCharacter[(CharacterData._CharacterClass)enumVal].Clear();
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player player = PhotonNetwork.PlayerList[i];
            CharacterData._CharacterClass playerCharacter = player.GetCharacter();
            PlayersChoosedCharacter[playerCharacter].Add(player);
        }
    }

}

public static class CharacterChange
{
    /// <summary>Extension for Player class to wrap up access to the player's custom property.</summary>
    /// <returns>PunTeam.Team.none if no team was found (yet).</returns>
    public static CharacterData._CharacterClass GetCharacter(this Player player)
    {
        object characterId;
        if (player.CustomProperties.TryGetValue(PhotonCharacters.CharacterPlayerProp, out characterId))
        {
            return (CharacterData._CharacterClass)characterId;
        }

        return CharacterData._CharacterClass.None;
    }

    /// <summary>Switch that player's team to the one you assign.</summary>
    /// <remarks>Internally checks if this player is in that team already or not. Only team switches are actually sent.</remarks>
    /// <param name="player"></param>
    /// <param name="character"></param>
    public static void SetCharacter(this Player player, CharacterData._CharacterClass character)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("ChooseCharacter was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
            return;
        }

        CharacterData._CharacterClass currentCharacter = player.GetCharacter();
        if (currentCharacter != character)
        {
            player.SetCustomProperties(new Hashtable() { { PhotonCharacters.CharacterPlayerProp, (byte)character } });
        }
    }
}
