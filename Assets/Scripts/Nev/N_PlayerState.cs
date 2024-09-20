using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerState : MonoBehaviour
{
    [SerializeField] private int health;

    private void Start()
    {
        health = 100;
    }

    public void TakeDamage(int pDamage)
    {
        health -= pDamage;
        if(health <= 0)
        {
            // 플레이어 사망, 게임 오버
            Destroy(gameObject);
        }
    }
}
