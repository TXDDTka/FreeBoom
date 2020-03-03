using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class PlayerBuffs : MonoBehaviour
{
    public enum Buff
    {
        None, HealthMin, HealthMid, HealthMax, ShieldMin, ShieldMid, ShieldMax, StimulantMin,
        StimulantMid, StimulantMax, EnergeticMin, EnergeticMid, EnergeticMax
    }

    [Tooltip("Параметры бафов")]
    [Header("Buffs")]
    public Buff addedBuff = Buff.None;
    public Buff currentBuff = Buff.None;
    public bool firstBuffAdded = false;

    public int healthMinCount = 0;
    public int healthMidCount = 0;
    public int healthMaxCount = 0;

    public int shieldMinCount = 0;
    public int shieldMidCount = 0;
    public int shieldMaxCount = 0;

    public int stimulantMinCount = 0;

    private ChangeWeaponBar changeWeaponBar = null;
    public BuffsSettingsDatabase buffsSettingsDatabase = null;
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
            case Buff.HealthMin:
                    healthMinCount += 1;
                if (!changeWeaponBar.healthMinBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.HealthMin, healthMinCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.HealthMin, healthMinCount);
                break;
            case Buff.HealthMid:
                healthMidCount += 1;
                if (!changeWeaponBar.healthMidBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.HealthMid, healthMidCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.HealthMid, healthMidCount);
                break;
            case Buff.HealthMax:
                healthMinCount += 1;
                if (!changeWeaponBar.healthMaxBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.HealthMax, healthMaxCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.HealthMax, healthMaxCount);
                break;
            case Buff.ShieldMin:
                shieldMinCount += 1;
                if (!changeWeaponBar.shieldMinBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.ShieldMin, shieldMinCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.ShieldMin, shieldMinCount);
                break;
            case Buff.ShieldMid:
                shieldMidCount += 1;
                if (!changeWeaponBar.shieldMidBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.ShieldMid, shieldMidCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.ShieldMid, shieldMidCount);
                break;
            case Buff.ShieldMax:
                shieldMaxCount += 1;
                if (!changeWeaponBar.shieldMaxBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.ShieldMax, shieldMaxCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.ShieldMax, shieldMaxCount);
                break;
            case Buff.StimulantMin:
                stimulantMinCount += 1;
                if (!changeWeaponBar.stimulantMinBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.StimulantMin, stimulantMinCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.StimulantMin, stimulantMinCount);
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
            case ChangeWeaponBar.Buff.HealthMin:
                healthMinCount -= 1;
                changeWeaponBar.UseBuff(healthMinCount);
                break;
            case ChangeWeaponBar.Buff.HealthMid:
                healthMidCount -= 1;
                changeWeaponBar.UseBuff(healthMidCount);
                break;
            case ChangeWeaponBar.Buff.HealthMax:
                healthMaxCount -= 1;
                changeWeaponBar.UseBuff(healthMaxCount);
                break;
            case ChangeWeaponBar.Buff.ShieldMin:
                shieldMinCount -= 1;
                changeWeaponBar.UseBuff(shieldMinCount);
                break;
            case ChangeWeaponBar.Buff.ShieldMid:
                shieldMidCount -= 1;
                changeWeaponBar.UseBuff(shieldMidCount);
                break;
            case ChangeWeaponBar.Buff.ShieldMax:
                shieldMaxCount -= 1;
                changeWeaponBar.UseBuff(shieldMaxCount);
                break;
            case ChangeWeaponBar.Buff.StimulantMin:
                stimulantMinCount -= 1;
                changeWeaponBar.UseBuff(stimulantMinCount);
                break;
        }
    }

   private void OnTriggerEnter(Collider other)
    {
            if (other.GetComponent<BuffObject>() != null)
            {

            if (playerManager.PV.IsMine)
            {
                playerManager.PV.RPC("GetBuffs", RpcTarget.AllBufferedViaServer, (byte)other.GetComponent<BuffObject>().buffType);
            }

            if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(other.gameObject);
                }

            }
    }

}
