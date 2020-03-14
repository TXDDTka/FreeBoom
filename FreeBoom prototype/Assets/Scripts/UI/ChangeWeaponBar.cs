using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
public class ChangeWeaponBar : MonoBehaviour
{

    public enum ReloadingBarStatus { NoReloadNeeded, Reloading, Stopped }
    public enum Buff { None, FirstAid, Shield, Potions }

    [Serializable]
    public class BuffsList
    {
        public Sprite buffSprite;
        public int buffCount;
        public Buff currentBuff;
    }

    public PlayerWeaponManager.ReloadingStatus mainWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;
    public PlayerWeaponManager.ReloadingStatus secondWeaponReloadingBarStatus = PlayerWeaponManager.ReloadingStatus.NoReloadNeeded;

    public List<BuffsList> buffsList = new List<BuffsList>();

    public Buff currentBuff = Buff.None;
    private Buff addedBuff = Buff.None;
    public static ChangeWeaponBar Instance { get; private set; }

   // public IEnumerator mainWeaponCooldownCoroutine;
  //  public IEnumerator secondWeaponCooldownCoroutine;

    [Tooltip("Ячейка с бафами")]
    [Header("Buffs")]
    public bool firstAid = false;
    public bool shield = false;
    public bool potions = false;
    private bool firstElementExitst = false;
    private int buffIndex = 0;
    public BuffsSettingsDatabase buffsSettingsDatabase;
    [SerializeField] private Image buffSprite = null;
    [SerializeField] private Text buffCountText = null;

    [Tooltip("Ячейка с основным оружием")]
    [Header("MainWeapon")]
    public Button mainWeaponButton = null;
    public Image mainWeaponImage = null;
    public Text mainWeaponCurrentBulletCountText = null;
    [SerializeField] private bool mainWeaponActive = true;


    [Tooltip("Ячейка со вторым оружием")]
    [Header("SeconWeapon")]
    public Button secondWeaponButton = null;
    public Image secondWeaponImage = null;
    public Text secondWeaponCurrentBulletCountText = null;
    public bool secondWeaponActive = false;

    [Tooltip("Доп параметры оружия")]
    [Header("WeaponParametrs")]
    [SerializeField] private int yPosition = 0;
    public Button chooseButton = null;
    public Button changeButton = null;

    [Tooltip("КулдаунБар")]
    [Header("MainWeaponСooldownBar")]
    public GameObject cooldownBarMainWeapon = null;
    public Image cooldownImageMainWeapon = null;
    public Text cooldownTextMainWeapon = null;

    [Header("SecondWeaponСooldownBar")]
    public GameObject cooldownBarSecondWeapon = null;
    public Image cooldownImageSecondWeapon = null;
    public Text cooldownTextSecondWeapon = null;

