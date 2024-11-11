using System.Collections;
using UnityEngine;
using System;

public class K_AlienTank : MonoBehaviour
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
    public GameObject enemybullet; // 미사일 프리팹
    public float missileSpeed = 20f;
    public int Speed;
    [SerializeField] private float time = 5f; // 총알 발사 간격
    private float bulletTime;  // 총알 발사 대기 시간
    
    public Transform spawnPoint;  // 총알 발사 위치
    public float shootingRange = 10f; // 사격 범위
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
        if (Hp == 0)
        {
            Die();
        }
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
                    bulletScript.SetTarget(target);
                }

                Destroy(bulletObj, 5f);
            }

            yield return null;
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

        // 적이 죽으면 뒤로 넘어지는 효과 추가
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // 뒤로 넘어지도록 힘을 주기
            // 회전(토크) 추가
            rb.AddTorque(new Vector3(0, 0, -5), ForceMode.Impulse);  // 뒤로 넘어가는 회전

            // 뒤로 밀리는 힘을 주기
            rb.AddForce(new Vector3(0, -5f, 5f), ForceMode.Impulse);  // 뒤로 밀리는 힘
        }

        TryDropItem();  // 아이템 드롭

        Destroy(gameObject, 2f);  // 2초 후 오브젝트 삭제
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

}
