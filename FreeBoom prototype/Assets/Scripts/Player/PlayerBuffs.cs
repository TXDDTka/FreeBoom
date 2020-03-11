using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffs : MonoBehaviour
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

    private ChangeWeaponBar changeWeaponBar = null;
    public BuffsSettingsDatabase buffsSettingsDatabase;
    private PlayerManager playerManager = null;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        changeWeaponBar = ChangeWeaponBar.Instance;
    }

    private void Start()
    {
        if (!playerManager.PV.IsMine) return;

        changeWeaponBar.changeButton.onClick.AddListener(delegate 
        {
            changeWeaponBar.ChangeBuff();
            currentBuff = (Buff)(byte)changeWeaponBar.currentBuff;
        });

        changeWeaponBar.chooseButton.onClick.AddListener(() => UseBuffs());
    }

    [PunRPC]
    public void GetBuffs(byte buff)
    {
        if (!playerManager.PV.IsMine) return;
        addedBuff = (Buff)buff;
        switch (addedBuff)
        {
            case Buff.FirstAid:
                    firstAidCount += 1;
                if (!changeWeaponBar.firstAid)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.FirstAid, firstAidCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.FirstAid, firstAidCount);
                break;
            case Buff.Shield:
                shieldCount += 1;
                if (!changeWeaponBar.shield)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.Shield, shieldCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.Shield, shieldCount);
                break;
            case Buff.Potions:
                potionsCount += 1;

                if (!changeWeaponBar.potions)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.Potions, potionsCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.Potions, potionsCount);
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
        switch(changeWeaponBar.currentBuff)
        {
            case ChangeWeaponBar.Buff.FirstAid:
                firstAidCount -= 1;
                changeWeaponBar.UseBuff(firstAidCount);
                break;
            case ChangeWeaponBar.Buff.Shield:
                shieldCount -= 1;
                changeWeaponBar.UseBuff(shieldCount);
                break;
            case ChangeWeaponBar.Buff.Potions:
                potionsCount -= 1;
                changeWeaponBar.UseBuff(potionsCount);
                break;
        }
    }

   private void OnCollisionEnter2D(Collision2D other)
    {
            if (other.gameObject.GetComponent<BuffObject>())
            {

            if (playerManager.PV.IsMine)
            {
                playerManager.PV.RPC("GetBuffs", RpcTarget.AllBufferedViaServer, (byte)other.gameObject.GetComponent<BuffObject>().buffType);
            }

            if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }

            }
    }

}
