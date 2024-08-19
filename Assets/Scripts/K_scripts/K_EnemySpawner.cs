using System.Collections;
using UnityEngine;

public class K_EnemySpawner : MonoBehaviour
{
    public GameObject[] Monsters; 
    public float MonsterInterval = 10f;
    public Vector3 spawnerPosition = new Vector3(-7.5f, 2.1f, -8.9f);// let me know the approximate location where you plan to summon
    public float spawnRange = 2.0f;

    void Start()
    {
        StartCoroutine(spawnEnemy(MonsterInterval));
    }

    IEnumerator spawnEnemy(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Vector3 spawnPosition = spawnerPosition + Random.insideUnitSphere * spawnRange;
            spawnPosition.y = spawnerPosition.y; 

           
            int randomIndex = Random.Range(0, Monsters.Length);
            GameObject randomMonster = Monsters[randomIndex];

            Instantiate(randomMonster, spawnPosition, Quaternion.identity);
        }
    }
}
