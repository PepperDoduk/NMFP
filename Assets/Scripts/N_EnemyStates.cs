using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_EnemyStates
{
    public N_EnemyData Data { get; }
    public float Health { get; private set; }
    public bool IsAlive => Health > 0;


    public N_EnemyStates(N_EnemyData pData)
    {
        Data = pData;
        Health = Data.Health;
    }

    public void TakeDamage(int pDamage)
    {
        Health -= pDamage;
        if (Health < 0)
            Health = 0;
    }
}
