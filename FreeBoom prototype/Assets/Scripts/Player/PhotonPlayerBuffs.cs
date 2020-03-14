using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerBuffs : MonoBehaviour
{
    public enum Buff { None, FirstAid, Shield, Potions }

    [Tooltip("Параметры бафов")]
    [Header("Buffs")]
    public Buff addedBuff = Buff.None;
    public Buff currentBuff = Buff.None;
    public bool firstBuffAdded = false;

    public int firstAidCount = 0;
    public int shieldCount = 0;
    public int potionsCount = 0;

    private PhotonChangeWeaponBar photonChangeWeaponBar = null;
    public BuffsSettingsDatabase buffsSettingsDatabase;
    private PhotonView PV = null;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonChangeWeaponBar = PhotonChangeWeaponBar.Instance;
    }

    private void Start()
    {
        if (!PV.IsMine) return;

        photonChangeWeaponBar.changeButton.onClick.AddListener(delegate 
        {
            photonChangeWeaponBar.ChangeBuff();
            currentBuff = (Buff)(byte)photonChangeWeaponBar.currentBuff;
        });

        photonChangeWeaponBar.chooseButton.onClick.AddListener(() => UseBuffs());
    }

    [PunRPC]
    public void GetBuffs(byte buff)
    {
        if (!PV.IsMine) return;
        addedBuff = (Buff)buff;
        switch (addedBuff)
        {
            case Buff.FirstAid:
                    firstAidCount += 1;
                if (!photonChangeWeaponBar.firstAid)
                    photonChangeWeaponBar.AddBuffsToListFirstTime(PhotonChangeWeaponBar.Buff.FirstAid, firstAidCount);
                else
                    photonChangeWeaponBar.AddBuffsToList(PhotonChangeWeaponBar.Buff.FirstAid, firstAidCount);
                break;
            case Buff.Shield:
                shieldCount += 1;
                if (!photonChangeWeaponBar.shield)
                    photonChangeWeaponBar.AddBuffsToListFirstTime(PhotonChangeWeaponBar.Buff.Shield, shieldCount);
                else
                    photonChangeWeaponBar.AddBuffsToList(PhotonChangeWeaponBar.Buff.Shield, shieldCount);
                break;
            case Buff.Potions:
                potionsCount += 1;

                if (!photonChangeWeaponBar.potions)
                    photonChangeWeaponBar.AddBuffsToListFirstTime(PhotonChangeWeaponBar.Buff.Potions, potionsCount);
                else
                    photonChangeWeaponBar.AddBuffsToList(PhotonChangeWeaponBar.Buff.Potions, potionsCount);
                break;
        }
        if (!firstBuffAdded)
        {
            currentBuff = addedBuff;
            firstBuffAdded = true;
        }
    }


    public void UseBuffs()
    {     
        switch(photonChangeWeaponBar.currentBuff)
        {
            case PhotonChangeWeaponBar.Buff.FirstAid:
                firstAidCount -= 1;
                photonChangeWeaponBar.UseBuff(firstAidCount);
                break;
            case PhotonChangeWeaponBar.Buff.Shield:
                shieldCount -= 1;
                photonChangeWeaponBar.UseBuff(shieldCount);
                break;
            case PhotonChangeWeaponBar.Buff.Potions:
                potionsCount -= 1;
                photonChangeWeaponBar.UseBuff(potionsCount);
                break;
        }
    }

   private void OnTriggerEnter(Collider other)
    {
            if (other.GetComponent<BuffObject>() != null)
            {

            if (PV.IsMine)
            {
                PV.RPC("GetBuffs", RpcTarget.AllBufferedViaServer, (byte)other.GetComponent<BuffObject>().buffType);
            }

            if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }

            }
    }

}
