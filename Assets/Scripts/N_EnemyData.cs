using System.Collections;
using System.Collections.Generic;


public class N_EnemyData { 
    public string Name { get; }
    public float Health { get; }
    public float MoveSpeed { get; }
    public float AttackSpeed { get; }
    public float MaxAttackDistance { get; }


    public N_EnemyData(string pName,
        float pHealth, float pMoveSpeed,
        float pAttackSpeed, float pMaxAttackDistance)
    {
        Name = pName;
        Health = pHealth;
        MoveSpeed = pMoveSpeed;
        AttackSpeed = pAttackSpeed;
        MaxAttackDistance = pMaxAttackDistance;
    }

}
