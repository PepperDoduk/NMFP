using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_EnemyStates : MonoBehaviour
{
    public float moveSpeed;
    public float health;

    public float maxAttackDistance;
    public float attackSpeed;

    private void Awake()
    {
        moveSpeed = 5f;
        health = 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
