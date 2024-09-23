using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_robotGun : MonoBehaviour
{
    public LineRenderer railgunLaser; // 레일건의 LineRenderer
    public LineRenderer laserRifleLaser; // 레이저 라이플의 LineRenderer
    public float railgunLaserWidth = 0.1f; // 레일건 두께
    public float laserRifleLaserWidth = 0.05f; // 레이저 라이플 두께
    public float shootingRange = 10f; // 발사 거리
    public float shootInterval = 2f; // 발사 간격
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 각 LineRenderer의 두께 설정
        railgunLaser.startWidth = railgunLaserWidth;
        railgunLaser.endWidth = railgunLaserWidth;

        laserRifleLaser.startWidth = laserRifleLaserWidth;
        laserRifleLaser.endWidth = laserRifleLaserWidth;

        // 초기 상태 비활성화
        railgunLaser.enabled = false;
        laserRifleLaser.enabled = false;

        // 자동 발사 시작
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
        yield return new WaitForSeconds(0.2f); // 발사 지속 시간
        laser.enabled = false;
    }
}
