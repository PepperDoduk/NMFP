using System;
using UnityEngine;


[CreateAssetMenu(
    fileName = "G18c",
    menuName = "Weapons/G18c",
    order = int.MinValue)] //의존성 주입
public class G18cWeaponFactory : WeaponFactory
{
    public override IWeaponStrategy CreateWeaponStrategy()
    {
        return new G18cWeaponStrategy(m_damage);
    }
}