using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class N_EnemyController : MonoBehaviour
{
     public N_EnemyStates states = new N_EnemyStates(new N_EnemyData(
        "Name",
        100,
        5,
        10,
        1
        ));

    private Rigidbody rigid;
    private NavMeshAgent nav;
    public Transform player;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = states.Data.MoveSpeed;
    }

    void Update()
    {
        nav.SetDestination(player.position);
        if(!states.IsAlive)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
