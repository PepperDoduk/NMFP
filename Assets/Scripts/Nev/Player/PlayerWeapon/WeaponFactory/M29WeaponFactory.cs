using System;
using UnityEngine;


[CreateAssetMenu(
    fileName = "M29",
    menuName = "Weapons/M29",
    order = int.MinValue)]
public class M29WeaponFactory : WeaponFactory
{
    public override IWeaponStrategy CreateWeaponStrategy()
    {
        return new M29WeaponStrategy(m_damage);
    }
}