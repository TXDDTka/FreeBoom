using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJoystick : JoystickController
{
    public static MoveJoystick Instance { get; private set; }

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    protected override void Awake()
    {
        InitializeSingleton();
    }

    public override float Horizontal => direction.x != 0 ? direction.x : Input.GetAxisRaw("Horizontal");
    public override float Vertical => direction.y != 0 ? direction.y : Input.GetAxisRaw("Vertical");

}
