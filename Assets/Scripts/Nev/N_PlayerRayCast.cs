using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class N_PlayerRayCast : MonoBehaviour
{
    N_WeaponController weapon;
    RaycastHit hit;

    public bool canShot = true;
    public bool repeater = true;

    float maxRayDistance = 15f;
    public float rayZ = 1.0f;
    public float rayY = 1.0f;

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
        //Debug.Log("Shot");
        canShot = false;
        //Debug.DrawRay(transform.position, transform.forward * maxRayDistance, Color.blue, 0.3f);

        float randY = Random.Range(-rayY * 100, rayY * 100);
        float randZ = Random.Range(-rayZ * 100, rayZ * 100);
        randY /= 100;
        randZ /= 100;

        Debug.DrawRay(transform.position,
            new Vector3(transform.forward.x * maxRayDistance, transform.forward.y * maxRayDistance + randY, transform.forward.z * maxRayDistance + randY),
            Color.blue, 0.3f);

        if (Physics.Raycast(transform.position,
            new Vector3(
                transform.forward.x * maxRayDistance,
                transform.forward.y * maxRayDistance + randY,
                transform.forward.z * maxRayDistance + randY)
            , out hit, maxRayDistance))
        {
            //Debug.Log($"{hit.collider.tag} Hit");
            if (hit.collider.tag == "Enemy")
            {
                Debug.Log("EnemyHit");
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
