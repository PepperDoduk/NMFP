using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class N_EnemyController : MonoBehaviour
{
    private Rigidbody rigid;
    private NavMeshAgent nav;
    public Transform player;

    public float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        nav.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(player.position);
    }
    private void FixedUpdate()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
}
