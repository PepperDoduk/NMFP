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
    public Transform player; // 플레이어의 Transform을 참조

    void Start()
    {

    }

    void Update()
    {
        ShootAtPlayer();
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;
        if (bulletTime > 0) return;
        bulletTime = time;
        GameObject bulletObj = Instantiate(enemybullet, spawnPoint.position, spawnPoint.rotation);
        K_Bullet bulletScript = bulletObj.GetComponent<K_Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(player);
        }
        Destroy(bulletObj, 5f);
    }
}
