using System.Collections;
using UnityEngine;

public class K_robotGun : MonoBehaviour
{
    public float shootingRange = 10f; // 발사 거리
    public float shootInterval = 2f; // 발사 간격
    private Transform player;
    public Transform railgun;
    public Transform lazer;
    public Transform tankbullet;
    public GameObject railgunMuzzleFlashParticle; // 레일건 파티클 효과
    public GameObject laserRifleMuzzleFlashParticle; // 레이저 라이플 파티클 효과
    GameObject damage;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        damage = GameObject.Find("N_PlayerModel");
        StartCoroutine(AutomaticShooting());

    }

    IEnumerator AutomaticShooting()
    {
        while (true)
        {
            if (player != null && Vector3.Distance(transform.position, player.position) <= shootingRange)
            {
                ShootRailgun(player.position);
                yield return new WaitForSeconds(shootInterval);
                ShootLaserRifle(player.position);
                yield return new WaitForSeconds(shootInterval);
            }
            yield return null;
        }
    }

    public void ShootRailgun(Vector3 targetPosition)
    {
        GameObject particle = Instantiate(railgunMuzzleFlashParticle, railgun.position, Quaternion.identity);
        Destroy(particle, 1f);

        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, shootingRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                damage.GetComponent<N_PlayerModel>().TakeDamage(3);
            }
        }
    }

    public void ShootLaserRifle(Vector3 targetPosition)
    {
        GameObject particle = Instantiate(laserRifleMuzzleFlashParticle, lazer.position, Quaternion.identity);
        Destroy(particle, 1f);

        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, shootingRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                damage.GetComponent<N_PlayerModel>().TakeDamage(3);
            }
        }
    }
}
