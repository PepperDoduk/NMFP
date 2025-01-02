using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_defaultWeapon;
    [SerializeField] private GameObject m_defaultWeaponPrefab;
    
    [SerializeField] private WeaponFactory[] m_inventory;
    [SerializeField] private GameObject[] m_weaponPrefabs;
    private PlayerWeapon m_playerWeapon;

    public bool canChangeWep = true;
    public float swapCooldown = 0.5f;

    private int curInvenNum = 0;
    private int maxInvenNum = 3;

    private void Awake()
    {
        m_playerWeapon = GetComponent<PlayerWeapon>();
        m_inventory[0] = m_defaultWeapon;
        m_inventory[1] = m_defaultWeapon;
        m_weaponPrefabs[0] = m_defaultWeaponPrefab;

        m_playerWeapon.SetWeapon(
            m_defaultWeapon.CreateWeaponStrategy(),
            m_defaultWeapon.CreateWeaponData(),
            m_defaultWeaponPrefab);

        for (int i = 2; i < maxInvenNum; ++i)
        {
            m_inventory[i] = null;
        }
    }

    private void Update()
    {
        //InventoryWeaponSwap
        if (Input.GetKeyDown(KeyCode.Alpha1) && curInvenNum != 0 && m_inventory[0] != null)
        {
            m_playerWeapon.UninstallWeapon();
            m_playerWeapon.SetWeapon(
                m_inventory[0].CreateWeaponStrategy(),
                m_inventory[0].CreateWeaponData(),
                m_weaponPrefabs[0]);

            curInvenNum = 0;
            StartCoroutine(SwapCooldown(swapCooldown));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && curInvenNum != 1 && m_inventory[1] != null)
        {
            m_playerWeapon.UninstallWeapon();
            m_playerWeapon.SetWeapon(
                m_inventory[1].CreateWeaponStrategy(),
                m_inventory[1].CreateWeaponData(),
                m_weaponPrefabs[1]);

            curInvenNum = 1;
            StartCoroutine(SwapCooldown(swapCooldown));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && curInvenNum != 2 && m_inventory[2] != null)
        {
            m_playerWeapon.UninstallWeapon();
            m_playerWeapon.SetWeapon(
                m_inventory[2].CreateWeaponStrategy(),
                m_inventory[2].CreateWeaponData(),
                m_weaponPrefabs[2]);

            curInvenNum = 2;
            StartCoroutine(SwapCooldown(swapCooldown));
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

            m_playerWeapon.UninstallWeapon();
            m_playerWeapon.SetWeapon(
                m_inventory[curInvenNum].CreateWeaponStrategy(),
                m_inventory[curInvenNum].CreateWeaponData(),
                m_weaponPrefabs[curInvenNum]);

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

            m_playerWeapon.UninstallWeapon();
            m_playerWeapon.SetWeapon(
                m_inventory[curInvenNum].CreateWeaponStrategy(),
                m_inventory[curInvenNum].CreateWeaponData(),
                m_weaponPrefabs[curInvenNum]);

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
