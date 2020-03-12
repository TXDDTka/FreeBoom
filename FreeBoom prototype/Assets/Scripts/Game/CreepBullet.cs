using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBullet : MonoBehaviour
{
    private float speed = 7.5f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
