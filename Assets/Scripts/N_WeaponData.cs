using System.Collections;
using System.Collections.Generic;


public class N_WeaponData
{
    public string Name { get; }
    public float Damage { get; }
    public float Range { get; }
    public float FireRate { get; }
    public int ReroadSpeed { get; }
    public int MaxAmmo { get; }


    public N_WeaponData(string pName, float pDamage,
        float pRange, float pFireRate,
        int pMaxAmmo, int pReroadSpeed)
    {
        Name = pName;
        Damage = pDamage;
        Range = pRange;
        FireRate = pFireRate;
        MaxAmmo = pMaxAmmo;
        ReroadSpeed = pReroadSpeed;
    }
}
