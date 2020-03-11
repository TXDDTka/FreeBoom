using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Characters", fileName = "Characters")]
public class CharactersSettingsDatabase : ScriptableObject
{
    [/*SerializeField, */HideInInspector] public List<CharacterData> charactersList; //Лист где хранятся данны, SerializeField сохраняет данные после выхода из редактора, HideInInspector что бы не отображало List в Editor

    [SerializeField] private CharacterData currentCharacter; //Текущий отображаемый элемент

    private int currentIndex = 0; //Что бы было удобней следить за номером текущего элемента

    //Метод добавления в лист нового пустого элемента и удаление текущего. 
    public void AddElement()
    {
        if (charactersList == null) // Необходимо будет проверить наличие листа в памяти что бы его инициализировать
            charactersList = new List<CharacterData>();

        currentCharacter = new CharacterData(); // в currentCharacter помещаем новый объект нашего класса
        charactersList.Add(currentCharacter); // добавляем в список персонажей
        currentIndex = charactersList.Count - 1; // обновляем индекс
    }

    //Удаление элемента. Вместо создания нового экземляра, подсовываем предыдущий объект, если он есть.
    public void RemoveCurrentElement()
    {
        if (currentIndex > 0)
        {
            currentCharacter = charactersList[--currentIndex];
            charactersList.RemoveAt(++currentIndex);
        }
        else
        {
            charactersList.Clear();
            currentCharacter = null;
        }
    }

    //Метод переключающий элемент на следующий
    public CharacterData GetNext()
    {
        if (currentIndex < charactersList.Count - 1)
            currentIndex++;
        currentCharacter = this[currentIndex];
        return currentCharacter;
    }

    //Метод переключающий элемент на предыдущий
    public CharacterData GetPrev()
    {
        if (currentIndex > 0)
            currentIndex--;
        currentCharacter = this[currentIndex];
        return currentCharacter;
    }

    //Метод очищения базы данных
    public void ClearDatabase()
    {
        charactersList.Clear();
        charactersList.Add(new CharacterData());
        currentCharacter = charactersList[0];
        currentIndex = 0;
    }

    //Получить случайный элемент
    public CharacterData GetRandomElement()
    {
        int random = Random.Range(0, charactersList.Count);
        return (charactersList[random]);
    }

    public CharacterData this[int index]
    {
        get
        {
            if (charactersList != null && index >= 0 && index < charactersList.Count)
                return charactersList[index];
            return null;
        } 

        set
        {
            if (charactersList == null)
                charactersList = new List<CharacterData>();

            if (index >= 0 && index < charactersList.Count && value != null)
                charactersList[index] = value;
            else Debug.LogError("Выход за границы массива");

        }
    }
}

[System.Serializable] //Отображает объекты этого класса в Editor
public class CharacterData
{
    //[SerializeField] private string name = null;
    //public string CharacterName
    //{
    //    get { return name; }
    //    protected set { }
    //}

    public enum _CharacterClass : byte { None, All, Demoman, Engineer, Soldier }
    [Tooltip("Класс персонажа")]
    [SerializeField] private _CharacterClass characterClass = _CharacterClass.None;
    public _CharacterClass CharacterClass
    {
        get { return characterClass; }
        protected set { }
    }

    [Tooltip("Cкорость персонажа")]
    [SerializeField] private float speed = 0f;
    public float Speed
    {
        get { return speed; }
        protected set { }
    }

    [Tooltip("Здоровье персонажа")]
    [SerializeField] private float health = 0f;
    public float Health
    {
        get { return health; }
        protected set { }
    }
}
