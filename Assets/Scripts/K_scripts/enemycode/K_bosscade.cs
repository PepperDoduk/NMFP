using System.Collections;
using UnityEngine;

public class K_bosscade : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // ��ȯ�� �� ������ �迭
    public float summonInterval = 10f; // ��ȯ ����
    private float summonCooldown;
    public float spawnRadius = 5f; // ���� �ֺ� ��ȯ �ݰ�

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
