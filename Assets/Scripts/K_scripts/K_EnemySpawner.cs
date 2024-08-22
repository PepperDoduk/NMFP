using System.Collections;
using System.Collections.Generic; // List ����� ���� �ʿ�
using UnityEngine;

public class K_EnemySpawner : MonoBehaviour
{
    public GameObject[] Monsters;
    public float MonsterInterval = 10f;
    public Vector3[] spawnerPositions;
    public float spawnRange = 2.0f;
    public Transform player;
    public float triggerRadius = 5f; // ���̺� ���� ����
    private int currentWave = 1;
    private bool waveActive = false;

    private List<GameObject> activeMonsters = new List<GameObject>();

    void Update()
    {
        if (!waveActive && Vector3.Distance(player.position, transform.position) < triggerRadius)
        {
            waveActive = true;
            StartCoroutine(SpawnWaves());
        }

        if (waveActive && currentWave == 5 && activeMonsters.Count == 0)
        {
            waveActive = false;
            Debug.Log("��� ���̺갡 �Ϸ�Ǿ����ϴ�!");
        }
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave <= 5)
        {
            int spawnerCount = Mathf.Min(currentWave, spawnerPositions.Length); // ������ ���� ����
            for (int i = 0; i < spawnerCount; i++)
            {
                StartCoroutine(SpawnEnemies(MonsterInterval, spawnerPositions[i]));
            }

            yield return new WaitForSeconds(MonsterInterval * 10);
            currentWave++;
        }
    }

    IEnumerator SpawnEnemies(float interval, Vector3 spawnerPosition)
    {
        for (int i = 0; i < currentWave; i++)
        {
            yield return new WaitForSeconds(interval);

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
