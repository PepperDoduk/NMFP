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
    public GameObject enemybullet; // �̻��� ������
    public float missileSpeed = 20f;
    public int Speed;
    [SerializeField] private float time = 5f; // �Ѿ� �߻� ����
    private float bulletTime;  // �Ѿ� �߻� ��� �ð�
    
    public Transform spawnPoint;  // �Ѿ� �߻� ��ġ
    public float shootingRange = 10f; // ��� ����
    private Transform player;
    private bool isShooting = false;

    private bool isFalling = true;  // ���߿��� �������� �ִ��� ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject?.transform;
        target = player;
        currentHp = Hp;

        // �߷� ����
        rb.useGravity = true; // �߷��� ����ǵ��� ����

        // �ʱ� �ϴ� ��ġ���� ���������� ����
        Vector3 fallPosition = transform.position;
        fallPosition.y = 10f; // �ϴÿ��� �������� ���� ���� ����
        transform.position = fallPosition;

        isFalling = true; // ���߿��� �������� �ִٴ� �÷���
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

        // HP�� 0 ������ �� ��� ó��
        if (currentHp <= 0)
        {
            Die();
            return;
        }

        if (isFalling)
        {
            // ���߿��� �������� ���̶�� �߷��� ������ ������ ������
            if (transform.position.y <= 1f)  // �ٴڿ� ��������� �÷��̾� ���� ����
            {
                isFalling = false;  // ������ �Ϸ�, ���� ����
            }
            return;  // �������� ���ȿ��� �÷��̾� ������ ���� ����
        }

        // �߷¿� ���� ������ �� �÷��̾ �Ѵ� ����
        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
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

        // ���� ������ �ڷ� �Ѿ����� ȿ�� �߰�
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // �ڷ� �Ѿ������� ���� �ֱ�
            // ȸ��(��ũ) �߰�
            rb.AddTorque(new Vector3(0, 0, -5), ForceMode.Impulse);  // �ڷ� �Ѿ�� ȸ��

            // �ڷ� �и��� ���� �ֱ�
            rb.AddForce(new Vector3(0, -5f, 5f), ForceMode.Impulse);  // �ڷ� �и��� ��
        }

        TryDropItem();  // ������ ���

        Destroy(gameObject, 2f);  // 2�� �� ������Ʈ ����
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

            // Rigidbody�� �̵� ó��
            rb.velocity = direction * Speed * Time.deltaTime;
        }
    }

}
