using System.Collections;
using UnityEngine;

public class K_boss : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly;
    public float flyHeight = 5f;
    public float flySpeed = 5f;
    public float shootingRange = 10f;
    public GameObject enemyBullet;
    public Transform bulletSpawnPoint;
    public GameObject[] enemyPrefabs;
    public float summonInterval = 10f;
    public float spawnRadius = 5f;
    public Transform railgun;
    public Transform laserRifle;
    public GameObject railgunMuzzleFlashParticle;
    public GameObject laserRifleMuzzleFlashParticle;
    public int damageAmount = 10;

    private K_EnemyHp enemyHp;
    private Rigidbody rb;
    private Animator ani;
    private bool isShooting = false;
    private float summonCooldown;
    private bool isDead = false;

    void Start()
    {
        enemyHp = GetComponent<K_EnemyHp>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject player = GameObject.FindWithTag("Player");
        target = player?.transform;

        if (fly)
        {
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition;
        }

        if (enemyHp != null)
        {
            enemyHp.OnDeath += Die;
        }

        StartCoroutine(AutomaticShooting());
    }

    void Update()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= shootingRange && !isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootAtPlayer());
        }
        else if (distance > shootingRange)
        {
            isShooting = false;
        }

        if (fly && distance <= lookRadius)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0;
            transform.position += direction * flySpeed * Time.deltaTime;

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

    private void Die()
    {
        
        ani.SetTrigger("Die");
        enemyHp.Die();
        Destroy(gameObject, 2f);
    }

  
    void SummonEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
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
            GameObject bullet = Instantiate(enemyBullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            K_missilebullet missile = bullet.GetComponent<K_missilebullet>();
            if (missile != null)
            {
                missile.SetTarget(target);
            }
            Destroy(bullet, 5f);
            yield return new WaitForSeconds(2f);
        }
    }
}
