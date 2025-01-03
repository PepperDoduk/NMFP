using System.Collections;
using UnityEngine;

public class AlienStormtrooper : MonoBehaviour
{
    public float lookRadius = 10f;
    public GameObject muzzleFlashParticle;
    public float shootingRange = 10f;
    private float distance;
    public float Pdistance = 3f;

    [SerializeField] private float laserCooldown = 5f;
    private float laserTime;
    private bool isShooting = false;

    public int Speed;
    public Transform spawnPoint;
    private Animator ani;
    private Rigidbody rb;
    private Transform player;

    private bool isDead = false;
    private K_EnemyHp enemyHp;

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
        ani = GetComponent<Animator>();
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

        RunStateMachine();

        if (enemyHp.Hp <= 0)
        {
            currentState = State.Dead;
        }
    }

    void RunStateMachine()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.MovingTowardsPlayer:
                MoveTowardsPlayerState();
                break;
            case State.ShootingAtPlayer:
                ShootingState();
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

    void MoveTowardsPlayerState()
    {
        if (distance > shootingRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            currentState = State.ShootingAtPlayer;
        }
    }

    void ShootingState()
    {
        RotateTowardsPlayer();
        if (distance > shootingRange)
        {
            currentState = State.MovingTowardsPlayer;
        }
        else if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(ShootLaserAtPlayer());
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        
        rb.velocity = direction * Speed;
        rb.angularVelocity = Vector3.zero;

        RotateTowardsPlayer();
        UpdateStop();
    }

    void UpdateStop()
    {
        if (distance < Pdistance)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    IEnumerator ShootLaserAtPlayer()
    {
        while (isShooting && currentState == State.ShootingAtPlayer)
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
            yield return null;
        }

        isShooting = false;
    }

    void HandleDeath()
    {
        isDead = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        

        Destroy(gameObject, 2f);
    }
}
