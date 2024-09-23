using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_robotGun : MonoBehaviour
{
    public LineRenderer railgunLaser; // ���ϰ��� LineRenderer
    public LineRenderer laserRifleLaser; // ������ �������� LineRenderer
    public float railgunLaserWidth = 0.1f; // ���ϰ� �β�
    public float laserRifleLaserWidth = 0.05f; // ������ ������ �β�
    public float shootingRange = 10f; // �߻� �Ÿ�
    public float shootInterval = 2f; // �߻� ����
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // �� LineRenderer�� �β� ����
        railgunLaser.startWidth = railgunLaserWidth;
        railgunLaser.endWidth = railgunLaserWidth;

        laserRifleLaser.startWidth = laserRifleLaserWidth;
        laserRifleLaser.endWidth = laserRifleLaserWidth;

        // �ʱ� ���� ��Ȱ��ȭ
        railgunLaser.enabled = false;
        laserRifleLaser.enabled = false;

        // �ڵ� �߻� ����
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
        railgunLaser.SetPosition(0, transform.position);
        railgunLaser.SetPosition(1, targetPosition);
        railgunLaser.enabled = true;

        StartCoroutine(DisableLaser(railgunLaser));
    }

    public void ShootLaserRifle(Vector3 targetPosition)
    {
        laserRifleLaser.SetPosition(0, transform.position);
        laserRifleLaser.SetPosition(1, targetPosition);
        laserRifleLaser.enabled = true;

        StartCoroutine(DisableLaser(laserRifleLaser));
    }

    private IEnumerator DisableLaser(LineRenderer laser)
    {
        yield return new WaitForSeconds(0.2f); // �߻� ���� �ð�
        laser.enabled = false;
    }
}
