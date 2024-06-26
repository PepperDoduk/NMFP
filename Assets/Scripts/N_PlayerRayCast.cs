using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerRayCast : MonoBehaviour
{
    RaycastHit hit;
    float maxRayDistance = 15f;
    public bool reroad = false;
    public bool canShot = true;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShot && !reroad)
        {
            Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.3f);
            canShot = false;

            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRayDistance))
            {
                //Debug.Log($"{hit.collider.tag} Hit");
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("EnemyHit");
                    hit.collider.gameObject.GetComponent<N_EnemyController>().states.TakeDamage(10);
                }
            }
            StartCoroutine(Shotcooldown());
        }
    }
    IEnumerator Shotcooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canShot = true;
    }
}
