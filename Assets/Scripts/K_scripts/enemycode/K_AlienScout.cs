using System.Collections;
using UnityEngine;

public class K_AlienScout : MonoBehaviour
{
    public int Speed;
    [SerializeField] private float time = 5f;
    private float bulletTime;
    public GameObject muzzleFlashParticle;
    public Transform spawnPoint;
    public float shootingRange = 10f;
    private Transform player;
    private bool isShooting = false;
    private bool isDead = false;
    private Animator animator;
    private SpriteRenderer spriterendere;

    GameObject damage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        damage = GameObject.Find("N_PlayerModel");
      
    }

    void Update()
    {
        if (isDead || player == null) return;

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

    public void Die()
    {
        isDead = true;
        StopAllCoroutines();
        transform.Rotate(new Vector3(0, 0, -90));
    }
}
