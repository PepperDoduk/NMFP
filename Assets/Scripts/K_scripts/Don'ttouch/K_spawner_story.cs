using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_spawner_story : MonoBehaviour
{
    public GameObject[] MajorEnemies;
    public GameObject[] GeneralEnemies;
    public Vector3[] MajorEnemySpawnPositions;
    public Vector3[] GeneralEnemySpawnPositions;
    public int MajorEnemiesToSpawn = 1;
    public int GeneralEnemiesToSpawn = 5;
    public bool hasSpawnedEnemies = false;

    private List<GameObject> ActiveMonsters = new List<GameObject>();
    public GameObject Exit;

    public float Width = 10f;
    public float Height = 5f;
    public float backc = 10f;

    void Start()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(Width, Height, backc);
    }

    void Update()
    {
        if (hasSpawnedEnemies)
        {
            CheckAllEnemiesDefeated();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawnedEnemies)
        {
            StartCoroutine(SpawnEnemies());
            hasSpawnedEnemies = true;
        }
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
            yield break;
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

            yield return new WaitForSeconds(0.5f);
        }
    }

    Vector3 GetFurthestSpawnPositionFromPlayer()
    {
        Vector3 furthestPosition = GeneralEnemySpawnPositions[0];
        float maxDistance = Vector3.Distance(transform.position, furthestPosition);

        foreach (Vector3 position in GeneralEnemySpawnPositions)
        {
            float distance = Vector3.Distance(transform.position, position);
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
        // null 값을 리스트에서 제거
        ActiveMonsters.RemoveAll(monster => monster == null);

        Debug.Log($"남은 적 수: {ActiveMonsters.Count}");

        if (ActiveMonsters.Count == 0 && hasSpawnedEnemies)
        {
            AllEnemiesDefeated();
        }
    }

    void AllEnemiesDefeated()
    {
        Debug.Log("모든 적이 처치되었습니다!");
        StopAllCoroutines();
        Exit.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, backc));
    }
}
