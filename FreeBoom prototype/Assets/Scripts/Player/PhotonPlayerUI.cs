using UnityEngine;
using UnityEngine.UI;


public class PhotonPlayerUI : MonoBehaviour
{

	[Tooltip("Cмещение по y от игрока")]
	[SerializeField]
	private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

	[Tooltip("Имя игрока")]
	[SerializeField]
	private Text playerNameText = null;

	[Tooltip("Здоровье игрока")]
	[SerializeField]
	private Slider playerHealthSlider = null;

	private PhotonPlayerController player;

	private float characterControllerHeight;

	private Transform playerTransform;

	//public Renderer playerRenderer;

	private CanvasGroup canvasGroup;

	private Vector3 playerPosition;

	public Camera currentCamera;

	//public bool InMenu = true;

	void Awake()
	{
		//playerRenderer = GetComponent<Renderer>();
		canvasGroup = GetComponent<CanvasGroup>();

		//foreach (Camera c in Camera.allCameras)
		//{
		//	if (c.name == "Main Camera")
		//		currentCamera = c;
		//}
		//notMain = GameObject.Find("StatisticsGameMonitoringCamera").GetComponent<Camera>();
		//Transform canvas = GameObject.Find("Canvas");/.GetComponent<Transform>();
		//canvas.transform.FindChild

		transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
		//transform.SetParent(canvas.FindChild("GamePanel").GetComponent<Transform>(), false);

	}


	void Update()
	{
		// Destroy itself if the player is null, It's a fail safe when Photon is destroying Instances of a Player over the network
		if (player == null)
		{
			Destroy(gameObject);
			return;
		}


		// Reflect the Player Health
		if (playerHealthSlider != null)
		{
			playerHealthSlider.value = player.health;
		}
	}

	void LateUpdate()
	{

		//bool isVisibleForCamera1 = currentCamera.IsObjectVisible(playerRenderer);
		//if (isVisibleForCamera1 == true)
		//	Debug.Log("Видно");
		//else
		//	Debug.Log("неВидно");

		if(Camera.main.cullingMask == 0)
		{
			canvasGroup.alpha = 0;
		}
		else
		{
			canvasGroup.alpha = 1;
		}
		// Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
		//if (playerRenderer != null)
		//{
		//	canvasGroup.alpha =  playerRenderer.isVisible ? 1f : 0f;//если видно то 1,если нет то 0
		//	//Debug.Log(canvasGroup.alpha);
		//}

		//if (Camera.main == currentCamera)
		//{
		//	if (playerRenderer.isVisible)
		//	{
		//		canvasGroup.alpha = 1;
		//		Debug.Log("Видно");
		//	}
		//	else
		//	{
		//		canvasGroup.alpha = 0;
		//		Debug.Log("НеВидно");
		//	}
		//}

		//if (playerRenderer.IsVisibleFrom(currentCamera))
		//{
		//	canvasGroup.alpha = 1;
		//	Debug.Log("Видно");
		//}
		//else
		//{
		//	canvasGroup.alpha = 0;
		//	Debug.Log("Не Видно");
		//}
		//else Debug.Log("Not visible");

		//if(InMenu)
		//canvasGroup.alpha = InMenu ? 0f : 1f;
		//else


		// #Critical
		// Follow the player GameObject on screen.
		if (playerTransform != null)
		{
			playerPosition = playerTransform.position;
			playerPosition.y += characterControllerHeight;//screenOffset.y;//characterControllerHeight;

			transform.position = Camera.main.WorldToScreenPoint(playerPosition) + screenOffset;
		}

	}

	public void SetPlayer(PhotonPlayerController _player)
	{

		if (_player == null)
		{
			Debug.LogError("Не найден игрок", this);
			return;
		}

		// Cache references for efficiency because we are going to reuse them.
		player = _player;
		playerTransform = player.GetComponent<Transform>();
		//InMenu = player.InMenu;
		//playerRenderer = player.GetComponentInChildren<Renderer>();


		CapsuleCollider capsuleCollider = player.GetComponent<CapsuleCollider>();

		////// Get data from the Player that won't change during the lifetime of this Component
		if (capsuleCollider != null)
		{
			characterControllerHeight = capsuleCollider.height;
		}

		if (playerNameText != null)
		{
			playerNameText.text = player.photonView.Owner.NickName;
		}
	}



}


	