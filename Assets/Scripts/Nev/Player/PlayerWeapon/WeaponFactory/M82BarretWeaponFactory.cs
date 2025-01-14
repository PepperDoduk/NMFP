using System;
using UnityEngine;


[CreateAssetMenu(
    fileName = "M82Barret",
    menuName = "Weapons/M82Barret",
    order = int.MinValue)]
public class M82BarretWeaponFactory : WeaponFactory
{
    public override IWeaponStrategy CreateWeaponStrategy()
    {
        return new M82BarretWeaponStrategy(m_damage);
    }
}