using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float smoothSpeed = 1;

    private bool isDragging = false;
    private Camera cam = null;
    private Vector3 smoothVelocity = Vector3.zero;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;
    private Vector3 dir = Vector3.zero;
    private Vector3 finalPosition = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;
        finalPosition.z = cam.transform.position.z;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isDragging = true;
            startPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            currentPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            dir = startPosition - currentPosition;
            finalPosition = transform.position + dir;
        }        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref smoothVelocity, 1 - smoothSpeed);
    }
}
