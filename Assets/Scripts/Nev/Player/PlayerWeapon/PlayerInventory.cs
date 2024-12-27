using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_defaultWeapon;

    private PlayerWeapon m_playerWeapon;
    private WeaponFactory[] m_inventory;

    public bool canChangeWep = true;
    public float swapCooldown = 0.5f;

    private int curInvenNum = 0;
    private int maxInvenNum = 3;

    private void Awake()
    {
        m_playerWeapon = GetComponent<PlayerWeapon>();
        m_inventory[0] = m_defaultWeapon;

        for(int i = 1;  i < maxInvenNum; ++i)
        {
            m_inventory[i] = null;
        }
    }

    private void Update()
    {
        //InventoryWeaponSwap
        if (Input.GetKeyDown(KeyCode.Alpha1) && curInvenNum != 0)
        {
            m_playerWeapon.SetWeapon(
                m_inventory[0].CreateWeaponStrategy(),
                m_inventory[0].CreateWeaponData());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && curInvenNum != 1)
        {
            m_playerWeapon.SetWeapon(
                m_inventory[1].CreateWeaponStrategy(),
                m_inventory[1].CreateWeaponData());
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && curInvenNum != 2)
        {
            m_playerWeapon.SetWeapon(
                m_inventory[2].CreateWeaponStrategy(),
                m_inventory[2].CreateWeaponData());
        }

        //InventoryWheelControll
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0 && canChangeWep)
        {
            canChangeWep = false;

            do
            {
                curInvenNum--;
                if (curInvenNum < 0)
                    curInvenNum = maxInvenNum - 1;
            } while (m_inventory[curInvenNum] == null);

            m_playerWeapon.SetWeapon(
                m_inventory[curInvenNum].CreateWeaponStrategy(),
                m_inventory[curInvenNum].CreateWeaponData());

            StartCoroutine(SwapCooldown(swapCooldown));

            Debug.Log("ScrollUP");
        }
        else if (wheelInput < 0 && canChangeWep)
        {
            canChangeWep = false;

            do
            {
                curInvenNum++;
                curInvenNum %= maxInvenNum;
            } while (m_inventory[curInvenNum] == null);

            m_playerWeapon.SetWeapon(
                m_inventory[curInvenNum].CreateWeaponStrategy(),
                m_inventory[curInvenNum].CreateWeaponData());

            StartCoroutine(SwapCooldown(swapCooldown));
            Debug.Log("ScrollDwon");
        }
    }
    IEnumerator SwapCooldown(float cd)
    {
        yield return new WaitForSeconds(cd);
        canChangeWep = true;
    }
}
