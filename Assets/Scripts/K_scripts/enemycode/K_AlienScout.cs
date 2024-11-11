using System.Collections;
using UnityEngine;
using System;

public class K_AlienScout : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    float distance;
    public float Pdistance = 3f;
    public event Action OnDeath;
    public Animator ani;
    private Rigidbody rb;
    private bool isDead = false;

    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;
    public GameObject muzzleFlashParticle;
    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;

    private bool isFalling = true;  // 공중에서 떨어지고 있는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject?.transform;
        target = player;
        currentHp = Hp;

        // 중력 적용
        rb.useGravity = true; // 중력은 적용되도록 설정

        // 초기 하늘 위치에서 떨어지도록 설정
        Vector3 fallPosition = transform.position;
        fallPosition.y = 10f; // 하늘에서 떨어지기 위한 높이 설정
        transform.position = fallPosition;

        isFalling = true; // 공중에서 떨어지고 있다는 플래그
    }

    void Update()
    {
        if (isDead) return;
        if (target == null) return;

        distance = Vector3.Distance(target.position, transform.position);

        // HP가 0 이하일 때 사망 처리
        if (currentHp <= 0)
        {
            Die();
            return;
        }

        if (isFalling)
        {
            // 공중에서 떨어지는 중이라면 중력의 영향을 받으며 내려옴
            if (transform.position.y <= 1f)  // 바닥에 가까워지면 플레이어 추적 시작
            {
                isFalling = false;  // 떨어짐 완료, 추적 시작
            }
            return;  // 떨어지는 동안에는 플레이어 추적을 하지 않음
        }

        // 중력에 의해 내려온 후 플레이어를 쫓는 동작
        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
        }

        UpdateStop();

        // 사격 처리
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
        if(Hp==0)
        {
            Die();
        }
    }

    void UpdateStop()
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

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
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
            rb.AddTorque(new Vector3(0, 0, 5), ForceMode.Impulse);
            rb.AddForce(new Vector3(0, -5f, -5f), ForceMode.Impulse);
        }

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

    void MoveTowardsPlayer()
    {
        if (!isDead && player != null)
        {
            ani.SetBool("isWalking", true);
            Vector3 direction = (player.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);

            // Rigidbody로 이동 처리
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
                        // Assuming the player has a method to take damage
                    }
                }
            }
            yield return null;
        }
    }
}
