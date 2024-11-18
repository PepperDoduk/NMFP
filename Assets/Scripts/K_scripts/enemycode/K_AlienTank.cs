using System.Collections;
using UnityEngine;

public class K_AlienTank : MonoBehaviour
{
    public float lookRadius = 10f;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    private float distance;
    public float Pdistance = 3f;

    public GameObject enemybullet;
    public float missileSpeed = 20f;
    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;

    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;

    private bool isFalling = true;
    private bool isDead = false;

    private Rigidbody rb;
    private Animator ani;
    private K_EnemyHp enemyHp; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject?.transform;
        enemyHp = GetComponent<K_EnemyHp>();

        if (enemyHp != null)
        {
            enemyHp.OnDeath += OnEnemyDeath; 
        }

        rb.useGravity = true;

        Vector3 fallPosition = transform.position;
        fallPosition.y = 10f;
        transform.position = fallPosition;

        isFalling = true;
    }

    void Update()
    {
        if (isDead || player == null) return;

        distance = Vector3.Distance(player.position, transform.position);

        if (isFalling)
        {
            if (transform.position.y <= 1f)
            {
                isFalling = false;
            }
            return;
        }

        if (distance <= lookRadius)
        {
            MoveTowardsPlayer();
        }

        UpdateStop();

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
                    bulletScript.SetTarget(player);
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

    void OnEnemyDeath()
    {
        isDead = true;

       
       
        enemyHp.Die();
        Destroy(gameObject, 2f);
    }

   
    void MoveTowardsPlayer()
    {
        if (!isDead && player != null)
        {
            ani.SetBool("isWalking", true);
            Vector3 direction = (player.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * Speed);

            rb.velocity = direction * Speed * Time.deltaTime;
        }
    }
}
