using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_spawner_story : MonoBehaviour
{
    public GameObject[] MajorEnemies;
    public GameObject[] GeneralEnemies;
    public Vector3[] MajorEnemySpawnPositions;
    public Vector3[] GeneralEnemySpawnPositions;
    public int MajorEnemiesToSpawn = 1; // ��ȯ�� �ֿ� ��ǥ ���� ��
    public int GeneralEnemiesToSpawn = 5; // ��ȯ�� �Ϲ� ��ǥ ���� ��
    public float SpawnerRange;
    public Transform player;
    public float TriggerRadius;
    public bool hasSpawnedEnemies = false;

    private List<GameObject> ActiveMonsters = new List<GameObject>();

    public GameObject Exit;
    void Start()
    {
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < TriggerRadius && !hasSpawnedEnemies)
        {
            StartCoroutine(SpawnEnemies());
            hasSpawnedEnemies = true;
        }

        // �� ����Ʈ���� ��� ���� ���ŵǾ����� Ȯ��
        CheckAllEnemiesDefeated();
    }

    IEnumerator SpawnEnemies()
    {
        yield return StartCoroutine(SpawnMajorEnemies());
        yield return StartCoroutine(SpawnGeneralEnemies());
    }

    IEnumerator SpawnMajorEnemies()
    {
        if (MajorEnemySpawnPositions.Length == 0 || MajorEnemies.Length == 0)
        {
            Debug.LogWarning("MajorEnemySpawnPositions �Ǵ� MajorEnemies �迭�� ��� �ֽ��ϴ�.");
            yield break; // �迭�� ��� ���� ��� �ڷ�ƾ ����
        }

        for (int i = 0; i < MajorEnemiesToSpawn; i++)
        {
            Vector3 spawnPosition = MajorEnemySpawnPositions[i % MajorEnemySpawnPositions.Length];
            GameObject enemy = Instantiate(MajorEnemies[i % MajorEnemies.Length], spawnPosition, Quaternion.identity);
            ActiveMonsters.Add(enemy);
        }

        yield return null;
    }

    IEnumerator SpawnGeneralEnemies()
    {
        for (int i = 0; i < GeneralEnemiesToSpawn; i++)
        {
            Vector3 furthestPosition = GetFurthestSpawnPositionFromPlayer();
            GameObject enemy = Instantiate(GeneralEnemies[Random.Range(0, GeneralEnemies.Length)], furthestPosition, Quaternion.identity);
            ActiveMonsters.Add(enemy);

            yield return new WaitForSeconds(2f);
        }
    }

    Vector3 GetFurthestSpawnPositionFromPlayer()
    {
        Vector3 furthestPosition = GeneralEnemySpawnPositions[0];
        float maxDistance = Vector3.Distance(player.position, furthestPosition);

        foreach (Vector3 position in GeneralEnemySpawnPositions)
        {
            float distance = Vector3.Distance(player.position, position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestPosition = position;
            }
        }

        return furthestPosition;
    }

    
    void CheckAllEnemiesDefeated()
    {
        // Ȱ��ȭ�� �� ����Ʈ���� null�� �ƴ� ��Ҹ� �����
        ActiveMonsters.RemoveAll(monster => monster == null);

        if (ActiveMonsters.Count == 0 && hasSpawnedEnemies)
        {
            AllEnemiesDefeated();
        }
    }

    // ��� ���� �׾��� �� ȣ��Ǵ� �Լ�
    void AllEnemiesDefeated()
    {
        Debug.Log("��� ���� óġ�Ǿ����ϴ�!");
        StopAllCoroutines();
        Exit.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 position in MajorEnemySpawnPositions)
        {
            Gizmos.DrawCube(position, Vector3.one * 2f);
        }

        Gizmos.color = Color.blue;
        foreach (Vector3 position in GeneralEnemySpawnPositions)
        {
            Gizmos.DrawCube(position, Vector3.one * 2f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector3.one * TriggerRadius * 2);
    }
}
