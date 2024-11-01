using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerModel : MonoBehaviour, IPlayerModel
{
    [SerializeField] private N_PlayerData m_playerData;

    public int m_currentHealth;

    public int MaxHealth => m_playerData.MaxHealth;
    public float MoveSpeed => m_playerData.MoveSpeed;
    public float JumpSpeed => m_playerData.JumpSpeed;
    public float ReloadSpeed => m_playerData.ReloadSpeed;


    public void Awake()
    {
        m_currentHealth = MaxHealth;
    }

    public void TakeDamage(int pDamage)
    {
        m_currentHealth -= pDamage;
        if(m_currentHealth <= 0)
        {
            // 플레이어 사망, 게임 오버
            Destroy(gameObject);
        }
    }
}
