using System;
using UnityEngine;


public abstract class WeaponFactory : ScriptableObject
{
    [SerializeField] protected int m_maxBullet;
    [SerializeField] protected int m_damage;

    public WeaponData CreateWeaponData()
    {
        return new WeaponData(
            m_maxBullet);
    }
    public abstract IWeaponStrategy CreateWeaponStrategy();
}