using System.Collections;
using UnityEngine;

public class K_UFO : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly;
    public float flyHeight = 5f;
    public float flySpeed = 5f;
    public float shootingRange = 10f;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    private bool isShooting = false;
    private float bulletTime;
    [SerializeField] private float time = 5f;

    private Rigidbody rb;
    public Animator ani;

    private K_EnemyHp enemyHp;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        enemyHp = GetComponent<K_EnemyHp>();
        enemyHp.OnDeath += HandleDeath;
        

        GameObject playerObject = GameObject.FindWithTag("Player");
        target = playerObject?.transform;

        if (fly)
        {
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition;

            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    void Update()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (fly && distance <= lookRadius)
        {
            MoveTowardsTarget(distance);
        }

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
        }
    }

    private void MoveTowardsTarget(float distance)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * flySpeed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void HandleDeath()
    {
        isDead = true;

        enemyHp.Die();

        if (ani != null)
        {
            ani.SetTrigger("Die");
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
                GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);
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
