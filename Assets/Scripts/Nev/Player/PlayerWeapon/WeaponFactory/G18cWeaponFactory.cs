using System;
using UnityEngine;


[CreateAssetMenu(
    fileName = "G18c",
    menuName = "Weapons/G18c",
    order = int.MinValue)]
public class G18cWeaponFactory : WeaponFactory
{
    public override IWeaponStrategy CreateWeaponStrategy()
    {
        return new G18cWeaponStrategy(m_damage);
    }
}