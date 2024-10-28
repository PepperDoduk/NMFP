using System.Collections;
using UnityEngine;

public class K_robotGun : MonoBehaviour
{
    public float shootingRange = 10f; // �߻� �Ÿ�
    public float shootInterval = 2f; // �߻� ����
    private Transform player;
    public GameObject railgunMuzzleFlashParticle; // ���ϰ� ��ƼŬ ȿ��
    public GameObject laserRifleMuzzleFlashParticle; // ������ ������ ��ƼŬ ȿ��

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
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
        GameObject particle = Instantiate(railgunMuzzleFlashParticle, transform.position, Quaternion.identity);
        Destroy(particle, 1f);

        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, shootingRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                // �÷��̾�� ������
            }
        }
    }

    public void ShootLaserRifle(Vector3 targetPosition)
    {
        GameObject particle = Instantiate(laserRifleMuzzleFlashParticle, transform.position, Quaternion.identity);
        Destroy(particle, 1f);

        if (Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out RaycastHit hit, shootingRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                // �÷��̾�� ������
            }
        }
    }
}
