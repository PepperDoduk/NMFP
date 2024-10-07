using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_WeaponController : MonoBehaviour
{

    public bool canSwapWep = false;
    public bool canChangeWep = true;
    public float swapCooldown = 0.5f;

    public GameObject Weapon;
    public N_WeaponStates swapWeapon;
    public N_WeaponStates[] Inventory;

    public bool[] hasItem;
    public int InvenNum = 0;
    public int maxInvenNum = 3;

    private void Awake()
    {
        //Inventory = new N_WeaponStates[maxInvenNum];
        //hasItem = new bool[maxInvenNum];
        for (int i = 1; i < maxInvenNum; ++i)
        {
            hasItem[i] = false;
        }
    }

    private void Update()
    {
        //Swap Weapon
        if (Input.GetKeyDown(KeyCode.F) && canSwapWep && canChangeWep)
        {
            bool trigger = true;
            canChangeWep = false;
            Weapon.SetActive(false);

            for (int i = 0; i < maxInvenNum; ++i)
            {
                if (hasItem[i] == false)
                {
                    trigger = false;

                    //현재 무기를 바닥에 버리는 함수

                    hasItem[i] = true;
                    Inventory[i] = swapWeapon;
                    InvenNum = i;

                    break;
                }
            }
            if (trigger)
            {
                //현재 무기를 바닥에 버리는 함수

                Inventory[InvenNum] = swapWeapon;
            }

            Weapon = Inventory[InvenNum].WeaponPrefab;
            Weapon.SetActive(true);


            GetComponent<N_PlayerRayCast>().canShot = true;
            Debug.Log("Weapon Swapped");

            StartCoroutine(SwapCooldown(swapCooldown));
        }

        //InventoryWeaponSwap
        if(Input.GetKeyDown(KeyCode.Alpha1) && InvenNum != 0)
        {
            SelectWep(0);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && InvenNum != 1)
        {
            SelectWep(1);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && InvenNum != 2)
        {
            SelectWep(2);
        }

        //InventoryWheelControll
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0 && canChangeWep)
        {
            canChangeWep = false;

            do
            {
                InvenNum--;
                if (InvenNum < 0)
                    InvenNum = maxInvenNum - 1;
            } while (!hasItem[InvenNum]) ;

            Weapon = Inventory[InvenNum].WeaponPrefab;
            Weapon.SetActive(true);

            StartCoroutine(SwapCooldown(swapCooldown));

            Debug.Log("ScrollUP");
        }
        else if (wheelInput < 0 && canChangeWep)
        {
            canChangeWep = false;

            do
            {
                InvenNum++;
                InvenNum %= maxInvenNum;
            } while (!hasItem[InvenNum]);

            Weapon = Inventory[InvenNum].WeaponPrefab;
            Weapon.SetActive(true);

            StartCoroutine(SwapCooldown(swapCooldown));
            Debug.Log("ScrollDwon");
        }
    }
    private void  SelectWep(int num)
    {
        if (canChangeWep && hasItem[num])
        {
            canChangeWep = false;
            InvenNum = num;

            Weapon = Inventory[InvenNum].WeaponPrefab;
            Weapon.SetActive(true);

            StartCoroutine(SwapCooldown(swapCooldown));
        }
        return;
    }
    IEnumerator SwapCooldown(float cd)
    {
        yield return new WaitForSeconds(swapCooldown);
        canChangeWep = true;
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
        if (other.transform.tag == "Weapon")
        {
            Debug.Log("WeaponExit");

            canSwapWep = false;
        }
    }
}