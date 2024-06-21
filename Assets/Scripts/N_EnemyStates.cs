using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_EnemyStates : MonoBehaviour
{
    public float moveSpeed;
    public float health;

    public float maxAttackDistance;
    public float attackSpeed;


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
