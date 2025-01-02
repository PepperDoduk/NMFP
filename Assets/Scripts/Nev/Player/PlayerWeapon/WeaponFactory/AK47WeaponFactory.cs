using System;
using UnityEngine;


[CreateAssetMenu(
    fileName = "AK47",
    menuName = "Weapons/AK47",
    order = int.MinValue)]
public class AK47WeaponFactory : WeaponFactory
{
    public override IWeaponStrategy CreateWeaponStrategy()
    {
        return new AK47WeaponStrategy(m_damage);
    }
}