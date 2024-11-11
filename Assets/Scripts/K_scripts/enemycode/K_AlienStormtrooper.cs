using System;
using System.Collections;
using UnityEngine;

public class K_AlienStormtrooper : MonoBehaviour
{
    public float lookRadius = 10f;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    private float distance;
    public float Pdistance = 3f;
    private bool isDead = false;

    [SerializeField] private float laserCooldown = 5f;
    public float laserTime;
    public float shootingRange = 10f;
    public GameObject muzzleFlashParticle;
    private bool isShooting = false;

    public int Speed;
    public Transform spawnPoint;
    public Transform player;
    public Animator ani;
    public Rigidbody rb;

    public event Action OnDeath;

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
        currentHp = Hp;

        // 물리 엔진 관련 설정 제거
        // Rigidbody는 이제 물리 영향을 받지 않도록 설정되지 않음.
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

        if (Hp <= 0) Die();

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
            // 물리적으로 정지하지 않게 하기 위해
            rb.velocity = Vector3.zero;  // 물리엔진의 영향을 받지 않도록 수동으로 속도 초기화
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) Die();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        // 죽을 때 물리 적용 없앴음 (추락 효과만 있음)
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

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // y축으로는 이동하지 않도록 설정
        direction.y = 0;

        // Rigidbody를 사용하여 이동, 회전 방지
        rb.velocity = direction * Speed;

        // 회전 방지
        rb.angularVelocity = Vector3.zero;  // 회전 방지
    }
}
