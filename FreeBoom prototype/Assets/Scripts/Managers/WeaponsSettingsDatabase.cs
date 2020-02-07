using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Database/Weapons", fileName = "Weapons")]
public class WeaponsSettingsDatabase : ScriptableObject
{
    [/*SerializeField,*/ HideInInspector] public List<WeaponData> weaponsList; //Лист где хранятся данны, SerializeField сохраняет данные после выхода из редактора, HideInInspector что бы не отображало List в Editor

    [SerializeField] private WeaponData currentWeapon; //Текущий отображаемый элемент

    private int currentIndex = 0; //Что бы было удобней следить за номером текущего элемента

    //Метод добавления в лист нового пустого элемента и удаление текущего. 
    public void AddElement()
    {
        if (weaponsList == null) // Необходимо будет проверить наличие листа в памяти что бы его инициализировать
            weaponsList = new List<WeaponData>();

        currentWeapon = new WeaponData(); // в currentWeapon помещаем новый объект нашего класса
        weaponsList.Add(currentWeapon); // добавляем в список персонажей
        currentIndex = weaponsList.Count - 1; // обновляем индекс
    }

    //Удаление элемента. Вместо создания нового экземляра, подсовываем предыдущий объект, если он есть.
    public void RemoveCurrentElement()
    {
        if (currentIndex > 0)
        {
            currentWeapon = weaponsList[--currentIndex];
            weaponsList.RemoveAt(++currentIndex);
        }
        else
        {
            weaponsList.Clear();
            currentWeapon = null;
        }
    }

    //Метод переключающий элемент на следующий
    public WeaponData GetNext()
    {
        if (currentIndex < weaponsList.Count - 1)
            currentIndex++;
        currentWeapon = this[currentIndex];
        return currentWeapon;
    }

    //Метод переключающий элемент на предыдущий
    public WeaponData GetPrev()
    {
        if (currentIndex > 0)
            currentIndex--;
        currentWeapon = this[currentIndex];
        return currentWeapon;
    }

    //Метод очищения базы данных
    public void ClearDatabase()
    {
        weaponsList.Clear();
        weaponsList.Add(new WeaponData());
        currentWeapon = weaponsList[0];
        currentIndex = 0;
    }

    //Получить случайный элемент
    public WeaponData GetRandomElement()
    {
        int random = Random.Range(0, weaponsList.Count);
        return (weaponsList[random]);
    }

    public WeaponData this[int index]
    {
        get
        {
            if (weaponsList != null && index >= 0 && index < weaponsList.Count)
                return weaponsList[index];
            return null;
        } 

        set
        {
            if (weaponsList == null)
                weaponsList = new List<WeaponData>();

            if (index >= 0 && index < weaponsList.Count && value != null)
                weaponsList[index] = value;
            else Debug.LogError("Выход за границы массива");

        }
    }
}

[System.Serializable] //Отображает объекты этого класса в Editor
public class WeaponData
{

    public enum _WeaponGroup { None, MainWeapon, SecondWeapon, Tool }
    [SerializeField] private _WeaponGroup weaponGroup = _WeaponGroup.None;
    public _WeaponGroup WeaponGroup
    {
        get { return weaponGroup; }
        protected set { }
    }

    public enum _MainWeaponName { None, Bazuka , Rifle, Lazer }
    [SerializeField] private _MainWeaponName mainWeaponName = _MainWeaponName.None;
    public _MainWeaponName MainWeaponName
    {
        get { return mainWeaponName; }
        protected set { }
    }

    public enum _SecondWeaponName { None, Pistol }
    [SerializeField] private _SecondWeaponName secondWeaponName = _SecondWeaponName.None;
    public _SecondWeaponName SecondWeaponName
    {
        get { return secondWeaponName; }
        protected set { }
    }

    public enum _CharacterClass { None, All, Demoman, Engineer, Soldier }
    [SerializeField] private _CharacterClass characterClass = _CharacterClass.None;
    public _CharacterClass CharacterClass
    {
        get { return characterClass; }
        protected set { }
    }

    public enum TypeOfDamage { Unit, Mass }
    [SerializeField] private TypeOfDamage damageType = TypeOfDamage.Unit;
    public TypeOfDamage DamageType
    {
        get { return damageType; }
        protected set { }
    }

    [SerializeField] private GameObject bulletPrefab = null;
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
        protected set { }
    }

    [SerializeField] private float damage = 0f;
    public float Damage
    {
        get { return damage; }
        protected set { }
    }

    [SerializeField] private float lifeTime = 0f;
    public float LifeTime
    {
        get { return lifeTime; }
        protected set { }
    }

    [SerializeField] private float bulletSpeed = 0f;
    public float BulletSpeed
    {
        get { return bulletSpeed; }
        protected set { }
    }

    [Tooltip("Количество пуль в обойме")]
    [SerializeField] private int bulletsCountInClip = 0;
    public int BulletsCountInClip
    {
        get { return bulletsCountInClip; }
        protected set { }
    }

    [Tooltip("Максимальное количество пуль")]
    [SerializeField] private int bulletsMaxCount = 0;
    public int BulletsMaxCount
    {
        get { return bulletsMaxCount; }
        protected set { }
    }

    [Tooltip("Задержка между выстрелами")]
    [SerializeField] private float shootDelay = 0f;
    public float ShotDelay
    {
        get { return shootDelay; }
        protected set { }
    }

    [SerializeField] private float reloadTime = 0f;
    public float ReloadTime
    {
        get { return reloadTime; }
        protected set { }
    }

    [Tooltip("Вес оружия")]
    [SerializeField] private float weaponWeight = 0f;
    public float WeaponWeight
    {
        get { return weaponWeight; }
        protected set { }
    }

    [Tooltip("Иконка оружия")]
    [SerializeField] private Sprite weaponSprite = null;
    public Sprite WeaponSprite
    {
        get { return weaponSprite; }
        protected set { }

    }
}
