using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanZoom : MonoBehaviour
{
    [SerializeField] private float smoothSpeed;

    private bool isDragging;
    private Camera cam;
    private Vector3 smoothVelocity;
    private Vector3 startPosition;
    private Vector3 currentPosition;
    private Vector3 dir;
    private Vector3 finalPosition;

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

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref smoothVelocity, smoothSpeed * Time.deltaTime);
    }
}
