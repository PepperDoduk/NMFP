using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class K_AlienTank : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;
    public int Hp; // 기존 HP
    private int currentHp; // 현재 HP
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly; // 하늘에 떠 있을지 여부
    public float flyHeight = 5f; // 떠 있을 높이
    public float flySpeed = 5f; // 공중에서 이동 속도
    public int Speed; // 이동 속도
    [SerializeField] private float time = 5f; // 발사 간격
    private float bulletTime; // 총알 발사 타이머
    public GameObject enemybullet; // 적 총알
    public Transform spawnPoint; // 총알 발사 위치
    public float enemySpeed; // 적 총알 속도
    public float shootingRange = 10f; // 사격 거리
    private Transform player; // 플레이어의 트랜스폼
    private bool isShooting = false; // 사격 여부
    public int damageAmount = 10; // 플레이어에게 줄 데미지
    public Animator ani; // 애니메이터
    private Rigidbody rb; // 리지드바디
    private bool isDead = false; // 적의 사망 상태를 추적하는 변수

    public event Action OnDeath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject?.transform;
        target = player; // 타겟을 플레이어로 설정
        currentHp = Hp;

        if (fly)
        {
            agent.enabled = false; // NavMeshAgent 비활성화
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition; // 공중에 띄우기
        }
        else
        {
            agent.enabled = true; // NavMeshAgent 활성화
        }
        if(Hp==0)
        {
            Die();
        }
    }

    void Update()
    {
        if (isDead) return; // 적이 죽으면 업데이트 중단

        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        // HP가 0 이하일 때 사망 처리
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
                direction.y = 0; // Y축 변경 방지
                transform.position += direction * flySpeed * Time.deltaTime; // 공중에서 타겟 방향으로 이동
            }
        }
        else
        {
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position); // NavMeshAgent로 지상 이동
                MoveTowardsPlayer();
            }
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
            MoveTowardsPlayer();
        }
        if(Hp==0)
        {
            Die() ;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Damage taken: {damage}. Current HP: {currentHp}"); // 현재 HP 로그 출력
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        Debug.Log("Die method called"); // 디버그 로그 추가
        isDead = true; // 적이 죽었다고 상태 업데이트

        OnDeath?.Invoke();

        if (agent != null)
        {
            agent.enabled = false; // NavMeshAgent 비활성화
        }

        if (rb != null)
        {
            rb.isKinematic = false; // 물리적인 상호작용 허용
            rb.useGravity = true;   // 중력 적용
            rb.AddForce(new Vector3(0, -5f, -5f), ForceMode.Impulse); // 자연스러운 넘어짐 연출
        }

        // 적이 죽으면 애니메이션 실행
        if (ani != null)
        {
            ani.SetTrigger("Die"); // 죽음 애니메이션 트리거
        }

        TryDropItem();
        Destroy(gameObject, 2f); // 2초 후 삭제 (넘어지는 연출을 위해 지연)
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
            transform.position += direction * Speed * Time.deltaTime;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //damage.GetComponent<N_PlayerModel>().TakeDamage(damageAmount); // 플레이어에게 데미지 주기
        }
    }
}
