using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Banks", fileName = "Banks")]
public class BankSettingsDatabase : ScriptableObject
{
    [/*SerializeField, */HideInInspector] public List<BankData> bankList; //Лист где хранятся данны, SerializeField сохраняет данные после выхода из редактора, HideInInspector что бы не отображало List в Editor

    [SerializeField] private BankData currentBank; //Текущий отображаемый элемент

    private int currentIndex = 0; //Что бы было удобней следить за номером текущего элемента

    //Метод добавления в лист нового пустого элемента и удаление текущего. 
    public void AddElement()
    {
        if (bankList == null) // Необходимо будет проверить наличие листа в памяти что бы его инициализировать
            bankList = new List<BankData>();

        currentBank = new BankData(); // в currentCharacter помещаем новый объект нашего класса
        bankList.Add(currentBank); // добавляем в список персонажей
        currentIndex = bankList.Count - 1; // обновляем индекс
    }

    //Удаление элемента. Вместо создания нового экземляра, подсовываем предыдущий объект, если он есть.
    public void RemoveCurrentElement()
    {
        if (currentIndex > 0)
        {
            currentBank = bankList[--currentIndex];
            bankList.RemoveAt(++currentIndex);
        }
        else
        {
            bankList.Clear();
            currentBank = null;
        }
    }

    //Метод переключающий элемент на следующий
    public BankData GetNext()
    {
        if (currentIndex < bankList.Count - 1)
            currentIndex++;
        currentBank = this[currentIndex];
        return currentBank;
    }

    //Метод переключающий элемент на предыдущий
    public BankData GetPrev()
    {
        if (currentIndex > 0)
            currentIndex--;
        currentBank = this[currentIndex];
        return currentBank;
    }

    //Метод очищения базы данных
    public void ClearDatabase()
    {
        bankList.Clear();
        bankList.Add(new BankData());
        currentBank = bankList[0];
        currentIndex = 0;
    }

    //Получить случайный элемент
    public BankData GetRandomElement()
    {
        int random = Random.Range(0, bankList.Count);
        return (bankList[random]);
    }

    public BankData this[int index]
    {
        get
        {
            if (bankList != null && index >= 0 && index < bankList.Count)
                return bankList[index];
            return null;
        }

        set
        {
            if (bankList == null)
                bankList = new List<BankData>();

            if (index >= 0 && index < bankList.Count && value != null)
                bankList[index] = value;
            else Debug.LogError("Выход за границы массива");

        }
    }
}

[System.Serializable] //Отображает объекты этого класса в Editor
public class BankData
{

    public enum _BankLevel { None, First, Second, Third }
    [Tooltip("Уровень банка")]
    [SerializeField] private _BankLevel bankLevel = _BankLevel.None;
    public _BankLevel BankLevel
    {
        get { return bankLevel; }
        protected set { }
    }

    [Tooltip("Здоровье банка")]
    [SerializeField] private float health = 0f;
    public float Health
    {
        get { return health; }
        protected set { }
    }
}
