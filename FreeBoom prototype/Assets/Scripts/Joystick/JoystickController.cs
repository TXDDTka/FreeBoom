using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class JoystickController : MonoBehaviour,
                                           IPointerDownHandler, IPointerUpHandler, IDragHandler,
                                           IBeginDragHandler, IEndDragHandler
{
   // public bool smoothEnabled = true;
    [SerializeField] private AnimationCurve smoothCurve = null;
    [SerializeField, Range(0.1f, 5)] private float transitionDuration = 0.5f;

    public RectTransform joystickBackground = null;
    public RectTransform moveableJoytick = null;
    public CanvasGroup canvasGroup = null;
    private Vector2 delta = Vector2.zero;
    public Vector2 startPosition = Vector2.zero;
    private IEnumerator joytickRoutine = null;

    protected Vector2 direction = Vector2.zero;
    public virtual float Horizontal => direction.x;
    public virtual float Vertical => direction.y;
    public bool HasInput => direction != Vector2.zero;

    protected virtual void Awake()
    {
        //used for singleton
    }

    private void Start()
    {
        delta = joystickBackground.sizeDelta;
        startPosition = joystickBackground.anchoredPosition;

        canvasGroup = joystickBackground.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        moveableJoytick = joystickBackground.GetChild(0).GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickBackground.position = eventData.position - moveableJoytick.anchoredPosition;
       // joystickBackground.position = eventData.position;


       // if (smoothEnabled)
            InvokeJoytickRoutine(true);
      //  else
       //     canvasGroup.alpha = 1;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        direction = Vector2.zero;
        moveableJoytick.anchoredPosition = direction;

        //if (smoothEnabled)
        //{
            InvokeJoytickRoutine(false);
        //}
        //else
        //{
        //    canvasGroup.alpha = 0;
        //    joystickBackground.anchoredPosition = startPosition;
        //}
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out pos))
        {
            direction = new Vector2(pos.x / delta.x, pos.y / delta.y) * 2;
            direction.x = Mathf.Clamp(direction.x, -1, 1);
            direction.y = Mathf.Clamp(direction.y, -1, 1);

            if (direction.magnitude > 1) direction.Normalize();

            Vector2 finalPosition = new Vector2(direction.x * delta.x / 3, direction.y * delta.y / 3);
            moveableJoytick.anchoredPosition = finalPosition;
        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {

    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {

    }

    public void InvokeJoytickRoutine(bool enable)
    {
        if (joytickRoutine != null) StopCoroutine(joytickRoutine);
        joytickRoutine = JoystickRoutine(enable);
        StartCoroutine(joytickRoutine);
    }

    public IEnumerator JoystickRoutine(bool enable)
    {
        float percent = 0;
        float smoothPercent = 0;
        float speed = 1f / transitionDuration;

        float currentAlpha = canvasGroup.alpha;
        float desiredAlpha = enable ? 1 : 0;

        while (percent < 1)
        {
            percent += speed * Time.deltaTime;
            smoothPercent = smoothCurve.Evaluate(percent);
            canvasGroup.alpha = Mathf.MoveTowards(currentAlpha, desiredAlpha, smoothPercent);

            yield return null;
        }

        //if (!enable) joystickBackground.anchoredPosition = startPosition;
    }
}
