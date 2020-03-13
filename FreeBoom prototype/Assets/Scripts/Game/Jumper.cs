using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{


    [SerializeField] private AnimationCurve curve = null;
    [SerializeField, Range(0, 4)] private float duration = 2;
    [SerializeField, Range(0, 1)] private float amount = 0;
    private float smoothAmount = 0;
    [Space]
    public Vector3 finalVelocity = Vector3.zero;
    [SerializeField, Range(-90, 90)] private float angle = 45;//-90 = запад, 0 = север, 90 = восток
    [SerializeField, Range(1, 10)] private float launchForce = 5;

    [SerializeField] private Vector3[] positions = null;

   


    private void Calculate()
    {
        smoothAmount = curve.Evaluate(amount);
        float forceAmount = (smoothAmount + 1) * launchForce;
        finalVelocity = GetDirection().normalized * forceAmount;
        CalculateTrajectory(transform.position, finalVelocity, transform.position.y);//trajectory visual debug
    }

    private void CalculateTrajectory(Vector3 origin, Vector3 velocity, float verticalTreshold = 0, int pointsCount = 30, float delay = 0.1f)
    {
        positions = new Vector3[pointsCount];
        Vector3 gravity = Physics.gravity;

        for (int i = 0; i < positions.Length; i++)
        {
            float time = i * delay;
            Vector3 pos = origin + velocity * time + gravity * time * time / 2f;
            if (pos.y < verticalTreshold) break;
            positions[i] = pos;
        }
    }

    private Vector3 GetDirection()
    {
        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector3(x, y);
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerManager>())
        {
            PlayerManager playerManager = other.gameObject.GetComponent<PlayerManager>();
            if (playerManager.playerMovement.IsJumperGrounded())
            {
                // angle = other.GetComponent<PlayerMovement>().isFacingRight ? 30 : -30;
                angle = !playerManager.spriteRenderer.flipX ? 30 : -30;
                amount += 1 / duration * Time.deltaTime;
                amount %= 1;
                Calculate();

                playerManager.rb.velocity = finalVelocity;
            }
            }
        }
    
}
