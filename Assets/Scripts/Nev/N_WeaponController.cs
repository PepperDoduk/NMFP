using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_WeaponController : MonoBehaviour
{
    public GameObject[] Inventory;
    public bool[] hasItem;

    public GameObject weaponPrefab;
    public N_WeaponStates curWeapon;
    public N_WeaponStates swapWeapon;

    public bool canSwapWep = false;
    public bool canChangeWep = true;
    public float swapCooldown = 0.5f;

    public int InvenNum = 0;
    public int maxInvenNum = 3;

    private void Awake()
    {

    }

    private void Update()
    {
        //Swap Weapon
        if (Input.GetKeyDown(KeyCode.F) && canSwapWep && canChangeWep)
        {
            canChangeWep = false;

            weaponPrefab.SetActive(false);
            weaponPrefab = swapWeapon.WeaponPrefab;
            curWeapon = swapWeapon;
            weaponPrefab.SetActive(true);

            GetComponent<N_PlayerRayCast>().canShot = true;
            Debug.Log("Weapon Swapped");

            StartCoroutine(SwapCooldown(swapCooldown));
        }

        //Inventory
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0 && canChangeWep)
        {
            canChangeWep = false;
            InvenNum++;
            InvenNum %= maxInvenNum;

            StartCoroutine(SwapCooldown(swapCooldown));

            Debug.Log("ScrollUP");
        }
        else if(wheelInput < 0 && canChangeWep)
        {
            canChangeWep = false;
            InvenNum--;
            if (InvenNum < 0)
                InvenNum = maxInvenNum;

            StartCoroutine(SwapCooldown(swapCooldown));

            Debug.Log("ScrollDwon");
        }

        IEnumerator SwapCooldown(float cd)
        {
            yield return new WaitForSeconds(swapCooldown);
            canChangeWep = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Weapon")
        {
            Debug.Log("WeaponEnter");

            canSwapWep = true;

            swapWeapon = other.transform.GetComponent<N_WeaponStates>();
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