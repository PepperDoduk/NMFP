using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_EnemySpawner : MonoBehaviour
{
    public GameObject[] Monsters;
    public Vector3[] spawnerPositions;
    public float spawnRange = 2.0f;
    public Transform player;
    public float triggerRadius = 5f;
    public int[][] monstersPerWavePerType;
    public float waveInterval = 10f;
    private int currentWave = 0;
    private bool waveActive = false;

    private List<GameObject> activeMonsters = new List<GameObject>();

    void Start()
    {
        monstersPerWavePerType = new int[][]
        {
            new int[] { 1, 2, 1 },//1wave 
            new int[] { 2, 3, 2 },//2wave
            new int[] { 3, 4, 3 },//3wave
        };
    }

    void Update()
    {
        if (!waveActive && Vector3.Distance(player.position, transform.position) < triggerRadius)
        {
            waveActive = true;
            StartCoroutine(SpawnWaves());
        }

        if (waveActive && currentWave >= monstersPerWavePerType.Length && activeMonsters.Count == 0)
        {
            waveActive = false;
            Debug.Log("모든 웨이브가 완료되었습니다!");
        }
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < monstersPerWavePerType.Length)
        {
            int[] monstersToSpawn = monstersPerWavePerType[currentWave];
            for (int i = 0; i < Monsters.Length; i++)
            {
                int numberOfEnemies = monstersToSpawn[i];
                SpawnEnemies(numberOfEnemies, spawnerPositions[i % spawnerPositions.Length]);
            }

            yield return new WaitForSeconds(waveInterval);
            currentWave++;
        }
    }

    void SpawnEnemies(int numberOfEnemies, Vector3 spawnerPosition)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = spawnerPosition + Random.insideUnitSphere * spawnRange;
            spawnPosition.y = spawnerPosition.y;

            int randomIndex = Random.Range(0, Monsters.Length);
            GameObject randomMonster = Instantiate(Monsters[randomIndex], spawnPosition, Quaternion.identity);

            K_Enenmy enemy = randomMonster.GetComponent<K_Enenmy>();
            enemy.OnDeath += () => { activeMonsters.Remove(randomMonster); };

            activeMonsters.Add(randomMonster);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 position in spawnerPositions)
        {
            Gizmos.DrawWireSphere(position, spawnRange);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
