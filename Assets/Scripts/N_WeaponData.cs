using System.Collections;
using System.Collections.Generic;


public class N_WeaponData
{
    public string Name { get; }
    public float Damage { get; }
    public float FireRate { get; }
    public float Range { get; }
    public float ReroadSpeed { get; }


    public N_WeaponData(string pName,
        float pDamage, float pFireRate,
        float pRange, float pReroadSpeed)
    {
        Name = pName;
        Damage = pDamage;
        FireRate = pFireRate;
        Range = pRange;
        ReroadSpeed = pReroadSpeed;
    }
}
