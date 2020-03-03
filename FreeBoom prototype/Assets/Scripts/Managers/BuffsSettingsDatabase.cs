using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Database/Buffs", fileName = "Buffs")]
public class BuffsSettingsDatabase : ScriptableObject
{
    [SerializeField, HideInInspector] public List<BuffData> buffsList; //Лист где хранятся данны, SerializeField сохраняет данные после выхода из редактора, HideInInspector что бы не отображало List в Editor

    [SerializeField] private BuffData currentBuff; //Текущий отображаемый элемент

    private int currentIndex = 0; //Что бы было удобней следить за номером текущего элемента

    //Метод добавления в лист нового пустого элемента и удаление текущего. 
    public void AddElement()
    {
        if (buffsList == null) // Необходимо будет проверить наличие листа в памяти что бы его инициализировать
            buffsList = new List<BuffData>();

        currentBuff = new BuffData(); // в currentBuff помещаем новый объект нашего класса
        buffsList.Add(currentBuff); // добавляем в список персонажей
        currentIndex = buffsList.Count - 1; // обновляем индекс
    }

    //Удаление элемента. Вместо создания нового экземляра, подсовываем предыдущий объект, если он есть.
    public void RemoveCurrentElement()
    {
        if (currentIndex > 0)
        {
            currentBuff = buffsList[--currentIndex];
            buffsList.RemoveAt(++currentIndex);
        }
        else
        {
            buffsList.Clear();
            currentBuff = null;
        }
    }

    //Метод переключающий элемент на следующий
    public BuffData GetNext()
    {
        if (currentIndex < buffsList.Count - 1)
            currentIndex++;
        currentBuff = this[currentIndex];
        return currentBuff;
    }

    //Метод переключающий элемент на предыдущий
    public BuffData GetPrev()
    {
        if (currentIndex > 0)
            currentIndex--;
        currentBuff = this[currentIndex];
        return currentBuff;
    }

    //Метод очищения базы данных
    public void ClearDatabase()
    {
        buffsList.Clear();
        buffsList.Add(new BuffData());
        currentBuff = buffsList[0];
        currentIndex = 0;
    }

    //Получить случайный элемент
    public BuffData GetRandomElement()
    {
        int random = Random.Range(0, buffsList.Count);
        return (buffsList[random]);
    }

    public BuffData this[int index]
    {
        get
        {
            if (buffsList != null && index >= 0 && index < buffsList.Count)
                return buffsList[index];
            return null;
        } 

        set
        {
            if (buffsList == null)
                buffsList = new List<BuffData>();

            if (index >= 0 && index < buffsList.Count && value != null)
                buffsList[index] = value;
            else Debug.LogError("Выход за границы массива");

        }
    }
}

[System.Serializable] //Отображает объекты этого класса в Editor
public class BuffData
{
    //private enum CharacterClass {All,Demoman, Engineer,Soldier};

    //[SerializeField] private string name = null;
    //public string WeaponName
    //{
    //    get { return name; }
    //    protected set { }
    //}


    public enum Buff {None, HealthMin, HealthMid, HealthMax, ShieldMin, ShieldMid, ShieldMax,  StimulantMin,
                        StimulantMid, StimulantMax, EnergeticMin, EnergeticMid, EnergeticMax }
    [SerializeField] private Buff buffName = Buff.None;
    public Buff BuffName
    {
        get { return buffName; }
        protected set { }
    }

    //[SerializeField] private GameObject buffPrefab = null;
    //public GameObject BuffPrefab
    //{
    //    get { return buffPrefab; }
    //    protected set { }
    //}

    [Tooltip("Префаб бафа")]
    [SerializeField] private GameObject buffPrefab = null;
    public GameObject BuffPrefab
    {
        get { return buffPrefab; }
        protected set { }
    }

    [Tooltip("Здоровье востонавливаемое бафом")]
    [SerializeField] private float buffHP = 0f;
    public float BuffHPRecovery
    {
        get { return buffHP; }
        protected set { }
    }

    [Tooltip("Защита востонавливаемая бафом")]
    [SerializeField] private float buffShield = 0f;
    public float BuffShieldRecovery
    {
        get { return buffShield; }
        protected set { }
    }

    [Tooltip("Скорость от бафа")]
    [SerializeField] private float buffSpeed = 0f;
    public float BuffSpeed
    {
        get { return buffSpeed; }
        protected set { }
    }

    [Tooltip("Урон от оружия от бафа")]
    [SerializeField] private float buffDamage = 0f;
    public float BuffDamage
    {
        get { return buffDamage; }
        protected set { }
    }

    [Tooltip("Инверсия управления от бафа")]
    [SerializeField] private bool buffInversion = false;
    public bool BuffInversion
    {
        get { return buffInversion; }
        protected set { }
    }

    [Tooltip("Скорость стрельбы из оружия от бафа")]
    [SerializeField] private float buffSpeedDamage = 0f;
    public float BuffSpeedDamage
    {
        get { return buffSpeedDamage; }
        protected set { }
    }

    [Tooltip("Иконка баффа")]
    [SerializeField] private Sprite buffSprite = null;
    public Sprite BuffSprite
    {
        get { return buffSprite; }
        protected set { }
    }
}
