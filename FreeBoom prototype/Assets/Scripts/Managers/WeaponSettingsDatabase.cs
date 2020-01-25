using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Weapons", fileName = "Weapons")]
public class WeaponSettingsDatabase : ScriptableObject
{
    [SerializeField, HideInInspector] private List<WeaponData> weaponList; //Лист где хранятся данны, SerializeField сохраняет данные после выхода из редактора, HideInInspector что бы не отображало List в Editor

    [SerializeField] private WeaponData currentWeapon; //Текущий отображаемый элемент

    private int currentIndex = 0; //Что бы было удобней следить за номером текущего элемента

    //Метод добавления в лист нового пустого элемента и удаление текущего. 
    public void AddElement()
    {
        if (weaponList == null) // Необходимо будет проверить наличие листа в памяти что бы его инициализировать
            weaponList = new List<WeaponData>();

        currentWeapon = new WeaponData(); // в currentWeapon помещаем новый объект нашего класса
        weaponList.Add(currentWeapon); // добавляем в список персонажей
        currentIndex = weaponList.Count - 1; // обновляем индекс
    }

    //Удаление элемента. Вместо создания нового экземляра, подсовываем предыдущий объект, если он есть.
    public void RemoveCurrentElement()
    {
        if (currentIndex > 0)
        {
            currentWeapon = weaponList[--currentIndex];
            weaponList.RemoveAt(++currentIndex);
        }
        else
        {
            weaponList.Clear();
            currentWeapon = null;
        }
    }

    //Метод переключающий элемент на следующий
    public WeaponData GetNext()
    {
        if (currentIndex < weaponList.Count - 1)
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
        weaponList.Clear();
        weaponList.Add(new WeaponData());
        currentWeapon = weaponList[0];
        currentIndex = 0;
    }

    //Получить случайный элемент
    public WeaponData GetRandomElement()
    {
        int random = Random.Range(0, weaponList.Count);
        return (weaponList[random]);
    }

    public WeaponData this[int index]
    {
        get
        {
            if (weaponList != null && index >= 0 && index < weaponList.Count)
                return weaponList[index];
            return null;
        } 

        set
        {
            if (weaponList == null)
                weaponList = new List<WeaponData>();

            if (index >= 0 && index < weaponList.Count && value != null)
                weaponList[index] = value;
            else Debug.LogError("Выход за границы массива");

        }
    }
}

[System.Serializable] //Отображает объекты этого класса в Editor
public class WeaponData
{
    [SerializeField] private string name = null;
    public string WeaponName
    {
        get { return name; }
        protected set { }
    }

    [SerializeField] private float speed = 0f;
    public float Speed
    {
        get { return speed; }
        protected set { }
    }

    [SerializeField] private float damage = 0f;
    public float Damage
    {
        get { return damage; }
        protected set { }
    }
}
