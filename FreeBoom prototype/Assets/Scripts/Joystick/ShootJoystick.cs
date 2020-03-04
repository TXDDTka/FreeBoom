using System;
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

    //Вызывается когда отпускаем стик
    public new void OnPointerDown(PointerEventData eventData)
    {
        joystickBackground.position = eventData.position - moveableJoytick.anchoredPosition;
        InvokeJoytickRoutine(true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(false); //Событие перемещение джойстиком
        OnUpEvent?.Invoke(); //События нажатия на джойстик

        InvokeJoytickRoutine(false); //Исчезновение картинки джойстика
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {

        OnBeginDragEvent?.Invoke(true);
    }

    public void ShootJoystickPointerUp()
    {

        OnBeginDragEvent?.Invoke(false);
        OnBeginDragEvent?.Invoke(false); //Событие перемещение джойстиком
        OnUpEvent?.Invoke(); //События нажатия на джойстик

        canvasGroup.alpha = 0;

        direction = Vector2.zero;
        moveableJoytick.anchoredPosition = direction;
    }

}
