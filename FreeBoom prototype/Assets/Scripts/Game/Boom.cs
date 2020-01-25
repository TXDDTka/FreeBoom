using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Boom : MonoBehaviour
{

    public static Boom Instance { get; private set; }

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    [SerializeField] private Image img = null;
    [SerializeField] private AnimationCurve curve = null;
    [SerializeField, Range(0, 4)] private float duration = 2;
    [SerializeField, Range(0, 1)] private float amount = 0;
    private float smoothAmount = 0;
    [Space]
    public Vector3 finalVelocity = Vector3.zero;
    [SerializeField, Range(-90, 90)] private float angle = 45;//-90 = запад, 0 = север, 90 = восток
    [SerializeField, Range(1, 10)] private float launchForce = 5;
    public Transform spawnPoint = null;
    //[Header("Debug")]
    public bool canJump = false;
    [SerializeField] private Vector3[] positions = null;

    public Button jumpButton;

    public int team = 0;
    private void Awake()
    {
        InitializeSingleton();
    }

    private void FixedUpdate()
    {

            if (canJump)
            {
                amount += 1 / duration * Time.deltaTime;
                amount %= 1;
                Calculate();
                
            }

    }

    public void Activate(Transform playerPosition, int newAngle)
    {
        angle = newAngle;
        amount = 0;
        spawnPoint = playerPosition;
        //bar.SetActive(isStart);
        canJump = true;
        img.transform.parent.gameObject.SetActive(true);
        jumpButton.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        canJump = false;
        img.transform.parent.gameObject.SetActive(false);
        jumpButton.gameObject.SetActive(false);
    }

    private void Calculate()
    {
        smoothAmount = curve.Evaluate(amount);
        img.fillAmount = smoothAmount;
        float forceAmount = (smoothAmount + 1) * launchForce;
        finalVelocity = GetDirection().normalized * forceAmount;
        CalculateTrajectory(spawnPoint.position, finalVelocity, spawnPoint.position.y);//trajectory visual debug
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
}
