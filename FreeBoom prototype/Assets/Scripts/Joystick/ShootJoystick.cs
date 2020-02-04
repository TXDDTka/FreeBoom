using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootJoystick : JoystickController
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
        joystickBackground.anchoredPosition = direction;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(false);
        OnUpEvent?.Invoke();
        //base.OnPointerUp(eventData);
        if (smoothEnabled)
        {
            InvokeJoytickRoutine(false);
        }
        else
        {
            canvasGroup.alpha = 0;
            //joystickBackground.anchoredPosition = startPosition;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(true);
    }
}
