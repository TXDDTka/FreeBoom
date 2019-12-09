using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class PlayerUIPhoton : MonoBehaviour
    {

    [SerializeField] private Text playerNameText;
    [SerializeField] private Vector3 screenOffset = new Vector3(0f, 30f, 0f);
    public Slider playerHealthSlider;

    [SerializeField] private PlayerControllerPhoton target;

    float boxColliderHeight = 0f;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup _canvasGroup;
    Vector3 targetPosition;

    public void SetTarget(PlayerControllerPhoton _target)
        {
        if (_target == null)
        {
            Debug.LogError("<Color=Red><b>Missing</b></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        // Cache references for efficiency because we are going to reuse them.
        target = _target;
        targetTransform = target.GetComponent<Transform>();
        //targetRenderer = target.GetComponentInChildren<Renderer>();
        targetRenderer = target.GetComponent<Renderer>();


        BoxCollider2D boxCollider = target.GetComponent<BoxCollider2D>();

        // Get data from the Player that won't change during the lifetime of this Component
        if (boxCollider != null)
        {
            boxColliderHeight = boxCollider.size.y;
        }

        if (playerNameText != null)
        {
           //   playerNameText.text = target.photonView.Owner.NickName;
            //  Debug.Log("Имя " + target.photonView.Owner.NickName);
           // playerNameText.text = target.photonView.
        }

        if (playerHealthSlider != null)
        {
            playerHealthSlider.maxValue = target.playerHealthCurrent;
            playerHealthSlider.value = playerHealthSlider.maxValue;
        }
    }
    // Use this for initialization
    void Awake()
    {

        _canvasGroup = GetComponent<CanvasGroup>();
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false); 
    }

    // Update is called once per frame
    //void Update()
    //    {
    //    // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
    //    //if (target == null)
    //    //{
    //    //    Destroy(gameObject);
    //    //    return;
    //    //}

    //    //if (playerHealthSlider != null)
    //    //{
    //    //    playerHealthSlider.value = target.playerHealthCurrent;
    //    //}
    //}

    public void Destroy()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
    }


    void LateUpdate()
    {
        // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
        if (targetRenderer != null)
        {
           _canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
        }


        // #Critical
        // Follow the Target GameObject on screen.
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += boxColliderHeight;
            transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    //public void UpdateHealthSlider(float health)
    //{
    //   // playerHealthSlider.value = target.playerHealthCurrent;
    //    playerHealthSlider.value = health;
    //}
}
