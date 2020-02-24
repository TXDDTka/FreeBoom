using System;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonPlayerStatistics : MonoBehaviour
{

    public const string PlayerKillsProp = "Kills";

    public const string PlayerDeathsProp = "Deaths";

    public const string PlayerAssistsProp = "Assists";

    public const string PlayerScoreProp = "Score";

}

public static class KillsExtensions
{
    public static void SetKills(this Player player, int newKills)
    {

        //player.SetCustomProperties(new Hashtable 
        //{ "Kills", newKills },);

        player.SetCustomProperties(new Hashtable{{PhotonPlayerStatistics.PlayerKillsProp, newKills},});
    }

    public static void AddKills(this Player player, int killsToAddToCurrent)
    {
        int current = player.GetKills();
        current = current + killsToAddToCurrent;

        //Hashtable kills = new Hashtable();  // using PUN's implementation of Hashtable
        //kills[PunPlayerScores.PlayerScoreProp] = current;

        //player.SetCustomProperties(kills);  // this locally sets the score and will sync it in-game asap.

        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerKillsProp, current }, });
    }

    public static int GetKills(this Player player)
    {
        object kills;
        if (player.CustomProperties.TryGetValue(PhotonPlayerStatistics.PlayerKillsProp, out kills))
        {
            return (int)kills;
        }

        return 0;
    }
}

public static class DeathsExtensions
{
    public static void SetDeaths(this Player player, int newDeaths)
    {

        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerDeathsProp, newDeaths }, });
    }

    public static void AddDeaths(this Player player, int deathsToAddToCurrent)
    {
        int current = player.GetDeaths();
        current = current + deathsToAddToCurrent;

        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerDeathsProp, current }, });
    }

    public static int GetDeaths(this Player player)
    {
        object deaths;
        if (player.CustomProperties.TryGetValue(PhotonPlayerStatistics.PlayerDeathsProp, out deaths))
        {
            return (int)deaths;
        }

        return 0;
    }
}

public static class AssistsExtensions
{
    public static void SetAssists(this Player player, int newAssists)
    {

        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerAssistsProp, newAssists }, });
    }

    public static void AddAssists(this Player player, int assistsToAddToCurrent)
    {
        int current = player.GetAssists();
        current = current + assistsToAddToCurrent;

        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerAssistsProp, current }, });
    }

    public static int GetAssists(this Player player)
    {
        object assists;
        if (player.CustomProperties.TryGetValue(PhotonPlayerStatistics.PlayerAssistsProp, out assists))
        {
            return (int)assists;
        }

        return 0;
    }
}

public static class ScoreExtensions
{
    public static void SetScore(this Player player, int newScore)
    {

        //Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
        //score[PunPlayerScores.PlayerScoreProp] = newScore;

        //player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.

        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerScoreProp, newScore }, });
    }

    public static void AddScore(this Player player, int scoreToAddToCurrent)
    {
        int current = player.GetScore();
        current = current + scoreToAddToCurrent;

        //Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
        //score[PunPlayerScores.PlayerScoreProp] = current;

        //player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.
        player.SetCustomProperties(new Hashtable { { PhotonPlayerStatistics.PlayerScoreProp, current }, });
    }

    public static int GetScore(this Player player)
    {
        object score;
        if (player.CustomProperties.TryGetValue(PhotonPlayerStatistics.PlayerScoreProp, out score))
        {
            return (int)score;
        }

        return 0;
    }
}
