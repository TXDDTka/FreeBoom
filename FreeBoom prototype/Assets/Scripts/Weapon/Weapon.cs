using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float shootDelay = 0.2f;
    [SerializeField] private float randomAngle = 5;

    private Transform shootPoint = null;
    private GameObject muzzle = null;
    private float rotationAngle = 0;

    private SpriteRenderer sr = null;
    private ShootJoystick shootJoystick = null;

    private WeaponSwitching wp = null;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        shootPoint = transform.GetChild(0);
        muzzle = transform.GetChild(1).gameObject;
        muzzle.SetActive(false);
    }

    private void OnEnable()
    {
        if (shootJoystick == null) shootJoystick = ShootJoystick.Instance;// FindObjectOfType<ShootJoystick>();

        shootJoystick.OnUpEvent += Fire;

        if (wp == null) wp = FindObjectOfType<WeaponSwitching>();
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        sr.flipY = false;

        shootJoystick.OnUpEvent -= Fire;
    }

    private void Update()
    {
        if (shootJoystick.HasInput) RotateGun();
    }

    private void RotateGun()
    {
        rotationAngle = Mathf.Atan2(shootJoystick.Vertical, shootJoystick.Horizontal) * Mathf.Rad2Deg;

        if (rotationAngle < 0) rotationAngle += 360;
        else if (rotationAngle > 360) rotationAngle -= 360;

        bool needFlip = rotationAngle > 90 && rotationAngle < 270;
        sr.flipY = needFlip;

        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    private void Fire()
    {
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        //Debug.Log($"fired from {transform.name}");
        //muzzle.SetActive(true);
        //yield return new WaitForEndOfFrame();
        //muzzle.SetActive(false);

        for (int i = 0; i < bulletCount; i++)
        {
            float a = Random.Range(-1f, 1f) * randomAngle;

            shootPoint.localRotation = Quaternion.AngleAxis(a, Vector3.forward);
            Bullet bullet = Instantiate(wp.BulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.Initialize(wp.BulletSpeed, shootPoint.right, rotationAngle);
            Destroy(bullet.gameObject, 5);

            shootPoint.localRotation = Quaternion.identity;
            yield return new WaitForSeconds(shootDelay);
            
        }
    }

    private void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Vector3 a = shootPoint.position;
            Vector3 b = shootPoint.right * 2;
            Gizmos.DrawLine(a, a + b);
        }
    }
}
