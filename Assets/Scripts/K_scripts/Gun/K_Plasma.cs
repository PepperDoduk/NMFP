using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Plasma : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 10f;
    public float destroyDistance = 0.1f;

    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
    }

    void Update()
    {

        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
