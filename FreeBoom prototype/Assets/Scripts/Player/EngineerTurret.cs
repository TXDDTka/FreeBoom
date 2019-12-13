using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerTurret : MonoBehaviour
{
    private Rigidbody2D rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Update()
    {
        
    }

    public void LauchTurret(Vector3 velocity, int gravityScale)
    {
        transform.parent = null;
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.gravityScale = gravityScale;
        rb.velocity = velocity;
    }
}
