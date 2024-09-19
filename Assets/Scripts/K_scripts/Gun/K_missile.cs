using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_missile : MonoBehaviour
{

    public GameObject missilePrefab;
    public Transform firePoint;
    public float missileSpeed = 20f;
    public float fireRate = 2f; 
    private float fireCooldown = 0f;
    public Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0f)
        {
            LaunchMissile();
            fireCooldown = fireRate; 
        }
    }

    void LaunchMissile()
    {
        if (target != null)
        {
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = missile.GetComponent<Rigidbody>();

            
            Vector3 direction = (target.position - firePoint.position).normalized;
            rb.velocity = direction * missileSpeed;
        }
    }
}


