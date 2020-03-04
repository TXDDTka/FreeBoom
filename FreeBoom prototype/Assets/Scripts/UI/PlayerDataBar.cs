using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDataBar : MonoBehaviour
{

	[Tooltip("Cмещение по y от игрока")]
	[SerializeField]
	private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

	[Tooltip("Имя игрока")]
	[SerializeField]
	private Text playerNameText = null;

	[Tooltip("Здоровье игрока")]
	[SerializeField]
	private Text playerHealthText = null;

	[Tooltip("Здоровье игрока")]
	[SerializeField]
	private Slider playerHealthSlider = null;

	private float characterControllerHeight;

	private Transform playerTransform;

	private CanvasGroup canvasGroup;

	private Vector2 playerPosition;

	private BoxCollider2D playerBoxCollider;

	void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
	}



	void Update()
	{
		if (playerTransform == null || playerHealthSlider.value <= 0)
		{
			Destroy(gameObject);
			return;
		}
		//else
		//{
		//	playerHealthSlider.value = player.currentHp;
		//	playerHealthText.text = player.currentHp.ToString();

		//}
	}

	public void SetPlayerHealth(float playerHealth)
	{
		if (playerHealth > 0)
		{
			playerHealthSlider.value = playerHealth;
			playerHealthText.text = playerHealth.ToString();
		}
		else
		{
			playerHealthSlider.value = 0;
			playerHealthText.text = "0";
		}
	}

	void LateUpdate()
	{

		if (Camera.main.cullingMask == 0)
		{
			canvasGroup.alpha = 0;
		}
		else
		{
			canvasGroup.alpha = 1;
		}
		if (playerTransform != null)
		{
			playerPosition = playerTransform.position;
			playerPosition.y += characterControllerHeight;

			transform.position = Camera.main.WorldToScreenPoint(playerPosition) + screenOffset;
		}

	}

	public void SetPlayer(Transform playerTransform, BoxCollider2D boxCollider, string playerName,float playerHealth)//(PlayerHealth _player)
	{

		//player = _player;
		//playerTransform = player.GetComponent<Transform>();
		//CapsuleCollider capsuleCollider = player.GetComponent<CapsuleCollider>();
		//characterControllerHeight = capsuleCollider.height;
		//playerNameText.text = player.playerManager.photonView.Owner.NickName;
		//playerHealthText.text = player.currentHp.ToString();
		//playerHealthSlider.maxValue = player.currentHp;
		//playerHealthSlider.value = playerHealthSlider.maxValue;

		this.playerTransform = playerTransform;
		playerBoxCollider = boxCollider;
		characterControllerHeight = boxCollider.size.y;
		playerNameText.text = playerName;
		playerHealthText.text = playerHealth.ToString();
		playerHealthSlider.maxValue = playerHealth;
		playerHealthSlider.value = playerHealthSlider.maxValue;
	}







}


	