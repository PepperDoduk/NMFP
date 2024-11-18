using System;
using System.Collections;
using UnityEngine;

public class K_AlienScout : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    private Rigidbody rb;
    private Animator ani;
    private Transform player;
    public float Pdistance = 3f;

    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;
    public GameObject muzzleFlashParticle;
    public Transform spawnPoint;
    public float shootingRange = 10f;
    private bool isShooting = false;

    private bool isFalling = true;
    private bool isDead = false;

    private K_EnemyHp enemyHp; // 체력 관리용 스크립트 참조
    private N_PlayerModel Hp;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject?.transform;
        target = player;

        enemyHp = GetComponent<K_EnemyHp>(); 

        rb.useGravity = true;
        Vector3 fallPosition = transform.position;
        fallPosition.y = 10f;
        transform.position = fallPosition;

        isFalling = true;

        if (enemyHp != null)
        {
            enemyHp.OnDeath += OnEnemyDeath; 
        }
    }

    void Update()
    {
        if (isDead) return;
        if (target == null) return;

        if (isFalling)
        {
            if (transform.position.y <= 1f)
            {
                isFalling = false;
            }
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
        }

        UpdateStop(distance);

        if (distance <= shootingRange)
        {
            if (!isShooting)
            {
                isShooting = true;
                ani.SetBool("isWalking", true);
                StartCoroutine(ShootAtPlayer());
            }
        }
        else
        {
            isShooting = false;
            MoveTowardsPlayer();
        }
    }

    void UpdateStop(float distance)
    {
        if (isDead) return;

        if (distance < Pdistance)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        if (!isDead && player != null)
        {
            ani.SetBool("isWalking", true);
            Vector3 direction = (player.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);

            rb.velocity = direction * Speed * Time.deltaTime;
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

                GameObject particle = Instantiate(muzzleFlashParticle, spawnPoint.position, spawnPoint.rotation);
                Destroy(particle, 0.1f);

                if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out RaycastHit hitInfo, shootingRange))
                {
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        var playerHp = hitInfo.collider.GetComponent<N_PlayerModel>();
                        if (playerHp != null)
                        {
                            playerHp.TakeDamage(10);
                        }
                        else
                        {
                            Debug.LogWarning("Player에서 N_PlayerModel을 찾을 수 없습니다.");
                        }
                    }
                }

            }
            yield return null;
        }
    }

    void OnEnemyDeath()
    {
        
        isDead = true;
        ani.SetBool("isWalking", false);
        enemyHp.Die();
    }
}
