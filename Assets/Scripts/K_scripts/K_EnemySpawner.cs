using System.Collections;
using UnityEngine;

public class K_EnemySpawner : MonoBehaviour
{
    public GameObject Monster;
    public float MonsterInterval = 10f;
    public Vector3 spawnerPosition = new Vector3(-7.5f, 2.1f, -8.9f);
    public float spawnRange = 2.0f;

    void Start()
    {
        StartCoroutine(spawnEnemy(MonsterInterval, Monster));
    }

    IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Vector3 spawnPosition = spawnerPosition + Random.insideUnitSphere * spawnRange;
            spawnPosition.y = spawnerPosition.y; // y °ª °íÁ¤
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }
}
