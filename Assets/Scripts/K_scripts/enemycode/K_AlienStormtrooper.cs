using System;
using System.Collections;
using UnityEngine;

public class K_AlienStormtrooper : MonoBehaviour
{
    public float lookRadius = 10f;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
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
    public Animator ani;
    public Rigidbody rb;

    public event Action OnDeath;

    void Start()
    {
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player")?.transform;
        currentHp = Hp;


    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
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
        }
    }

    void UpdateStop()
    {
        if (distance < Pdistance)
        {
            rb.velocity = Vector3.zero;
        }
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


        direction.y = 0;


        rb.velocity = direction * Speed;


        rb.angularVelocity = Vector3.zero;
    }
}
