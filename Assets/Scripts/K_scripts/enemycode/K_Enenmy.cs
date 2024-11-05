using UnityEngine;
using UnityEngine.AI;
using System;

public class K_Enenmy : MonoBehaviour
{
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
    float distance;
    public float Pdistance = 3f;
    public event Action OnDeath;
    public Animator ani;
    private Rigidbody rb;
    private bool isDead = false; // 적의 사망 상태를 추적하는 변수

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
        if (isDead) return; // 적이 죽으면 업데이트 중단

        if (target == null) return;

        distance = Vector3.Distance(target.position, transform.position);

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
        UpdateStop();
        if(Hp==0)
        {
            Die();
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

    void UpdateStop()
    {
        if (isDead) return; // 적이 죽으면 업데이트 중단

        if (!fly)
        {
            if (distance < Pdistance) agent.isStopped = true;
            if (distance > Pdistance) agent.isStopped = false;
        }
    }

    public void Die()
    {
        
        Debug.Log("Die method called"); // 디버그 로그 추가
        isDead = true; // 적이 죽었다고 상태 업데이트

        OnDeath?.Invoke();

        if (agent != null)
        {
            agent.enabled = false; // NavMeshAgent 비활성화
            Debug.Log("NavMeshAgent disabled"); // 디버그 로그 추가
        }

        if (rb != null)
        {
            rb.isKinematic = false; // 물리적인 상호작용 허용
            rb.useGravity = true;   // 중력 적용
        }

        // Rigidbody에 회전 추가
        if (rb != null)
        {
            rb.AddTorque(new Vector3(0, 0, 5), ForceMode.Impulse); // 회전 힘 추가
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
}
