using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class K_AlienTank : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;
    public int Hp; // ���� HP
    private int currentHp; // ���� HP
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly; // �ϴÿ� �� ������ ����
    public float flyHeight = 5f; // �� ���� ����
    public float flySpeed = 5f; // ���߿��� �̵� �ӵ�
    public int Speed; // �̵� �ӵ�
    [SerializeField] private float time = 5f; // �߻� ����
    private float bulletTime; // �Ѿ� �߻� Ÿ�̸�
    public GameObject enemybullet; // �� �Ѿ�
    public Transform spawnPoint; // �Ѿ� �߻� ��ġ
    public float enemySpeed; // �� �Ѿ� �ӵ�
    public float shootingRange = 10f; // ��� �Ÿ�
    private Transform player; // �÷��̾��� Ʈ������
    private bool isShooting = false; // ��� ����
    public int damageAmount = 10; // �÷��̾�� �� ������
    public Animator ani; // �ִϸ�����
    private Rigidbody rb; // ������ٵ�
    private bool isDead = false; // ���� ��� ���¸� �����ϴ� ����

    public event Action OnDeath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject?.transform;
        target = player; // Ÿ���� �÷��̾�� ����
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
        if(Hp==0)
        {
            Die();
        }
    }

    void Update()
    {
        if (isDead) return; // ���� ������ ������Ʈ �ߴ�

        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        // HP�� 0 ������ �� ��� ó��
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
                direction.y = 0; // Y�� ���� ����
                transform.position += direction * flySpeed * Time.deltaTime; // ���߿��� Ÿ�� �������� �̵�
            }
        }
        else
        {
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position); // NavMeshAgent�� ���� �̵�
                MoveTowardsPlayer();
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
            Die() ;
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

    public void Die()
    {
        if (isDead) return;

        Debug.Log("Die method called"); // ����� �α� �߰�
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
            rb.AddForce(new Vector3(0, -5f, -5f), ForceMode.Impulse); // �ڿ������� �Ѿ��� ����
        }

        // ���� ������ �ִϸ��̼� ����
        if (ani != null)
        {
            ani.SetTrigger("Die"); // ���� �ִϸ��̼� Ʈ����
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
            //damage.GetComponent<N_PlayerModel>().TakeDamage(damageAmount); // �÷��̾�� ������ �ֱ�
        }
    }
}
