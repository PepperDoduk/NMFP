using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_spawner_story : MonoBehaviour
{
    public GameObject[] MajorEnemies;
    public GameObject[] GeneralEnemies;
    public Vector3[] MajorEnemySpawnPositions;
    public Vector3[] GeneralEnemySpawnPositions;
    public int MajorEnemiesToSpawn = 1; // 소환할 주요 목표 적의 수
    public int GeneralEnemiesToSpawn = 5; // 소환할 일반 목표 적의 수
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

        // 적 리스트에서 모든 적이 제거되었는지 확인
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
            Debug.LogWarning("MajorEnemySpawnPositions 또는 MajorEnemies 배열이 비어 있습니다.");
            yield break; // 배열이 비어 있을 경우 코루틴 종료
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
        // 활성화된 적 리스트에서 null이 아닌 요소만 남기기
        ActiveMonsters.RemoveAll(monster => monster == null);

        if (ActiveMonsters.Count == 0 && hasSpawnedEnemies)
        {
            AllEnemiesDefeated();
        }
    }

    // 모든 적이 죽었을 때 호출되는 함수
    void AllEnemiesDefeated()
    {
        Debug.Log("모든 적이 처치되었습니다!");
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
