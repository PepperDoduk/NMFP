using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class K_UFO : MonoBehaviour
{
    // �⺻ ������
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

    // �Ѿ� �� ���� ���� ����
    public GameObject enemybullet;
    public Transform spawnPoint;
    private bool isShooting = false;
    private float bulletTime;
    [SerializeField] private float time = 5f; // �߻� ����
    public int damageAmount = 10;

    // ������ٵ�, �ִϸ�����
    private Rigidbody rb;
    public Animator ani;

    // ���� üũ ����
    private bool isDead = false;

    // �̺�Ʈ
    public event System.Action OnDeath;

    void Start()
    {
        // �ʿ��� ������Ʈ �ʱ�ȭ
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        target = playerObject?.transform;
        currentHp = Hp;

        // ���� ���� ����
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
        if (isDead || target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        // HP�� 0 ������ �� ��� ó��
        if (currentHp <= 0)
        {
            Die();
            return;
        }

        // ���� ���¿����� �̵�
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

        // ��� ó��
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

    // �÷��̾�� ������ �ֱ�
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Damage taken: {damage}. Current HP: {currentHp}"); // ���� HP �α� ���
        if (currentHp <= 0)
        {
            Die();
        }
    }

    // ��� ó��
    public void Die()
    {
        if (isDead) return;

        isDead = true; // ���� �׾��ٰ� ���� ������Ʈ
        OnDeath?.Invoke();

        if (agent != null)
        {
            agent.enabled = false; // NavMeshAgent ��Ȱ��ȭ
        }

        if (rb != null)
        {
            rb.isKinematic = false; // �������� ��ȣ�ۿ� ���
            rb.useGravity = true;   // �߷� ����
                                    // �ڷ� �Ѿ����� ���� �߰� (Z���� ������ ����)
            rb.AddForce(new Vector3(0, -5f, 5f), ForceMode.Impulse); // �ڿ������� �Ѿ��� ����
        }

        if (ani != null)
        {
            ani.SetTrigger("Die"); // ���� �ִϸ��̼� Ʈ����
        }

        TryDropItem();
        Destroy(gameObject, 2f); // 2�� �� ���� (�Ѿ����� ������ ���� ����)
    }

    // ������ ��� �õ�
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

    // �÷��̾� �������� �̵�
    void MoveTowardsPlayer()
    {
        if (!isDead && target != null)
        {
           // ani.SetBool("isWalking", true);
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * flySpeed * Time.deltaTime;
        }
    }

    // �÷��̾ ���� ���� (�Ѿ� �߻�)
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
