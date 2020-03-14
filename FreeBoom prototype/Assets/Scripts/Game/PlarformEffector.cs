using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlarformEffector : MonoBehaviour
{
    private Collider[] _colliders;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private LayerMask mask;
    private bool test;
    private void FixedUpdate()
    {
        _colliders = Physics.OverlapBox(checkPoint.position, transform.localScale, Quaternion.identity,mask);
    }
}
