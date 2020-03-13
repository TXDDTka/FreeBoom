using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class PhotonTeams : MonoBehaviourPunCallbacks
{


    public enum Team : byte { None, Red, Blue, Random , AutoChoose };
    public static Dictionary<Team, List<Player>> PlayersPerTeam;
    public const string TeamPlayerProp = "Team";

    public Text playersInReadTeam;
    public Text playersInBlueTeam;
    public Text spectators;
    public Text playersInGame;

    #region Events by Unity and Photon

    public void Start()
    {
        PlayersPerTeam = new Dictionary<Team, List<Player>>();
        Array enumVals = Enum.GetValues(typeof(Team));
        foreach (var enumVal in enumVals)
        {
            PlayersPerTeam[(Team)enumVal] = new List<Player>();
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

        this.UpdateTeams();
    }

    public override void OnLeftRoom()
    {
        Start();
    }

    /// <summary>Refreshes the team lists. It could be a non-team related property change, too.</summary>
    /// <remarks>Called by PUN. See enum MonoBehaviourPunCallbacks for an explanation.</remarks>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        this.UpdateTeams();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        this.UpdateTeams();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        this.UpdateTeams();
    }

    #endregion

    public void UpdateTeams()
    {
        Array enumVals = Enum.GetValues(typeof(Team));
        foreach (var enumVal in enumVals)
        {
            PlayersPerTeam[(Team)enumVal].Clear();
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Player player = PhotonNetwork.PlayerList[i];
            Team playerTeam = player.GetTeam();
            PlayersPerTeam[playerTeam].Add(player);
            UpdateStatisticsPanel();
        }
    }

    public void UpdateStatisticsPanel()
    {
        playersInReadTeam.text = "Players in Red Team : " + PlayersPerTeam[Team.Red].Count.ToString();
        playersInBlueTeam.text = "Players in Blue Team : " + PlayersPerTeam[Team.Blue].Count.ToString();
        spectators.text = "Spectators : " + PlayersPerTeam[Team.None].Count.ToString();
        playersInGame.text = "Players in Game : " + PhotonNetwork.PlayerList.Length;
    }
}

public static class TeamChange
{
    /// <summary>Extension for Player class to wrap up access to the player's custom property.</summary>
    /// <returns>PunTeam.Team.none if no team was found (yet).</returns>
    public static PhotonTeams.Team GetTeam(this Player player)
    {
        object teamId;
        if (player.CustomProperties.TryGetValue(PhotonTeams.TeamPlayerProp, out teamId))
        {
            return (PhotonTeams.Team)teamId;
        }

        return PhotonTeams.Team.None;
    }

    /// <summary>Switch that player's team to the one you assign.</summary>
    /// <remarks>Internally checks if this player is in that team already or not. Only team switches are actually sent.</remarks>
    /// <param name="player"></param>
    /// <param name="team"></param>
    public static void SetTeam(this Player player, PhotonTeams.Team team)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.NetworkClientState + ". Not IsConnectedAndReady.");
            return;
        }

        PhotonTeams.Team currentTeam = player.GetTeam();
        if (currentTeam != team)
        {
            player.SetCustomProperties(new Hashtable() { { PhotonTeams.TeamPlayerProp, (byte)team } });
        }
    }
}
