using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Transform shootPoint;
    private GameObject muzzle;
    private float rotationAngle;

    private SpriteRenderer sr;
    private ShootJoystick shootJoystick;

    private WeaponSwitching wp;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        shootPoint = transform.GetChild(0);
        muzzle = transform.GetChild(1).gameObject;
        muzzle.SetActive(false);
    }

    private void OnEnable()
    {
        if (shootJoystick == null) shootJoystick = FindObjectOfType<ShootJoystick>();

        shootJoystick.ShootEvent += Fire;

        if (wp == null) wp = FindObjectOfType<WeaponSwitching>();
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        sr.flipY = false;

        shootJoystick.ShootEvent -= Fire;
    }

    private void Update()
    {
        if (shootJoystick.HasInput) RotateGun();
    }

    private void RotateGun()
    {
        rotationAngle = Mathf.Atan2(shootJoystick.Vertical, shootJoystick.Horizontal) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        if (rotationAngle < 0) rotationAngle += 360;
        else if (rotationAngle > 360) rotationAngle -= 360;

        if (rotationAngle > 90 && rotationAngle < 270)
            sr.flipY = true;
        else
            sr.flipY = false;
    }

    private void Fire()
    {
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        //Debug.Log($"fired from {transform.name}");
        muzzle.SetActive(true);
        yield return new WaitForEndOfFrame();
        muzzle.SetActive(false);

        Rigidbody2D bullet = Instantiate(wp.BulletPrefab, shootPoint.position, shootPoint.rotation);
        if (rotationAngle > 90 && rotationAngle < 270) bullet.GetComponent<SpriteRenderer>().flipY = true;
        bullet.velocity = transform.right * wp.BulletSpeed;

        Destroy(bullet.gameObject, 5);
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.right * 5);
    }
}
