using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class N_PlayerRayCast : MonoBehaviour
{
    N_WeaponController weapon;
    RaycastHit hit;

    float maxRayDistance = 15f;
    public bool canShot = true;
    public bool repeater = true;

    public float rayZ = 0.02f, rayY = 0.02f;

    private void Awake()
    {
        weapon = GetComponent<N_WeaponController>();
    }

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
    }
    public void Shot()
    {
        Debug.Log("Shot");
        canShot = false;
        //Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.3f);

        float randY = Random.Range(-rayY, rayY);
        float randZ = Random.Range(-rayZ, rayZ);

        Debug.DrawRay(transform.position,
            new Vector3(transform.forward.x * maxRayDistance, transform.forward.y * maxRayDistance + randY, transform.forward.z * maxRayDistance + randY),
            Color.blue, 0.3f);
        Debug.DrawRay(transform.position,
            new Vector3(transform.forward.x * maxRayDistance, transform.forward.y * maxRayDistance - randZ, transform.forward.z * maxRayDistance - randZ),
            Color.blue, 0.3f);

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
        float RPM = weapon.Inventory[weapon.InvenNum].Data.RPM;
        yield return new WaitForSeconds(1 / (RPM / 60));
        canShot = true;
    }
}
