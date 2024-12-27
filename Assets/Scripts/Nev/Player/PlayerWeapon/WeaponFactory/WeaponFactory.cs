using System;
using UnityEngine;


public abstract class WeaponFactory : ScriptableObject
{
    [SerializeField] protected GameObject m_weaponPrefab;
    [SerializeField] protected int m_maxBullet;
    [SerializeField] protected float m_RPM;
    [SerializeField] protected int m_damage;

    public WeaponData CreateWeaponData()
    {
        return new WeaponData(
            m_weaponPrefab,
            m_maxBullet,
            m_RPM);
    }
    public abstract IWeaponStrategy CreateWeaponStrategy();
}