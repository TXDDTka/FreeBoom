using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffObject : MonoBehaviour
{
    public enum BuffType { None, FirstAid, Shield, Potions }
    public BuffType buffType = BuffType.None;
}
