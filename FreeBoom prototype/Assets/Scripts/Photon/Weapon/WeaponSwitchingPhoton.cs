using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchingPhoton : MonoBehaviour, IPunObservable
{

    //public static WeaponSwitchingPhoton Instance;
    [SerializeField] private  ChangeWeaponButtonSingleton weaponSwitchButton;
   // [SerializeField] private Button changeWeaponButton;
    [SerializeField] private Rigidbody2D bulletPrefab;
    //[SerializeField] private float bulletSpeed;
    private int previousIndex;
    [SerializeField] private int currentIndex;
    public GameObject currentWeapon;
    public GameObject nextWeapon;
    private PhotonView photonview;

    public bool changeWeapon;

    //public Rigidbody2D BulletPrefab => bulletPrefab;
    //public float BulletSpeed => bulletSpeed;

    void Awake()
    {
        //if (Instance == null) Instance = this;
        //  else if (Instance != this) Destroy(this);

       
      // if(weaponSwitchButton == null) 
        weaponSwitchButton = ChangeWeaponButtonSingleton.Instance;
        photonview = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!photonview.IsMine) return;
        weaponSwitchButton.GetComponent<Button>().onClick.AddListener(() => ChangeWeapon());

        SwitchWeapon();
    }

    private void Update()
    {   if (!photonview.IsMine) return;
        if (previousIndex != currentIndex)
        {
            SwitchWeapon();
            previousIndex = currentIndex;
        }
    }

    private void ChangeWeapon()
    {
        if (!photonview.IsMine) return;
        if (currentIndex < transform.childCount - 1)
            currentIndex++;
        else
            currentIndex = 0;
        SwitchWeapon();
      //  Change();
    }


    private void SwitchWeapon()
    {
        if (!photonview.IsMine) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (currentIndex == i)
            {
                //changeWeapon = true;
                transform.GetChild(i).gameObject.SetActive(true);
                //  currentWeapon = transform.GetChild(i).gameObject;
            }

            else
            {
               // changeWeapon = false;
                transform.GetChild(i).gameObject.SetActive(false);
                // currentWeapon = transform.GetChild(i).gameObject; 
            }
        }

        //changeWeapon = true;
        //  transform.GetChild(currentIndex).gameObject.SetActive(true);
    }

    private void Change()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Мы владелец этого игрока: отправляем другим наши данные
            //  stream.SendNext(changeWeapon);
            // stream.SendNext(currentWeapon.activeSelf);
            stream.SendNext(transform.GetChild(0).gameObject.activeSelf);
            stream.SendNext(transform.GetChild(1).gameObject.activeSelf);
            stream.SendNext(transform.GetChild(2).gameObject.activeSelf);
        }
        else
        {
            // Не наш игрок,мы получаем данные от другого игрока
            // changeWeapon = (bool)stream.ReceiveNext();
            // currentWeapon.SetActive((bool)stream.ReceiveNext());
            transform.GetChild(0).gameObject.SetActive((bool)stream.ReceiveNext());
            transform.GetChild(1).gameObject.SetActive((bool)stream.ReceiveNext());
            transform.GetChild(2).gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }
}
