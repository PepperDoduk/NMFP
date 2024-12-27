using System;
using UnityEngine;


public class WeaponData
{
    public int MaxBullet { get; }
    public float RPM { get; }

    public WeaponData(
        int maxBullet, float rpm)
    {
        MaxBullet = maxBullet;
        RPM = rpm;
    }
}