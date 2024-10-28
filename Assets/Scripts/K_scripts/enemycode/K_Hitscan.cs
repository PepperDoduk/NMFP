using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Hitscan : MonoBehaviour
{
    private void OnShoot()
    {
        Debug.Log("shoot");
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 30, Color.red, 2.0f); 

        if (Physics.Raycast(ray, out var hit, 30))
        {
            if (hit.collider.TryGetComponent<K_Enenmy>(out K_Enenmy enemy))//player
            {
                enemy.TakeDamage(10);

            }
        }
    }
}
