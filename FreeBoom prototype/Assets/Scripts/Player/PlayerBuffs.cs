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
    public int stimulantMidCount = 0;
    public int stimulantMaxCount = 0;

    public int energeticMinCount = 0;
    public int energeticMidCount = 0;
    public int energeticMaxCount = 0;

    private ChangeWeaponBar changeWeaponBar = null;
    public BuffsSettingsDatabase buffsSettingsDatabase = null;
    private PlayerManager playerManager = null;
    private PlayerHealth playerHealth = null;
    private PlayerMovement playerMovement = null;
    private PlayerShooting playerShooting = null;

    private void Awake()
    {
        playerShooting = GetComponent<PlayerShooting>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
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
                healthMaxCount += 1;
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
            case Buff.StimulantMid:
                stimulantMidCount += 1;
                if (!changeWeaponBar.stimulantMidBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.StimulantMid, stimulantMidCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.StimulantMid, stimulantMidCount);
                break;
            case Buff.StimulantMax:
                stimulantMaxCount += 1;
                if (!changeWeaponBar.stimulantMaxBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.StimulantMax, stimulantMaxCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.StimulantMax, stimulantMaxCount);
                break;
            case Buff.EnergeticMin:
                energeticMinCount += 1;
                if (!changeWeaponBar.energeticMinBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.EnergeticMin, energeticMinCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.EnergeticMin, energeticMinCount);
                break;
            case Buff.EnergeticMid:
                energeticMidCount += 1;
                if (!changeWeaponBar.energeticMidBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.EnergeticMid, energeticMidCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.EnergeticMid, energeticMidCount);
                break;
            case Buff.EnergeticMax:
                energeticMaxCount += 1;
                if (!changeWeaponBar.energeticMinBuff)
                    changeWeaponBar.AddBuffsToListFirstTime(ChangeWeaponBar.Buff.EnergeticMax, energeticMaxCount);
                else
                    changeWeaponBar.AddBuffsToList(ChangeWeaponBar.Buff.EnergeticMax, energeticMaxCount);
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
                playerHealth.GetHealthPoint(buffsSettingsDatabase.buffsList[0].BuffHPRecovery);
                changeWeaponBar.UseBuff(healthMinCount);
                break;
            case ChangeWeaponBar.Buff.HealthMid:
                healthMidCount -= 1;
                playerHealth.GetHealthPoint(buffsSettingsDatabase.buffsList[1].BuffHPRecovery);
                changeWeaponBar.UseBuff(healthMidCount);
                break;
            case ChangeWeaponBar.Buff.HealthMax:
                healthMaxCount -= 1;
                playerHealth.GetHealthPoint(buffsSettingsDatabase.buffsList[2].BuffHPRecovery);
                changeWeaponBar.UseBuff(healthMaxCount);
                break;
            case ChangeWeaponBar.Buff.ShieldMin:
                shieldMinCount -= 1;
                playerHealth.GetShieldPoint(buffsSettingsDatabase.buffsList[3].BuffShieldRecovery);
                changeWeaponBar.UseBuff(shieldMinCount);
                break;
            case ChangeWeaponBar.Buff.ShieldMid:
                shieldMidCount -= 1;
                playerHealth.GetShieldPoint(buffsSettingsDatabase.buffsList[4].BuffShieldRecovery);
                playerMovement.GetSpeed(buffsSettingsDatabase.buffsList[4].BuffSpeed);
                changeWeaponBar.UseBuff(shieldMidCount);
                break;
            case ChangeWeaponBar.Buff.ShieldMax:
                shieldMaxCount -= 1;
                playerHealth.GetShieldPoint(buffsSettingsDatabase.buffsList[5].BuffShieldRecovery);
                playerMovement.GetSpeed(buffsSettingsDatabase.buffsList[5].BuffSpeed);
                changeWeaponBar.UseBuff(shieldMaxCount);
                break;
            case ChangeWeaponBar.Buff.StimulantMin:
                stimulantMinCount -= 1;
                playerShooting.GetDamage(buffsSettingsDatabase.buffsList[6].BuffDamage);
                changeWeaponBar.UseBuff(stimulantMinCount);
                break;
            case ChangeWeaponBar.Buff.StimulantMid:
                stimulantMidCount -= 1;
                playerShooting.GetDamage(buffsSettingsDatabase.buffsList[7].BuffDamage);
                playerMovement.GetSpeed(buffsSettingsDatabase.buffsList[7].BuffSpeed);
                changeWeaponBar.UseBuff(stimulantMidCount);
                break;
            case ChangeWeaponBar.Buff.StimulantMax:
                stimulantMaxCount -= 1;
                playerShooting.GetDamage(buffsSettingsDatabase.buffsList[8].BuffDamage);
                // инверсия
                //playerMovement.(buffsSettingsDatabase.buffsList[8].BuffInversion);
                changeWeaponBar.UseBuff(stimulantMaxCount);
                break;
            case ChangeWeaponBar.Buff.EnergeticMin:
                energeticMinCount -= 1;
                playerMovement.GetSpeed(buffsSettingsDatabase.buffsList[9].BuffSpeed);
                playerShooting.GetDamage(buffsSettingsDatabase.buffsList[9].BuffDamage);
                changeWeaponBar.UseBuff(energeticMinCount);
                break;
            case ChangeWeaponBar.Buff.EnergeticMid:
                energeticMidCount -= 1;
                playerMovement.GetSpeed(buffsSettingsDatabase.buffsList[10].BuffSpeed);
                changeWeaponBar.UseBuff(energeticMidCount);
                break;
            case ChangeWeaponBar.Buff.EnergeticMax:
                energeticMaxCount -= 1;
                playerMovement.GetSpeed(buffsSettingsDatabase.buffsList[11].BuffSpeed);
                changeWeaponBar.UseBuff(energeticMidCount);
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
