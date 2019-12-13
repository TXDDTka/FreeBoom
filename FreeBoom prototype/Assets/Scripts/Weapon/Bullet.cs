using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float speed, Vector3 direction, float angle)
    {
        if (angle > 90 && angle < 270) sr.flipY = true;

        rb.velocity = direction * speed;
    }
}