    private float cooldownTimer = 0f;
    public float reloadingTime = 0f;
    public bool mainWeaponCooldown = false;
    public bool secondWeaponCooldown = false;
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
     //   mainWeaponCooldownCoroutine = MainWeaponCooldownTimer();
    }

    public void ChooseMainWeapon(Sprite mainWeaponSprite, int mainWeaponBulletsInClip, int mainWeaponBulletsMaxCount)
    {
        mainWeaponImage.sprite = mainWeaponSprite;
        mainWeaponCurrentBulletCountText.text = $"{mainWeaponBulletsInClip} / {mainWeaponBulletsMaxCount}";
    }
    public void ChooseSecondWeapon(Sprite secondWeaponSprite, int secondWeaponBulletsInClip, int secondWeaponBulletsMaxCount)
    {
        secondWeaponImage.sprite = secondWeaponSprite;
        secondWeaponCurrentBulletCountText.text = $"{secondWeaponBulletsInClip} / {secondWeaponBulletsMaxCount}";
    }


    public void ReloadingTime(float reloadingTime)
    {
        this.reloadingTime = reloadingTime;
        cooldownTimer = reloadingTime;
    }

    public void MainWeaponBarReloadingStatus()
    {
        switch(mainWeaponReloadingBarStatus)
        {
            case PlayerWeaponManager.ReloadingStatus.NoReloadNeeded:
                cooldownBarMainWeapon.SetActive(false);
                mainWeaponCurrentBulletCountText.gameObject.SetActive(true);
                break;
            case PlayerWeaponManager.ReloadingStatus.NeedReload:
                cooldownBarMainWeapon.SetActive(false);
                break;
            case PlayerWeaponManager.ReloadingStatus.Reloading:
                mainWeaponCurrentBulletCountText.gameObject.SetActive(false);
                cooldownBarMainWeapon.SetActive(true);
                break;
        }
    }

    public void SecondWeaponBarReloadingStatus()
    {
        switch (secondWeaponReloadingBarStatus)
        {
            case PlayerWeaponManager.ReloadingStatus.NoReloadNeeded:
                cooldownBarSecondWeapon.SetActive(false);
                secondWeaponCurrentBulletCountText.gameObject.SetActive(true);
                break;
            case PlayerWeaponManager.ReloadingStatus.NeedReload:
                cooldownBarSecondWeapon.SetActive(false);
                break;
            case PlayerWeaponManager.ReloadingStatus.Reloading:
                secondWeaponCurrentBulletCountText.gameObject.SetActive(false);
                cooldownBarSecondWeapon.SetActive(true);
                break;
        }
    }

    public IEnumerator MainWeaponCooldownTimer()
    {
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
            cooldownTextMainWeapon.text = string.Format("{0:0}", cooldownTimer);
            cooldownImageMainWeapon.fillAmount = cooldownTimer / reloadingTime;
        }

    }

    public IEnumerator SecondWeaponCooldownTimer()
    {

        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
            cooldownTextSecondWeapon.text = string.Format("{0:0}", cooldownTimer);
            cooldownImageSecondWeapon.fillAmount = cooldownTimer / reloadingTime;
        }

    }

    public void ChangeWeapon()
    {
        if (mainWeaponActive)
        {
            mainWeaponButton.interactable = true;
            mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y - yPosition);
            mainWeaponCurrentBulletCountText.gameObject.SetActive(false);
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
            mainWeaponCurrentBulletCountText.gameObject.SetActive(true);
            mainWeaponActive = true;
        }
    }


    public void ChangeMainWeaponBulletsCount(int mainWeaponBulletsInClip, int mainWeaponBulletsMaxCount)
    {
        mainWeaponCurrentBulletCountText.text = $"{mainWeaponBulletsInClip} / {mainWeaponBulletsMaxCount}";
    }

    public void ChangeSecondWeaponBulletsCount(int secondWeaponBulletsInClip, int secondWeaponBulletsMaxCount)
    {
        secondWeaponCurrentBulletCountText.text = $"{secondWeaponBulletsInClip} / {secondWeaponBulletsMaxCount}";
    }

    public void AddBuffsToListFirstTime(Buff buff, int buffCount)
    {
        addedBuff = buff;
        switch (addedBuff)
        {
            case Buff.FirstAid:
                firstAid = true;
                break;
            case Buff.Shield:
                shield = true;
                break;
            case Buff.Potions:
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

    public void AddBuffsToList(Buff buff, int buffCount)
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
            changeButton.gameObject.SetActive(true);
    }

    public void HideBuff()
    {
        firstAid = false;
        potions = false;
        shield = false;

        buffSprite.gameObject.SetActive(false);
        buffCountText.gameObject.SetActive(false);
        chooseButton.interactable = false;
        firstElementExitst = false;
        currentBuff = Buff.None;
        addedBuff = Buff.None;
        changeButton.gameObject.SetActive(false);
        if (buffsList.Count > 0)
            buffsList.Clear();

    }

    public bool BuffsActive()
    {
        firstAid = false;
        potions = false;
        shield = false;

        return true;
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
                        case Buff.FirstAid:
                            firstAid = false;
                            break;
                        case Buff.Shield:
                            shield = false;
                            break;
                        case Buff.Potions:
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
}

