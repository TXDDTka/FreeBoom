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

	private Renderer playerRenderer;

	private CanvasGroup canvasGroup;

	private Vector3 playerPosition;

	void Awake()
	{

		canvasGroup = GetComponent<CanvasGroup>();

		transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
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

		// Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
		if (playerRenderer != null)
		{
			canvasGroup.alpha = playerRenderer.isVisible ? 1f : 0f;
		}

		// #Critical
		// Follow the player GameObject on screen.
		if (playerTransform != null)
		{
			playerPosition = playerTransform.position;
			playerPosition.y += characterControllerHeight;

			transform.position = Camera.main.WorldToScreenPoint(playerPosition) + screenOffset;
		}

	}

	public void SetPlayer(PhotonPlayerController _player)
	{

		if (_player == null)
		{
			Debug.LogError("<Color=Red><b>Missing</b></Color> PlayMakerManager player for PlayerUI.Setplayer.", this);
			return;
		}

		// Cache references for efficiency because we are going to reuse them.
		player = _player;
		playerTransform = this.player.GetComponent<Transform>();
		playerRenderer = this.player.GetComponentInChildren<Renderer>();


		CharacterController _characterController = player.GetComponent<CharacterController>();

		// Get data from the Player that won't change during the lifetime of this Component
		if (_characterController != null)
		{
			characterControllerHeight = _characterController.height;
		}

		if (playerNameText != null)
		{
			playerNameText.text = player.photonView.Owner.NickName;
		}
	}

}
	