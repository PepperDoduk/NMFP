using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Tankbullet : MonoBehaviour
{
    private Transform target;
    public float speed = 20f;  

    // Å¸°Ù ¼³Á¤
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    void Update()
    {
      
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

           
            if (Vector3.Distance(transform.position, target.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}
