using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class N_PlayerRayCast : MonoBehaviour
{
    RaycastHit hit;
    float maxRayDistance = 15f;
    public bool reroad = false;
    public bool canShot = true;
    public bool shotting = false;
    public bool repeater = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (repeater)
            {
                repeater = false;
            }
            else
            {
                repeater = true;
            }
        }

        if (repeater)
        {
            if(Input.GetKey(KeyCode.Mouse0) && canShot && !reroad)
            {
                Shot();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canShot && !reroad)
            {
                Shot();
            }
        }
    }
    private void Shot()
    {
        Debug.Log("Shot");
        canShot = false;
        shotting = true;
        Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.3f);

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
    IEnumerator Shotcooldown()
    {
        float RPM = GetComponent<N_WeaponController>().curWeapon.Data.RPM;
        yield return new WaitForSeconds(1 / (RPM / 60));
        canShot = true;
        shotting = false;
    }
}
