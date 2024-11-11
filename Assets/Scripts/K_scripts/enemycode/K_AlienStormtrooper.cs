using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class K_AlienStormtrooper : MonoBehaviour
{
    public float lookRadius = 10f;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly;
    public float flyHeight = 5f;
    public float flySpeed = 5f;
    private float distance;
    public float Pdistance = 3f;
    private bool isDead = false;

    [SerializeField] private float laserCooldown = 5f;
    public float laserTime;
    public float shootingRange = 10f;
    public GameObject muzzleFlashParticle;
    private bool isShooting = false;

    public int Speed;
    public Transform spawnPoint;
    public Transform player;
    public NavMeshAgent agent;
    public Animator ani;
    public Rigidbody rb;

    public event Action OnDeath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
        currentHp = Hp;

        if (fly)
        {
            agent.enabled = false;
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition;
        }
        else
        {
            agent.enabled = true;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        if (fly)
        {
            if (distance <= lookRadius)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                transform.position += direction * flySpeed * Time.deltaTime;
            }
        }
        else
        {
            if (distance <= lookRadius) agent.SetDestination(player.position);
        }

        UpdateStop();
        if (Hp <= 0) Die();

        if (distance <= shootingRange)
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootLaserAtPlayer());
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

    void UpdateStop()
    {
        if (!fly && distance < Pdistance)
            agent.isStopped = true;
        else
            agent.isStopped = false;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) Die();
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        if (agent != null) agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddTorque(new Vector3(0, 0, 5), ForceMode.Impulse);
            rb.AddForce(new Vector3(0, -5f, -5f), ForceMode.Impulse); // 뒤로 넘어지도록 힘 추가
        }

        TryDropItem();
        Destroy(gameObject, 2f);
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

    IEnumerator ShootLaserAtPlayer()
    {
        while (isShooting)
        {
            laserTime -= Time.deltaTime;
            if (laserTime <= 0)
            {
                laserTime = laserCooldown;
                GameObject particle = Instantiate(muzzleFlashParticle, spawnPoint.position, spawnPoint.rotation);
                Destroy(particle, 1f);

                if (Physics.Raycast(spawnPoint.position, (player.position - spawnPoint.position).normalized, out RaycastHit hit, shootingRange))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        GameObject damageObj = GameObject.Find("N_PlayerModel");
                        damageObj?.GetComponent<N_PlayerModel>()?.TakeDamage(5);
                    }
                }
            }
            yield return null;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * Speed * Time.deltaTime;
    }
}
