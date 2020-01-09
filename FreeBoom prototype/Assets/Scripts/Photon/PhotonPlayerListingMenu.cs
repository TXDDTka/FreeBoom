using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerListingMenu : MonoBehaviourPunCallbacks
{

	[SerializeField] private Transform _contentTeamRed = null;
	[SerializeField] private Transform _contentTeamBlue = null;

	[SerializeField]
	private PlayerListing _playerListingRedTeam = null;

	[SerializeField]
	private PlayerListing _playerListingBlueTeam = null;

	[SerializeField]
	private List<PlayerListing> _listings = new List<PlayerListing>();

	public void AddPlayerListing(Player player, byte teamNumber)
	{
		int index = _listings.FindIndex(x => x.Player == player);
		if (index != -1)
		{
			_listings[index].SetPlayerInfo(player);
		}
		if (teamNumber == 1)
		{
			PlayerListing playerListing = Instantiate(_playerListingRedTeam, _contentTeamRed);
			if (playerListing != null)
			{
				playerListing.SetPlayerInfo(player);
				_listings.Add(playerListing);
			}
		}
		else
		{
			PlayerListing playerListing = Instantiate(_playerListingBlueTeam, _contentTeamBlue);
			if (playerListing != null)
			{
				playerListing.SetPlayerInfo(player);
				_listings.Add(playerListing);
			}
		}

	}

	//public void RemovePlayerListing(Player otherPlayer)
	//{
	//	int index = _listings.FindIndex(x => x.Player == otherPlayer);
	//	if (index != -1)
	//	{
	//		Destroy(_listings[index].gameObject);
	//		_listings.RemoveAt(index);
	//	}
	//	//else
	//	//{
	//	//}
	//	//foreach (PlayerListing playerList in _listings)
	//	//{
	//	//	_listings.
	//	//}
	//}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		int index = _listings.FindIndex(x => x.Player == otherPlayer);
		if (index != -1)
		{
			Destroy(_listings[index].gameObject);
			_listings.RemoveAt(index);
		}

	}
}
