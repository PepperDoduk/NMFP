using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(
    fileName = "WeaponData",
    menuName = "Data/WeaponData",
    order = int.MinValue)]
public class N_WeaponData : ScriptableObject
{
    //public string Name;
    public float Damage;
    public float Range;
    public float RPM;
    public int MaxAmmo;
}
