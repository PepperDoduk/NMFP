using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class K_UFO : MonoBehaviour
{
    // 기본 변수들
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly;
    public float flyHeight = 5f;
    public float flySpeed = 5f;
    public float shootingRange = 10f;

    // 총알 및 공격 관련 변수
    public GameObject enemybullet;
    public Transform spawnPoint;
    private bool isShooting = false;
    private float bulletTime;
    [SerializeField] private float time = 5f; // 발사 간격
    public int damageAmount = 10;

    // 리지드바디, 애니메이터
    private Rigidbody rb;
    public Animator ani;

    // 상태 체크 변수
    private bool isDead = false;

    // 이벤트
    public event System.Action OnDeath;

    void Start()
    {
        // 필요한 컴포넌트 초기화
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        target = playerObject?.transform;
        currentHp = Hp;

        // 비행 상태 설정
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
    }

    void Update()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        // HP가 0 이하일 때 사망 처리
        if (currentHp <= 0)
        {
            Die();
            return;
        }

        // 비행 상태에서의 이동
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
            Die();
        }
    }

    // 플레이어에게 데미지 주기
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Damage taken: {damage}. Current HP: {currentHp}"); // 현재 HP 로그 출력
        if (currentHp <= 0)
        {
            Die();
        }
    }

    // 사망 처리
    public void Die()
    {
        if (isDead) return;

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
                                    // 뒤로 넘어지는 힘을 추가 (Z축을 음수로 설정)
            rb.AddForce(new Vector3(0, -5f, 5f), ForceMode.Impulse); // 자연스러운 넘어짐 연출
        }

        if (ani != null)
        {
            ani.SetTrigger("Die"); // 죽음 애니메이션 트리거
        }

        TryDropItem();
        Destroy(gameObject, 2f); // 2초 후 삭제 (넘어지는 연출을 위해 지연)
    }

    // 아이템 드롭 시도
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

    // 플레이어 방향으로 이동
    void MoveTowardsPlayer()
    {
        if (!isDead && target != null)
        {
           // ani.SetBool("isWalking", true);
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * flySpeed * Time.deltaTime;
        }
    }

    // 플레이어를 향해 공격 (총알 발사)
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
}
