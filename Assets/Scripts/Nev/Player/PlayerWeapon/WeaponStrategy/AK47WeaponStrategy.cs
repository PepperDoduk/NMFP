using System;
using UnityEngine;


public class AK47WeaponStrategy : IWeaponStrategy
{
    private readonly int m_damage;

    public AK47WeaponStrategy(int pDamage)
    {
        m_damage = pDamage;
    }

    public void OnShoot()
    {
        Vector3 direction =
            Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 2) * Camera.main.transform.forward;

        Ray ray = new Ray(
            Camera.main.transform.position,
            direction);

        Debug.DrawRay(ray.origin, ray.direction * 30);

        if (Physics.Raycast(ray, out var hit, 30))
        {
            //if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
            //{
            //    enemy.TakeDamage(m_damage);
            //}
        }
    }
}