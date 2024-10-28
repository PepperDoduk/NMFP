using System.Collections;
using UnityEngine;

public class K_bosscade : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // 소환할 적 프리팹 배열
    public float summonInterval = 10f; // 소환 간격
    private float summonCooldown;
    public float spawnRadius = 5f; // 보스 주변 소환 반경

    void Update()
    {
        summonCooldown -= Time.deltaTime;
        if (summonCooldown <= 0)
        {
            SummonEnemy();
            summonCooldown = summonInterval;
        }
    }

    void SummonEnemy()
    {
        if (enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Vector3 spawnPosition = transform.position + (Random.insideUnitSphere * spawnRadius);
        spawnPosition.y = transform.position.y; 

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
