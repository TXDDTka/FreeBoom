using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviourPunCallbacks
{
	public enum Character
	{
		None,
		Demoman,
		Enginer,
		Soldier
	}

	public Character character;

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
		byte characterNumber = (byte)player.CustomProperties["Character"];
		character = (Character)characterNumber;
		classText.text = character.ToString();
		killsText.text = player.CustomProperties["Kills"].ToString();
		deathsText.text = player.CustomProperties["Deaths"].ToString();
		assistsText.text = player.CustomProperties["Assists"].ToString();
		scoreText.text = player.CustomProperties["Score"].ToString();
	}

}
