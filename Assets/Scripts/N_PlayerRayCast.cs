using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerRayCast : MonoBehaviour
{
    RaycastHit hit;
    float maxRayDistance = 15f;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.red, 0.3f);
            if(Physics.Raycast(transform.position, transform.forward, out hit, maxRayDistance))
            {
                //Debug.Log($"{hit.collider.tag} Hit");
                if(hit.collider.tag == "Enemy")
                {
                    //Debug.Log("EnemyHit");
                    hit.collider.gameObject.GetComponent<N_EnemyController>().states.TakeDamage(10);
                }
            }
        }
    }
}
