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
	private Text playerHealthText = null;

	[Tooltip("Здоровье игрока")]
	[SerializeField]
	private Slider playerHealthSlider = null;

	private PhotonPlayerHealth player;

	private float characterControllerHeight;

	private Transform playerTransform;

	private CanvasGroup canvasGroup;

	private Vector2 playerPosition;


	void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();

		transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

	}



	//void Update()
	//{
	//	if (player == null || player.currentHp <= 0)
	//	{
	//		Destroy(gameObject);
	//		return;
	//	}
	//	else
	//	{
	//		playerHealthSlider.value = player.currentHp;
	//		playerHealthText.text = player.currentHp.ToString();

	//	}
	//}


	public void SetHealth(float playerHealth)
	{
		playerHealthSlider.value = playerHealth;//player.currentHp;
		playerHealthText.text = playerHealth.ToString();//player.currentHp.ToString();

		//if (player.currentHp <= 0)
		//if (playerHealth <= 0)
		//{
		//	Destroy(gameObject);
		//	return;
		//}
	}

	public void DestroyBar()
	{
		Destroy(gameObject);
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

	public void SetPlayer(PhotonPlayerHealth _player)
	{

		player = _player;
		playerTransform = player.GetComponent<Transform>();
		CapsuleCollider capsuleCollider = player.GetComponent<CapsuleCollider>();
		characterControllerHeight = capsuleCollider.height;
		playerNameText.text = player.photonView.Owner.NickName;
		playerHealthText.text = player.currentHp.ToString();
		playerHealthSlider.maxValue = player.currentHp;
		playerHealthSlider.value = playerHealthSlider.maxValue;
	}







}


	