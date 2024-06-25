using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_WeaponController : MonoBehaviour
{
    //public N_WeaponStates curWeapon;
    public GameObject weapon;
    GameObject swapWeapon;
    public string wepName;
    public bool canSwapWep = false;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canSwapWep)
        {
            weapon.SetActive(false);
            weapon = swapWeapon;
            weapon.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            Debug.Log("WeaponEnter");

            canSwapWep = true;

            swapWeapon = other.transform.GetComponent<N_WeaponStates>().WeaponPrefab;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Weapon")
        {
            Debug.Log("WeaponExit");

            canSwapWep = false;
        }
    }
}