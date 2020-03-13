using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootJoystick : JoystickController/*, IPointerDownHandler*/
{
    public static ShootJoystick Instance { get; private set; }

    

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public override float Horizontal => base.Horizontal;
    public override float Vertical => base.Vertical;
    public Vector3 Direction => new Vector3(Horizontal, Vertical);

    public event Action OnUpEvent;
    public event Action<bool> OnBeginDragEvent;

    protected override void Awake()
    {
        InitializeSingleton();
    }

    public void ResetPosition()
    {
        direction = Vector2.zero;
        moveableJoytick.anchoredPosition = direction;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //  //  joystickBackground.position = eventData.position;
    //    Debug.Log(eventData.position);

    //    if (smoothEnabled)
    //        InvokeJoytickRoutine(true);
    //    else
    //        canvasGroup.alpha = 1;
    //}

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(false);
        OnUpEvent?.Invoke();
        //base.OnPointerUp(eventData);
      //  if (smoothEnabled)
       // {
            InvokeJoytickRoutine(false);
       // }
      //  else
      //  {
        //    canvasGroup.alpha = 0;
            // joystickBackground.anchoredPosition = startPosition;
            //  startPosition = joystickBackground.anchoredPosition;
       // }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(true);
    }
}
