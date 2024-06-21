using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerRayCast : MonoBehaviour
{
    RaycastHit hit;
    float maxRayDistance = 15f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.3f);
            if(Physics.Raycast(transform.position, transform.forward, out hit, maxRayDistance))
            {
                if(hit.collider.tag == "Enemy")
                {
                    Debug.Log("EnemyHit");
                    hit.collider.gameObject.GetComponent<N_EnemyStates>().health -= 10;
                }
            }
        }
    }
}
