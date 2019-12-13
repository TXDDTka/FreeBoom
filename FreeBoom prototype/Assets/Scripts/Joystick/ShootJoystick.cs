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

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(false);
        OnUpEvent?.Invoke();
        base.OnPointerUp(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(true);
    }

    //public override void OnEndDrag(PointerEventData eventData)
    //{
    //    //base.OnEndDrag(eventData);
    //    OnBeginDragEvent?.Invoke(false);
    //}
}
