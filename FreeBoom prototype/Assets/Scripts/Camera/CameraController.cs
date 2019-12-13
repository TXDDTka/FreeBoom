using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target = null;
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField, Range(0, 10)] private float cameraDistance = 3;
    [SerializeField, Range(0, 1)] private float followSpeed = 0.5f;

    private Vector3 smoothVelocity = Vector3.zero;
    private float currentZ = 0;
    private Vector3 finalPosition = Vector3.zero;

    private Camera cam = null;
    private Vector2 mousePosition = Vector2.zero;
    //private Vector3 shakeOffset;

    //private bool isShaking;
    //private float shakeMagnitude;
    //private float shakeTimeEnd;
    //private Vector3 shakeVector;

    private void Start()
    {
        cam = GetComponent<Camera>();
        finalPosition = target.position;
        currentZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        CaptureMousePos();   //find out where the mouse is
        //shakeOffset = UpdateShake();    //account for screen shake

        UpdateTargetPos();
        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref smoothVelocity, 1 - followSpeed);
    }

    private void CaptureMousePos()
    {
        mousePosition = cam.ScreenToViewportPoint(Input.mousePosition); //raw mouse pos
        mousePosition *= 2;
        mousePosition -= Vector2.one;                                   //set (0,0) of mouse to middle of screen

        //float max = 0.9f;
        //if (Mathf.Abs(mousePosition.x) > max || Mathf.Abs(mousePosition.y) > max)
        //mousePosition = mousePosition.normalized;                       //helps smooth near edges of screen
    }

    private void UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePosition * cameraDistance;               //mult mouse vector by distance scalar 
        finalPosition = target.position + mouseOffset + (Vector3)offset;    //find position as it relates to the player
        //finalPosition += shakeOffset;                                     //add the screen shake vector to the target
        finalPosition.z = currentZ;                                         //make sure camera stays at same Z coord
    }

    //private Vector3 UpdateShake()
    //{
    //    if (!isShaking || Time.time > shakeTimeEnd)
    //    {
    //        isShaking = false;              //set shaking false when the shake time is up
    //        return Vector3.zero;            //return zero so that it won't effect the target
    //    }

    //    Vector3 tempOffset = shakeVector;
    //    tempOffset *= shakeMagnitude;       //find out how far to shake, in what direction
    //    return tempOffset;
    //}

    //public void Shake(Vector3 direction, float magnitude, float length) //capture values set for where it's called
    //{
    //    isShaking = true;                   //to know whether it's shaking
    //    shakeVector = direction;            //direction to shake towards
    //    shakeMagnitude = magnitude;         //how far in that direction
    //    shakeTimeEnd = Time.time + length;  //how long to shake
    //}
}