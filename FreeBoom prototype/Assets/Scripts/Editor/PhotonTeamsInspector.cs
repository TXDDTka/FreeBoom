using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

[CustomEditor(typeof(PhotonTeams))]
public class PhotonTeamsInspector : Editor
{ 
		Dictionary<PhotonTeams.Team, bool> _Foldouts ;


	private PhotonTeams photonTeams;

	public void OnEnable()
	{
		photonTeams = (PhotonTeams)target;
	}
	public override void OnInspectorGUI()	
	{

		photonTeams.playersInReadTeam = (Text)EditorGUILayout.ObjectField("Players In Read Team", photonTeams.playersInReadTeam, typeof(Text), true);
		photonTeams.playersInBlueTeam = (Text)EditorGUILayout.ObjectField("Players In Blue Team", photonTeams.playersInBlueTeam, typeof(Text), true);
		photonTeams.spectators = (Text)EditorGUILayout.ObjectField("Spectators", photonTeams.spectators, typeof(Text), true);
		photonTeams.playersInGame = (Text)EditorGUILayout.ObjectField("Players In Game", photonTeams.playersInGame, typeof(Text), true);

		if (_Foldouts == null)
	{
		_Foldouts = new Dictionary<PhotonTeams.Team, bool>();
	}

	if (PhotonTeams.PlayersPerTeam != null)
	{
		foreach (KeyValuePair<PhotonTeams.Team, List<Player>> _pair in PhotonTeams.PlayersPerTeam)
		{
			if (!_Foldouts.ContainsKey(_pair.Key))
			{
				_Foldouts[_pair.Key] = true;
			}

			_Foldouts[_pair.Key] = EditorGUILayout.Foldout(_Foldouts[_pair.Key], "Team " + _pair.Key + " (" + _pair.Value.Count + ")");

			if (_Foldouts[_pair.Key])
			{
				EditorGUI.indentLevel++;
				foreach (Player _player in _pair.Value)
				{
					EditorGUILayout.LabelField("", _player.ToString() + (PhotonNetwork.LocalPlayer == _player ? " - You -" : ""));
				}
				EditorGUI.indentLevel--;
			}

		}
	}
}
	}

