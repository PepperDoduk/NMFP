using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;

public class K_AlienScout : MonoBehaviour
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
    float distance;
    public float Pdistance = 3f;
    public event Action OnDeath;
    public Animator ani;
    private Rigidbody rb;
    private bool isDead = false; // ���� ��� ���¸� �����ϴ� ����

    // AlienScout ���� ����
    public int Speed; // AlienScout �ӵ�
    [SerializeField] private float time = 5f;
    private float bulletTime;
    public GameObject muzzleFlashParticle;
    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;

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
    }

    void Update()
    {
        if (isDead) return; // ���� ������ ������Ʈ �ߴ�

        if (target == null) return;

        distance = Vector3.Distance(target.position, transform.position);

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
        if(Hp==0)
        {
            Die();
        }

        UpdateStop();

        // ��� ó��
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
        if (isDead) return;

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

            // Rigidbody�� ȸ�� �߰�
            rb.AddTorque(new Vector3(0, 0, 5), ForceMode.Impulse); // ȸ�� �� �߰�
            rb.AddForce(new Vector3(0, -5f, -5f), ForceMode.Impulse); // �ڿ������� �Ѿ��� ����
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

                GameObject particle = Instantiate(muzzleFlashParticle, spawnPoint.position, spawnPoint.rotation);
                Destroy(particle, 0.1f);

                if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out RaycastHit hitInfo, shootingRange))
                {
                    if (hitInfo.collider.CompareTag("Player"))
                    {
                        // Assuming the player has a method to take damage
                       // damage.GetComponent<N_PlayerModel>().TakeDamage(5);
                    }
                }
            }
            yield return null;
        }
    }
}
