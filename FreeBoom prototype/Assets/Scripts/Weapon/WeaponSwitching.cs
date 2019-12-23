using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private Button changeWeaponButton;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private float bulletSpeed;
    private int previousIndex;
    private int currentIndex;

    public Rigidbody2D BulletPrefab => bulletPrefab;
    public float BulletSpeed => bulletSpeed;

    private void Start()
    {
        changeWeaponButton.onClick.AddListener(() => ChangeWeapon());

        SwitchWeapon();
    }

    private void Update()
    {
        if (previousIndex != currentIndex)
        {
            SwitchWeapon();
            previousIndex = currentIndex;
        }
    }

    private void ChangeWeapon()
    {
        if (currentIndex < transform.childCount - 1) currentIndex++;
        else currentIndex = 0;

        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (currentIndex == i)
                transform.GetChild(i).gameObject.SetActive(true);
            else
                transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
