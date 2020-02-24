using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{


	[SerializeField] private Text nameText = null;
	[SerializeField] private Text classText = null; 
	[SerializeField] private Text killsText = null;
	[SerializeField] private Text deathsText = null;
	[SerializeField] private Text assistsText = null;
	[SerializeField] private Text scoreText = null;

	public Player Player { get; private set; }

	public void SetPlayerInfo(Player player)
	{
		Player = player;
		nameText.text = player.NickName;

		killsText.text = player.GetKills().ToString();
		deathsText.text = player.GetDeaths().ToString();
		assistsText.text = player.GetAssists().ToString();
		scoreText.text = player.GetScore().ToString();
	}

	public void UpdatePlayerClass(Player player)
	{
		Player = player;
		classText.text = player.GetCharacter().ToString();
	}

	public void UpdatePlayerInfo(Player player)
	{
		Player = player;
		killsText.text = player.GetKills().ToString();
		deathsText.text = player.GetDeaths().ToString();
		assistsText.text = player.GetAssists().ToString();
		scoreText.text = player.GetScore().ToString();
	}
}
