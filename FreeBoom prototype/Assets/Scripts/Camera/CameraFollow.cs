using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Singleton

    public static CameraFollow Instance { get; private set; }

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    #endregion

    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField, Range(0, 1)] private float followSpeed = 1f;

    private Transform target = null;
    private Vector3 smoothVelocity = Vector3.zero;
    private float startZ = 0;
    private Vector3 finalPosition = Vector3.zero;

    private void Awake()
    {
        InitializeSingleton();

        startZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Movement();
        }
    }

    private void Movement()
    {
        finalPosition = target.position + (Vector3)offset;
        finalPosition.z = startZ;

        Vector3 smooth = Vector3.SmoothDamp(transform.position, finalPosition, ref smoothVelocity, 1 - followSpeed);
        transform.position = smooth;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
