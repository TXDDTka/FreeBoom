using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootJoystick : JoystickController
{
    public override float Horizontal => base.Horizontal;
    public override float Vertical => base.Vertical;

    public event Action ShootEvent;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        ShootEvent?.Invoke();
    }
}
