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
    float distance;
    public float Pdistance = 3f;
    public event Action OnDeath;
    public Animator ani;
    private Rigidbody rb;
    private bool isDead = false; // ���� ��� ���¸� �����ϴ� ����

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
        if (isDead) return; // ���� ������ ������Ʈ �ߴ�

        if (target == null) return;

        distance = Vector3.Distance(target.position, transform.position);

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
        UpdateStop();
        if(Hp==0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Damage taken: {damage}. Current HP: {currentHp}"); // ���� HP �α� ���
        if (currentHp <= 0)
        {
            Die();
        }
    }

    void UpdateStop()
    {
        if (isDead) return; // ���� ������ ������Ʈ �ߴ�

        if (!fly)
        {
            if (distance < Pdistance) agent.isStopped = true;
            if (distance > Pdistance) agent.isStopped = false;
        }
    }

    public void Die()
    {
        
        Debug.Log("Die method called"); // ����� �α� �߰�
        isDead = true; // ���� �׾��ٰ� ���� ������Ʈ

        OnDeath?.Invoke();

        if (agent != null)
        {
            agent.enabled = false; // NavMeshAgent ��Ȱ��ȭ
            Debug.Log("NavMeshAgent disabled"); // ����� �α� �߰�
        }

        if (rb != null)
        {
            rb.isKinematic = false; // �������� ��ȣ�ۿ� ���
            rb.useGravity = true;   // �߷� ����
        }

        // Rigidbody�� ȸ�� �߰�
        if (rb != null)
        {
            rb.AddTorque(new Vector3(0, 0, 5), ForceMode.Impulse); // ȸ�� �� �߰�
        }

        TryDropItem();
        Destroy(gameObject, 2f); // 2�� �� ���� (�Ѿ����� ������ ���� ����)
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
