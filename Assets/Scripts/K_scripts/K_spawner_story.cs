using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class K_spawner_story : MonoBehaviour
{
    public GameObject[] MajorEnemy;
    public Vector3[] Spawnerposition;
    public float SpawnerRange;
    public Transform player;
    public float TriggerRadius;
    public bool WaveActive = false;

    private List<GameObject> ActiveMonsters = new List<GameObject>();
    void Start()
    {


    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
       
    }
}
