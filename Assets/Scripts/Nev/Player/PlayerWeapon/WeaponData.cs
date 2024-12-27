using System;
using UnityEngine;


public class WeaponData
{
    public GameObject WeaponPrefab { get; }
    public int MaxBullet { get; }
    public float RPM { get; }

    public WeaponData(GameObject pWeaponPrefab,
        int maxBullet, float rpm)
    {
        WeaponPrefab = pWeaponPrefab;
        MaxBullet = maxBullet;
        RPM = rpm;
    }
}