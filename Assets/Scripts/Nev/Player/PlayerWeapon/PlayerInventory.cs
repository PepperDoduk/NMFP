using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_defaultWeapon;
    [SerializeField] private GameObject m_defaultWeaponPrefab;
    
    [SerializeField] private WeaponFactory[] m_inventory;
    [SerializeField] private GameObject[] m_weaponPrefabs;
    [SerializeField] private GameObject[] m_dropWeapons;

    private PlayerWeapon m_playerWeapon;

    public bool canChangeWep = true;
    public float swapCooldown = 0.5f;

    private int curInvenNum = 0;
    private int maxInvenNum = 3;


    private void SetWeapon(int pNum)
    {
        m_playerWeapon.SetWeapon(
            m_inventory[pNum].CreateWeaponStrategy(),
            m_inventory[pNum].CreateWeaponData(),
            m_weaponPrefabs[pNum]);
    }

    public void ChangeWeapon(WeaponFactory pWeaponFactory, GameObject pWeaponPrefab, GameObject pDropWeapon)
    {
        bool flag = true;
        for(int i = 0; i < maxInvenNum; ++i)
        {
            if(m_inventory[i] == null)
            {
                m_inventory[i] = pWeaponFactory;
                m_weaponPrefabs[i] = pWeaponPrefab;
                m_dropWeapons[i] = pDropWeapon;
                curInvenNum = i;
                flag = false;
                break;
            }
        }
        if(flag)
        {
            m_dropWeapons[curInvenNum].SetActive(true);
            m_dropWeapons[curInvenNum].transform.position =
                this.transform.position;

            m_inventory[curInvenNum] = pWeaponFactory;
            m_weaponPrefabs[curInvenNum] = pWeaponPrefab;
            m_dropWeapons[curInvenNum] = pDropWeapon;
        }
        SetWeapon(curInvenNum);
    }

    private void Awake()
    {
        m_playerWeapon = GetComponent<PlayerWeapon>();
        m_inventory[0] = m_defaultWeapon;
        m_weaponPrefabs[0] = m_defaultWeaponPrefab;

        SetWeapon(0);

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
            SetWeapon(0);

            curInvenNum = 0;
            StartCoroutine(SwapCooldown(swapCooldown));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && curInvenNum != 1 && m_inventory[1] != null)
        {
            SetWeapon(1);

            curInvenNum = 1;
            StartCoroutine(SwapCooldown(swapCooldown));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && curInvenNum != 2 && m_inventory[2] != null)
        {
            SetWeapon(2);

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

            SetWeapon(curInvenNum);

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

            SetWeapon(curInvenNum);

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
