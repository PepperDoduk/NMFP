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
    public LineRenderer laserLine;
    public float laserDuration = 0.2f;

    // 레이저 두께를 설정하는 변수
    public float lineWidth = 0.1f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        laserLine.enabled = false;

        // LineRenderer 두께 설정
        laserLine.startWidth = lineWidth;
        laserLine.endWidth = lineWidth;
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

                laserLine.SetPosition(0, spawnPoint.position);
                laserLine.SetPosition(1, player.position);
                laserLine.enabled = true;

                RaycastHit hit;
                if (Physics.Raycast(spawnPoint.position, (player.position - spawnPoint.position).normalized, out hit, shootingRange))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        // 플레이어에게 데미지
                    }
                }

                yield return new WaitForSeconds(laserDuration);
                laserLine.enabled = false;
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
