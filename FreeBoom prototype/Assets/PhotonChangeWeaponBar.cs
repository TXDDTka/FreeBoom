using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonChangeWeaponBar : MonoBehaviour
{

    public static PhotonChangeWeaponBar Instance { get; private set; }

    public int yPosition = 0;

    public Button mainWeaponButton;
    public Image mainWeaponImage;
    public Text mainWeaponBulletCountText;
    public int mainWeaponCurrentBulletCount = 0;
    public int mainWeaponMaxBulletCount = 0;
    public bool mainWeaponActive = true;

    public Button secondWeaponButton;
    public Image secondWeaponImage;
    public Text secondWeaponCurrentBulletCountText;
    public int secondWeaponCurrentBulletCount = 0;
    public int secondWeaponMaxBulletCount = 0;
    public bool secondWeaponActive = false;

    [Tooltip("Настройки ScrollBar")]
    //public Scrollbar scrollBar;
    //public Transform content;
    //float scroll_pos = 0;
    //float[] pos;

    [Range(1,5)]
    [Header ("Controllers")]
    public int panCount;

    [Range(1, 250)]
    public int panOffset;
    [Header("Other Objects")]
    public GameObject panPrefab;
    public Transform content;

    public GameObject[] instPans;
    public Vector2[] pansPos;
    public RectTransform contentRect;
    public int selectedPanId;
    public bool isScrolling;
    
    public Scrollbar scrollBar;
    public float[] pos;
    public float scroll_pos = 0;

    public Sprite[] baffsSprites;
    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        //Instance = this;
    }

    void Awake()
    {
        InitializeSingleton();
    }



     void Start()
    {
        //contentRect = content.GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        for (int i = 0; i < panCount; i ++)
        {
            //Создаем объект в content, используем локальные координаты для этих объектов
            instPans[i] = Instantiate(panPrefab, content,false);
            instPans[i].GetComponent<Image>().sprite = baffsSprites[i];
            //  if (i == 0) continue; //Пропускаем позицию первого объекта
            // instPans[i].transform.localScale = new Vector2(instPans[i].transform.localScale.x, //позиция по x
            // instPans[i - 1].transform.localPosition.y + panPrefab.GetComponent<RectTransform>().sizeDelta.y + panOffset);//позиция по y + отступ
            //  pansPos[i] = -instPans[i].transform.localPosition;
        }
        if(mainWeaponActive)
        {
            mainWeaponButton.interactable = false;
            mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y + yPosition);
            mainWeaponBulletCountText.text = $"{mainWeaponCurrentBulletCount} / {mainWeaponMaxBulletCount}";
            mainWeaponBulletCountText.gameObject.SetActive(true);
        }
    }

    //private void FixedUpdate()
    //{
    //    float nearestPos = float.MaxValue;

    //    for (int i = 0; i < panCount; i++)
    //    {
    //        float distance = Mathf.Abs(contentRect.anchoredPosition.y - pansPos[i].y);
    //        if (distance < nearestPos)
    //        {
    //            Debug.Log("distance < nearestPos");
    //            nearestPos = distance;
    //            selectedPanId = i;
    //        }
    //    }
       // pos = new float[content.childCount];
        //if (isScrolling)
        //{
        //    scrollBar.value = Mathf.Lerp(scrollBar.value, pos[2], 0.1f);
        //}
   // }


    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
    }
    private void Update()
    {
        //pos = new float[content.childCount];
        ////Debug.Log("content.childCount" + content.childCount);
        //float distanse = 1f / (pos.Length - 1);
        //for (int i = 0; i < pos.Length; i++)
        //{
        //    pos[i] = distanse * i;
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    scroll_pos = scrollBar.value;
        //}
        //else
        //{
        //    for (int i = 0; i < pos.Length; i++)
        //    {
        //        if (scroll_pos < pos[i] + (distanse / 2) && scroll_pos > pos[i] - (distanse / 2))
        //        {
        //            scrollBar.value = Mathf.Lerp(scrollBar.value, pos[i], 0.1f);
        //        }
        //    }
        //}

        //for (int i = 0; i < pos.Length; i++)
        //{
        //    if (scroll_pos < pos[i] + (distanse / 2) && scroll_pos > pos[i] - (distanse / 2))
        //    {
        //        //  transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
        //        for (int a = 0; a < pos.Length; a++)
        //        {
        //            if (a != i)
        //            {
        //                transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
        //            }
        //        }
        //    }
        //}
    }

    public void ChangeWeapon()
    {
        if(mainWeaponActive)
        {
            mainWeaponButton.interactable = true;
            mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y - yPosition);
            //mainWeaponbulletCountText.text = $"{mainWeaponbulletCount} / {mainWeaponbulletCount}";
            mainWeaponBulletCountText.gameObject.SetActive(false);
            mainWeaponActive = false;

            secondWeaponButton.interactable = false;
            secondWeaponImage.transform.position = new Vector2(secondWeaponImage.transform.position.x, secondWeaponImage.transform.position.y + yPosition);
            secondWeaponCurrentBulletCountText.text = $"{secondWeaponCurrentBulletCount} / {secondWeaponMaxBulletCount}";
            secondWeaponCurrentBulletCountText.gameObject.SetActive(true);
            secondWeaponActive = true;
        }
        else if(secondWeaponActive)
        {
            secondWeaponButton.interactable = true;
            secondWeaponImage.transform.position = new Vector2(secondWeaponImage.transform.position.x, secondWeaponImage.transform.position.y - yPosition);
            //secondWeaponCurrentBulletCountText.text = $"{secondWeaponCurrentBulletCount} / {secondWeaponMaxBulletCount}";
            secondWeaponCurrentBulletCountText.gameObject.SetActive(false);
            secondWeaponActive = false;

            mainWeaponButton.interactable = false;
            mainWeaponImage.transform.position = new Vector2(mainWeaponImage.transform.position.x, mainWeaponImage.transform.position.y + yPosition);
            mainWeaponBulletCountText.text = $"{mainWeaponCurrentBulletCount} / {mainWeaponMaxBulletCount}";
            mainWeaponBulletCountText.gameObject.SetActive(true);
            mainWeaponActive = true;
        }
    }
}
