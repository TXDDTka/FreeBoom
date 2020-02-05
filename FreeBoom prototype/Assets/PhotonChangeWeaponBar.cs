using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PhotonChangeWeaponBar : MonoBehaviour
{
    public enum CurrentBuff
    {
        None,
        FirstAid,
        Shield,
        Potions
    }

    [System.Serializable]
    public class BuffsList
    {
        public Sprite buffSprite;
        public int buffCount;
        public CurrentBuff currentBuff;
    }

    private List<BuffsList> buffsList = new List<BuffsList>();

    public CurrentBuff currentBuff = CurrentBuff.None;
    private CurrentBuff addedBuff = CurrentBuff.None;
    public static PhotonChangeWeaponBar Instance { get; private set; }

    [SerializeField]private int yPosition = 0;

    [Tooltip("Ячейка с бафами")]
    [Header("MainWeapon")]
    public Button mainWeaponButton;
    public Image mainWeaponImage;
    public Text mainWeaponBulletCountText;
    private int mainWeaponCurrentBulletCount = 0;
    private int mainWeaponMaxBulletCount = 0;
    private bool mainWeaponActive = true;

    [Tooltip("Ячейка с бафами")]
    [Header("SeconWeapon")]
    public Button secondWeaponButton;
    public Image secondWeaponImage;
    public Text secondWeaponCurrentBulletCountText;
    private int secondWeaponCurrentBulletCount = 0;
    private int secondWeaponMaxBulletCount = 0;
    private bool secondWeaponActive = false;

    [Tooltip("Ячейка с бафами")]
    [Header("Buffs")]
    public bool firstAid = false;
    public bool shield = false;
    public bool potions = false;
    private bool firstElementExitst = false;

    public Button chooseButton;
    public Button changeButton;
    [SerializeField] private Image buffSprite;
    [SerializeField] private Text buffCountText;

    private int buffIndex = 0;

    public BuffsSettingsDatabase buffsSettingsDatabase;

    
    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    void Awake()
    {
        InitializeSingleton();
    }

    void Start()
    {
        mainWeaponButton.interactable = false;
        mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y + yPosition);
    }

    public void ChangeWeapon()
    {
        if (mainWeaponActive)
        {
            mainWeaponButton.interactable = true;
            mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y - yPosition);
            mainWeaponBulletCountText.gameObject.SetActive(false);
            mainWeaponActive = false;

            secondWeaponButton.interactable = false;
            secondWeaponImage.transform.position = new Vector2(secondWeaponImage.transform.position.x, secondWeaponImage.transform.position.y + yPosition);
            secondWeaponCurrentBulletCountText.gameObject.SetActive(true);
            secondWeaponActive = true;
        }
        else if (secondWeaponActive)
        {
            secondWeaponButton.interactable = true;
            secondWeaponImage.transform.position = new Vector2(secondWeaponImage.transform.position.x, secondWeaponImage.transform.position.y - yPosition);
            secondWeaponCurrentBulletCountText.gameObject.SetActive(false);
            secondWeaponActive = false;

            mainWeaponButton.interactable = false;
            mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y + yPosition);
            mainWeaponBulletCountText.gameObject.SetActive(true);
            mainWeaponActive = true;
        }
    }



    public void AddBuffsToListFirstTime(CurrentBuff buff,int buffCount)
    {
        addedBuff = buff;
        switch (addedBuff)
        {
          case  CurrentBuff.FirstAid:
                firstAid = true;
            break;
            case CurrentBuff.Shield:
                shield = true;
                break;
            case CurrentBuff.Potions:
                potions = true;
                break;
        }

        for (int i = buffsSettingsDatabase.buffsList.Count - 1; i >= 0; i--)
        {
            var buffInDatabase = buffsSettingsDatabase.buffsList[i];
            if (buffInDatabase.BuffName.ToString() == addedBuff.ToString())
            {
                buffsList.Add(new BuffsList() { currentBuff = addedBuff, buffSprite = buffInDatabase.BuffSprite, buffCount = buffCount });
                ShowBuff();
                return;
            }
        }
    }

    public void AddBuffsToList(CurrentBuff buff, int buffCount)
    {
        addedBuff = buff;
        for (int i = buffsList.Count - 1; i >= 0; i--)
        {
            var buffInList = buffsList[i];
            if (addedBuff == buffInList.currentBuff)
            {
                buffInList.buffCount = buffCount;
                buffCountText.text = buffInList.buffCount.ToString();

                return;
            }
        }
    }



    private void ShowBuff()
    {
        if (!firstElementExitst)
        {
            buffSprite.gameObject.SetActive(true);
            buffCountText.gameObject.SetActive(true);
            chooseButton.interactable = true;
            buffSprite.sprite = buffsList[buffIndex].buffSprite;
            buffCountText.text = buffsList[buffIndex].buffCount.ToString();
            firstElementExitst = true;
            currentBuff = addedBuff;
        }
        else
        {
            if (changeButton.gameObject.activeInHierarchy == false)
                changeButton.gameObject.SetActive(true);
        }
    }


    public void UseBuff(int buffCount)
    {
        for (int i = buffsList.Count - 1; i >= 0; i--)
        {
            var buffInList = buffsList[i];
            if (currentBuff == buffInList.currentBuff)
            {
                buffInList.buffCount = buffCount;
                if (buffInList.buffCount == 0)
                {
                    buffsList.RemoveAt(buffsList.IndexOf(buffInList));
                    
                    switch (currentBuff)
                    {
                        case CurrentBuff.FirstAid:
                            firstAid = false;
                            break;
                        case CurrentBuff.Shield:
                            shield = false;
                            break;
                        case CurrentBuff.Potions:
                            potions = false;
                            break;
                    }

                    ChangeBuff();
                }
                else
                {
                    buffCountText.text = buffInList.buffCount.ToString();
                }
                return;

            }
        }
    }


    public void ChangeBuff()
    {
        if (buffsList.Count > 0)
        {
            if (buffIndex < buffsList.Count - 1)
            {
                buffIndex++;

                buffSprite.sprite = buffsList[buffIndex].buffSprite;
                buffCountText.text = buffsList[buffIndex].buffCount.ToString();
                currentBuff = buffsList[buffIndex].currentBuff;

                if (buffsList.Count == 1)
                {
                    changeButton.gameObject.SetActive(false);
                }
            }
            else
            {
                buffIndex = 0;

                buffSprite.sprite = buffsList[buffIndex].buffSprite;
                buffCountText.text = buffsList[buffIndex].buffCount.ToString();
                currentBuff = buffsList[buffIndex].currentBuff;

                if (buffsList.Count == 1)
                {
                    changeButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            HideBuff();
        }
    }


    private void HideBuff()
    {

            buffSprite.gameObject.SetActive(false);
            buffCountText.gameObject.SetActive(false);
            chooseButton.interactable = false;   
            firstElementExitst = false;
            currentBuff = CurrentBuff.None;
            changeButton.gameObject.SetActive(false);
    }
}

