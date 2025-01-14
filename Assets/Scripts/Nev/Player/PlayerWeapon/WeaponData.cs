using System;
using UnityEngine;


public class WeaponData
{
    public int MaxBullet { get; }

    public WeaponData(
        int maxBullet)
    {
        MaxBullet = maxBullet;
    }
}