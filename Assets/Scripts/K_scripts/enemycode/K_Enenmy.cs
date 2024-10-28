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
    public bool fly; // �ϴÿ� �� ������ ����
    public float flyHeight = 5f; // �� ���� ����
    public float flySpeed = 5f; // ���߿��� �̵� �ӵ�

    public event Action OnDeath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindWithTag("Player");
        target = player?.transform;
        currentHp = Hp;

        if (fly)
        {
            agent.enabled = false; // NavMeshAgent ��Ȱ��ȭ
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition; // ���߿� ����
        }
        else
        {
            agent.enabled = true; // NavMeshAgent Ȱ��ȭ
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (fly)
        {
            if (distance <= lookRadius)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0; // Y�� ���� ����
                transform.position += direction * flySpeed * Time.deltaTime; // ���߿��� Ÿ�� �������� �̵�
            }
        }
        else
        {
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position); // NavMeshAgent�� ���� �̵�
            }
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
        OnDeath?.Invoke();
        TryDropItem();
        Destroy(gameObject);
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
