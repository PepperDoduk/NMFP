using System.Collections;
using UnityEngine;

public class K_AlienStormtrooper : MonoBehaviour
{
    public int Speed;
    [SerializeField] private float laserCooldown = 5f;
    private float laserTime;
    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;
    public GameObject muzzleFlashParticle;

    GameObject damage;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        damage = GameObject.Find("N_PlayerModel");
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
                StartCoroutine(ShootLaserAtPlayer());
            }
        }
        else
        {
            isShooting = false;
            MoveTowardsPlayer();
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
                        damage.GetComponent<N_PlayerModel>().TakeDamage(5);
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
