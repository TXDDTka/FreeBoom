using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

[CustomEditor(typeof(PhotonCharacters))]
public class PhotonCharactersInspector : Editor
{ 
		Dictionary<CharacterData._CharacterClass, bool> _Foldouts ;


	public override void OnInspectorGUI()	
	{


		if (_Foldouts == null)
	{
		_Foldouts = new Dictionary<CharacterData._CharacterClass, bool>();
	}

	if (PhotonCharacters.PlayersChoosedCharacter != null)
	{
		foreach (KeyValuePair<CharacterData._CharacterClass, List<Player>> _pair in PhotonCharacters.PlayersChoosedCharacter)
		{
			if (!_Foldouts.ContainsKey(_pair.Key))
			{
				_Foldouts[_pair.Key] = true;
			}

			_Foldouts[_pair.Key] = EditorGUILayout.Foldout(_Foldouts[_pair.Key], "Character " + _pair.Key + " (" + _pair.Value.Count + ")");

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

