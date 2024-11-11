using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class K_boss : MonoBehaviour
{
    // 공통 변수
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly; // 하늘에 떠 있을지 여부
    public float flyHeight = 5f; // 떠 있을 높이
    public float flySpeed = 5f; // 공중에서 이동 속도
    private bool isDead = false; // 적의 사망 상태를 추적하는 변수
    public Animator ani;
    private Rigidbody rb;
    public event Action OnDeath;

    // K_AlienTank 추가 변수
    public int Speed;
    private float bulletTime;
    public GameObject enemyBullet;
    public Transform bulletSpawnPoint;
    public float bulletSpeed;
    public float shootingRange = 10f;
    private bool isShooting = false;
    public int damageAmount = 10;

    // K_robotGun 추가 변수
    public Transform railgun;
    public Transform laserRifle;
    public GameObject railgunMuzzleFlashParticle;
    public GameObject laserRifleMuzzleFlashParticle;

    // K_bosscade 추가 변수
    public GameObject[] enemyPrefabs; // 소환할 적 프리팹 배열
    public float summonInterval = 10f;
    private float summonCooldown;
    public float spawnRadius = 5f; // 보스 주변 소환 반경

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindWithTag("Player");
        target = player?.transform;
        currentHp = Hp;

        if (fly)
        {
            agent.enabled = false;
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition;
        }
        else
        {
            agent.enabled = true;
        }

        if (Hp == 0)
        {
            Die();
        }

        StartCoroutine(AutomaticShooting());
    }

    void Update()
    {
        if (isDead) return;

        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (currentHp <= 0)
        {
            Die();
            return;
        }

        if (fly)
        {
            if (distance <= lookRadius)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * flySpeed * Time.deltaTime;
            }
        }
        else
        {
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
            }
        }

        // 소환 처리
        summonCooldown -= Time.deltaTime;
        if (summonCooldown <= 0)
        {
            SummonEnemy();
            summonCooldown = summonInterval;
        }

        // 사격 처리
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
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Damage taken: {damage}. Current HP: {currentHp}");
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        if (agent != null)
        {
            agent.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse); // 밑으로 떨어지는 힘
        }

        ani.SetTrigger("Die");

        TryDropItem();
        Destroy(gameObject, 2f);
    }

    void TryDropItem()
    {
        if (dropItems.Length == 0) return;

        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue <= dropChance)
        {
            int randomIndex = UnityEngine.Random.Range(0, dropItems.Length);
            GameObject dropItem = dropItems[randomIndex];
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }

    void SummonEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)];
        Vector3 spawnPosition = transform.position + (UnityEngine.Random.insideUnitSphere * spawnRadius);
        spawnPosition.y = transform.position.y;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    IEnumerator AutomaticShooting()
    {
        while (true)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= shootingRange)
            {
                ShootRailgun(target.position);
                yield return new WaitForSeconds(2f);
                ShootLaserRifle(target.position);
                yield return new WaitForSeconds(2f);
            }
            yield return null;
        }
    }

    public void ShootRailgun(Vector3 targetPosition)
    {
        GameObject particle = Instantiate(railgunMuzzleFlashParticle, railgun.position, Quaternion.identity);
        Destroy(particle, 1f);

        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, shootingRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                target.GetComponent<N_PlayerModel>()?.TakeDamage(damageAmount);
            }
        }
    }

    public void ShootLaserRifle(Vector3 targetPosition)
    {
        GameObject particle = Instantiate(laserRifleMuzzleFlashParticle, laserRifle.position, Quaternion.identity);
        Destroy(particle, 1f);

        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, shootingRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                target.GetComponent<N_PlayerModel>()?.TakeDamage(damageAmount);
            }
        }
    }

    IEnumerator ShootAtPlayer()
    {
        while (isShooting && !isDead)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime <= 0)
            {
                bulletTime = 2f;
                GameObject bulletObj = Instantiate(enemyBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                K_missilebullet bulletScript = bulletObj.GetComponent<K_missilebullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetTarget(target);
                }

                Destroy(bulletObj, 5f);
            }

            yield return null;
        }
    }
}
