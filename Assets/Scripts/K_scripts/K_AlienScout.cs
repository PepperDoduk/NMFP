using System.Collections;
using UnityEngine;

public class K_AlienScout : MonoBehaviour
{
    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;
    public GameObject enemybullet;
    public Transform spawnPoint;
    public float enemySpeed;
    public float shootingRange = 10f; 
    private Transform player;
    private bool isShooting = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
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
    }

    IEnumerator ShootAtPlayer()
    {
        while (isShooting)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime <= 0)
            {
                bulletTime = time;
                GameObject bulletObj = Instantiate(enemybullet, spawnPoint.position, spawnPoint.rotation);
                K_Bullet bulletScript = bulletObj.GetComponent<K_Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetTarget(player);
                }
                Destroy(bulletObj, 5f);
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
