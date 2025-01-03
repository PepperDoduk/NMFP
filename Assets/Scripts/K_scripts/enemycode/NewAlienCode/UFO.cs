using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
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

    public bool fly;
    public float flyHeight = 5f;
    public float flySpeed = 5f;

    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;

    private bool isFalling = true;
    private bool isDead = false;

    private Rigidbody rb;
    private Animator ani;
    private K_EnemyHp enemyHp;
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
        if (fly)
        {
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition;

            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
            LookAtPlayer();
        }

        UpdateStop();

        if (distance <= shootingRange)
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootAtPlayer());
            }
        }
        else
        {
            isShooting = false;
        }

        if (enemyHp.Hp <= 0)
        {
            enemyHp.Die();
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void UpdateStop()
    {
        if (distance < Pdistance)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        rb.velocity = direction * Speed;
        rb.angularVelocity = Vector3.zero;
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

        Destroy(gameObject, 2f);
    }
}
