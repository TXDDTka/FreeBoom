using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffObject : MonoBehaviour
{
    public enum BuffType
    {
        None, HealthMin, HealthMid, HealthMax, ShieldMin, ShieldMid, ShieldMax, StimulantMin,
        StimulantMid, StimulantMax, EnergeticMin, EnergeticMid, EnergeticMax
    }
    public BuffType buffType = BuffType.None;
}


