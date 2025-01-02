using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AlienScout : MonoBehaviour
{
    public float lookRadius = 10f;
    public GameObject muzzleFlashParticle;
    public float shootingRange = 10f;
    public float Pdistance = 3f;
    [SerializeField] private float laserCooldown = 5f;
    private float laserTime;
    private bool isShooting = false;
    public int Speed;
    public Transform spawnPoint;
    private Rigidbody rb;
    private Transform player;
    private K_EnemyHp enemyHp;
    private bool isDead = false;
    public float distance = 10;
    private enum State
    {
        Idle,
        MovingTowardsPlayer,
        ShootingAtPlayer,
        Dead
    }

    private State currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        player = GameObject.FindWithTag("Player")?.transform;
        enemyHp = GetComponent<K_EnemyHp>();

        if (enemyHp != null)
        {
            enemyHp.OnDeath += HandleDeath;
        }

        currentState = State.Idle;
    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        if (enemyHp.Hp <= 0)
        {
            enemyHp.Die();
        }

        RunBT();
    }

    void RunBT()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.MovingTowardsPlayer:
                MoveTowardsPlayer();
                break;
            case State.ShootingAtPlayer:
                ShootAtPlayer();
                break;
            case State.Dead:
                HandleDeath();
                break;
        }
    }

    void IdleState()
    {
        if (distance <= lookRadius)
        {
            currentState = State.MovingTowardsPlayer;
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        rb.velocity = direction * Speed;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (distance <= shootingRange)
        {
            currentState = State.ShootingAtPlayer;
        }
        else if (distance > lookRadius)
        {
            currentState = State.Idle;
        }
    }

    void ShootAtPlayer()
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
                    GameObject damageObj = GameObject.Find("PlayerModel");
                    damageObj?.GetComponent<PlayerModel>()?.TakeDamage(5);
                }
            }
        }

        if (distance > shootingRange)
        {
            currentState = State.MovingTowardsPlayer;
        }
    }

    void HandleDeath()
    {
        isDead = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;

        Destroy(gameObject, 2f);
    }
}
