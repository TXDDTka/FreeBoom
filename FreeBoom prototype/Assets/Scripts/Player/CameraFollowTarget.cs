using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    private void Start()
    {
        CameraFollow.Instance.SetTarget(transform);
    }
}
