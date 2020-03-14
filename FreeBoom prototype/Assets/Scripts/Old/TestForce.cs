using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour
{
    public float speed = 0f;
    public float magnitude = 0f;
    [Header("3D")]
    public Transform transform3d;
    public Rigidbody rigidbody3d;

    [Header("2D")]
    public Transform transform2d;
    public Rigidbody2D rigidbody2d;
    void Start()
    {
        
        // rigidbody2d.AddForce(transform3d.right * 500);
      //  rigidbody2d.AddRelativeForce(transform3d.right * 500);
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2d.velocity = new Vector2(2.0f, 0);
        rigidbody3d.AddForce(transform3d.right * magnitude);
    }
}
