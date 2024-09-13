using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerState : MonoBehaviour
{
    [SerializeField] private int m_health;

    private void Start()
    {
        m_health = 100;
    }

    void TakeDamage(int pDamage)
    {
        m_health -= pDamage;
        if(m_health <= 0)
        {
            // 플레이어 사망, 게임 오버
        }
    }
}
