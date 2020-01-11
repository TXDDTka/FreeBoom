using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLogin : MonoBehaviourPunCallbacks
{

	private static string gameVersion = "1.0";

	[SerializeField]
	private InputField nameInputField = null;

	public void Connect()
	{
		if (string.IsNullOrEmpty(nameInputField.text))
		{
			nameInputField.text = "Player" + UnityEngine.Random.Range(1000, 9999);
		}
		PhotonNetwork.NickName = this.nameInputField.text;
		PhotonNetwork.GameVersion = PhotonLogin.gameVersion;
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Локальный игрок " + PhotonNetwork.NickName + " подключился к Master Server");
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonLoading.Load(LoadingScene.Lobby);
	}


	public override void OnCustomAuthenticationFailed(string debugMessage)
	{
		Debug.Log("Не удалось подключиться к серверу, проверьте включен ли сервер");
	}


	public void ExitGame()
	{
		Application.Quit();
	}
}
