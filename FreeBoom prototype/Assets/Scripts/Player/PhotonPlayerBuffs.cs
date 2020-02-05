using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerBuffs : MonoBehaviour
{
    public enum Buff { None, FirstAid, Shield, Potions }

    [Tooltip("Параметры бафов")]
    [Header("Buffs")]
    public Buff currentBuff = Buff.None;

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
        photonChangeWeaponBar.changeButton.onClick.AddListener(() => photonChangeWeaponBar.ChangeBuff());
        photonChangeWeaponBar.chooseButton.onClick.AddListener(() => UseBuffs());
    }

    [PunRPC]
    public void GetBuffs(Buff buff)
    {
        if (!PV.IsMine) return;
        currentBuff = buff;
        switch (currentBuff)
        {
            case Buff.FirstAid:
                    firstAidCount += 1;
                if (!photonChangeWeaponBar.firstAid)
                    photonChangeWeaponBar.AddBuffsToListFirstTime(PhotonChangeWeaponBar.CurrentBuff.FirstAid, firstAidCount);
                else
                    photonChangeWeaponBar.AddBuffsToList(PhotonChangeWeaponBar.CurrentBuff.FirstAid, firstAidCount);
                break;
            case Buff.Shield:
                shieldCount += 1;
                if (!photonChangeWeaponBar.shield)
                    photonChangeWeaponBar.AddBuffsToListFirstTime(PhotonChangeWeaponBar.CurrentBuff.Shield, shieldCount);
                else
                    photonChangeWeaponBar.AddBuffsToList(PhotonChangeWeaponBar.CurrentBuff.Shield, shieldCount);
                break;
            case Buff.Potions:
                potionsCount += 1;

                if (!photonChangeWeaponBar.potions)
                    photonChangeWeaponBar.AddBuffsToListFirstTime(PhotonChangeWeaponBar.CurrentBuff.Potions, potionsCount);
                else
                    photonChangeWeaponBar.AddBuffsToList(PhotonChangeWeaponBar.CurrentBuff.Potions, potionsCount);
                break;
        }
    }

    public void UseBuffs()
    {     
        switch(photonChangeWeaponBar.currentBuff)
        {
            case PhotonChangeWeaponBar.CurrentBuff.FirstAid:
                firstAidCount -= 1;
                photonChangeWeaponBar.UseBuff(firstAidCount);
                break;
            case PhotonChangeWeaponBar.CurrentBuff.Shield:
                shieldCount -= 1;
                photonChangeWeaponBar.UseBuff(shieldCount);
                break;
            case PhotonChangeWeaponBar.CurrentBuff.Potions:
                potionsCount -= 1;
                photonChangeWeaponBar.UseBuff(potionsCount);
                break;
        }
    }



}
