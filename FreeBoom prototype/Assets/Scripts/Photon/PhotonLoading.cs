using Photon.Pun;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public enum LoadingScene
{
	Lobby,
	Game,
	Login
}

public class PhotonLoading : MonoBehaviourPunCallbacks
{	

	//public float timer;
	private static LoadingScene _nextScene { get; set; }

	public Scrollbar progressBar;

	public Text progressText;

	private void Start()
	{
		if (_nextScene == LoadingScene.Lobby)
		{
			PhotonNetwork.LoadLevel(PhotonConfig.SceneLobby);
			StartCoroutine(LoadingProgress());
		}
		else if (_nextScene == LoadingScene.Game)
		{
			PhotonNetwork.LoadLevel(PhotonConfig.SceneGame);
			StartCoroutine(LoadingProgress());
		}
		else 
		{
			PhotonNetwork.LoadLevel(PhotonConfig.SceneLogin);
			//StartCoroutine(LoadingProgress());
		}
	}

	public static void Load(LoadingScene nextScene)
	{
		_nextScene = nextScene;
		PhotonNetwork.LoadLevel(PhotonConfig.SceneLoading);
	}


	private IEnumerator LoadingProgress()
	{
		while(PhotonNetwork.LevelLoadingProgress < 1)
		{
			//timer = Time.deltaTime;
			float progress = PhotonNetwork.LevelLoadingProgress / 0.9f;
			progressBar.size = progress;
			progressText.text = string.Format("{0:0}%", progress * 100f);
			//Debug.Log(timer);
			yield return null;
		}


	}
}
