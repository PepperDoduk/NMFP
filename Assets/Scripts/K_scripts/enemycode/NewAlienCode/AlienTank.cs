using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienTank : MonoBehaviour
{
    public float lookRadius = 10f;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    private float distance;
    public float Pdistance = 3f;

    public GameObject enemybullet;
    public float missileSpeed = 20f;
    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;

    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;

    private bool isFalling = true;
    private bool isDead = false;

    private Rigidbody rb;
    private Animator ani;
    private K_EnemyHp enemyHp;

    private enum State
    {
        Idle,
        MovingTowardsPlayer,
        ShootingAtPlayer,
        Dead
    }

    private State currentState;

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        player = GameObject.FindWithTag("Player")?.transform;
        enemyHp = GetComponent<K_EnemyHp>();

        if (enemyHp != null)
        {
            enemyHp.OnDeath += HandleDeath;
        }

        currentState = State.Idle;
    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        RunStateMachine();

        if (enemyHp.Hp <= 0)
        {
            currentState = State.Dead;
        }
    }

    void RunStateMachine()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.MovingTowardsPlayer:
                MoveTowardsPlayerState();
                break;
            case State.ShootingAtPlayer:
                ShootingState();
                break;
            case State.Dead:
                HandleDeath();
                break;
        }
    }

    void IdleState()
    {
        if (distance <= lookRadius)
        {
            currentState = State.MovingTowardsPlayer;
        }
    }

    void MoveTowardsPlayerState()
    {
        if (distance > shootingRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            currentState = State.ShootingAtPlayer;
        }
    }

    void ShootingState()
    {
        RotateTowardsPlayer();
        if (distance > shootingRange)
        {
            currentState = State.MovingTowardsPlayer;
        }
        else if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootAtPlayer());
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        ani.SetBool("isWalking", true);
        rb.velocity = direction * Speed;
        rb.angularVelocity = Vector3.zero;

        RotateTowardsPlayer();
        UpdateStop();
    }

    void UpdateStop()
    {
        if (distance < Pdistance)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    IEnumerator ShootAtPlayer()
    {
        while (isShooting && !isDead)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime <= 0)
            {
                bulletTime = time;
                GameObject bulletObj = Instantiate(enemybullet, spawnPoint.position, spawnPoint.rotation);
                K_missilebullet bulletScript = bulletObj.GetComponent<K_missilebullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetTarget(player);
                }

                Destroy(bulletObj, 5f);
            }

            yield return null;
        }
    }

    void HandleDeath()
    {
        isDead = true;
        rb.velocity = Vector3.zero;

        rb.constraints = RigidbodyConstraints.None;

        DropItemsOnDeath();

        Destroy(gameObject, 2f);
    }

    void DropItemsOnDeath()
    {
        if (dropItems != null && dropItems.Length > 0 && UnityEngine.Random.value <= dropChance)
        {
            GameObject itemToDrop = dropItems[UnityEngine.Random.Range(0, dropItems.Length)];
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }
}
