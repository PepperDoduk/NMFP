using System.Collections;
using UnityEngine;

public class K_AlienStormtrooper : MonoBehaviour
{
    public float lookRadius = 10f;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    private float distance;
    public float Pdistance = 3f;

    [SerializeField] private float laserCooldown = 5f;
    private float laserTime;
    public float shootingRange = 10f;
    public GameObject muzzleFlashParticle;
    private bool isShooting = false;

    public int Speed;
    public Transform spawnPoint;
    public Transform player;
    private Animator ani;
    private Rigidbody rb;

    private bool isDead = false;
    private K_EnemyHp enemyHp; 

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
        enemyHp = GetComponent<K_EnemyHp>();

        if (enemyHp != null)
        {
            enemyHp.OnDeath += OnEnemyDeath; 
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
        }

        UpdateStop();

        if (distance <= shootingRange)
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootLaserAtPlayer());
            }
        }
        else
        {
            isShooting = false;
        }
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

    IEnumerator ShootLaserAtPlayer()
    {
        while (isShooting)
        {
            laserTime -= Time.deltaTime;
            if (laserTime <= 0)
            {
                laserTime = laserCooldown;
                GameObject particle = Instantiate(muzzleFlashParticle, spawnPoint.position, spawnPoint.rotation);
                Destroy(particle, 1f);

                if (Physics.Raycast(spawnPoint.position, (player.position - spawnPoint.position).normalized, out RaycastHit hit, shootingRange))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        GameObject damageObj = GameObject.Find("N_PlayerModel");
                        damageObj?.GetComponent<N_PlayerModel>()?.TakeDamage(5);
                    }
                }
            }
            yield return null;
        }
    }

    void OnEnemyDeath()
    {
        isDead = true;
        enemyHp.Die();
    }

  
}
