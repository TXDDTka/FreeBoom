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

    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField, Range(0, 1)] private float followSpeed = 1f;

    public Transform target = null;
    private Vector3 smoothVelocity = Vector3.zero;

    private void Awake()
    {
        InitializeSingleton();
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            HandleMovement();
            
        }
    }

    private void HandleMovement()
    {
       Vector3 pos = target.position + offset;
        Vector3 smooth = Vector3.SmoothDamp(transform.position, pos, ref smoothVelocity, 1 - followSpeed);

        transform.position = smooth;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
