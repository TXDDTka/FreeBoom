using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float maxHeight = 2;

    private Vector3 startPosition = Vector3.zero;
    private Vector3 finalPosition = Vector3.zero;
    private bool isLaunched = false;
    private bool isArrived = false;

    private Vector3 nextPos = Vector3.zero;
    private float x0 = 0;
    private float x1 = 0;
    private float dist = 0;

    private Animator anim = null;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isLaunched && !isArrived)
        {
            float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
            float baseY = Mathf.Lerp(startPosition.y, finalPosition.y, (nextX - x0) / dist);
            float arc = maxHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            nextPos = new Vector3(nextX, baseY + arc);

            transform.position = nextPos;
            //Debug.Log("moving");
        }

        if (!isArrived && transform.position == finalPosition)
        {
            Arrived();
        }
    }

    private void Arrived()
    {
        isArrived = true;
        anim.SetTrigger("isArrived");
        //Debug.Log("arrived");
    }

    public void LauchTurret(Vector3 startPos, Vector3 finalPos)
    {
        startPosition = startPos;
        finalPosition = finalPos;

        x0 = startPosition.x;
        x1 = finalPosition.x;
        dist = x1 - x0;

        isLaunched = true;
        transform.parent = null;
    }
}
