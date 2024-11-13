using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_boss : MonoBehaviour
{
 
    public float lookRadius = 10f;
    public Transform target;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly; 
    public float flyHeight = 5f; 
    public float flySpeed = 5f; 
    private bool isDead = false; 
    public Animator ani;
    private Rigidbody rb;
    public event Action OnDeath;


    public int Speed;
    private float bulletTime;
    public GameObject enemyBullet;
    public Transform bulletSpawnPoint;
    public float bulletSpeed;
    public float shootingRange = 10f;
    private bool isShooting = false;
    public int damageAmount = 10;


    public Transform railgun;
    public Transform laserRifle;
    public GameObject railgunMuzzleFlashParticle;
    public GameObject laserRifleMuzzleFlashParticle;


    public GameObject[] enemyPrefabs; 
    public float summonInterval = 10f;
    private float summonCooldown;
    public float spawnRadius = 5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject player = GameObject.FindWithTag("Player");
        target = player?.transform;
        currentHp = Hp;

        if (fly)
        {
           
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition;
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

      
        float distance = Mathf.Abs(target.position.x - transform.position.x);

        if (currentHp <= 0)
        {
            Die();
            return;
        }

      
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

        if (fly)
        {
            if (distance <= lookRadius)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * flySpeed * Time.deltaTime;
            }

        
            Vector3 lookDirection = target.position - transform.position;
            lookDirection.y = 0; 
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }

     
        summonCooldown -= Time.deltaTime;
        if (summonCooldown <= 0)
        {
            SummonEnemy();
            summonCooldown = summonInterval;
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

        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse); 
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
