using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitching : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab = null;
    [SerializeField] private float bulletSpeed = 5;
    private Button changeWeaponButton = null;
    private Weapon[] weapons = null;
    private int previousIndex = 0;
    private int currentIndex = 0;

    public Bullet BulletPrefab => bulletPrefab;
    public float BulletSpeed => bulletSpeed;

    private void Start()
    {
        weapons = GetComponentsInChildren<Weapon>();

        changeWeaponButton = GameObject.Find("ChangeWeapon Button").GetComponent<Button>();
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
        if (currentIndex < weapons.Length - 1) currentIndex++;
        else currentIndex = 0;

        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (currentIndex == i)
                weapons[i].gameObject.SetActive(true);
            else
                weapons[i].gameObject.SetActive(false);
        }
    }
}
