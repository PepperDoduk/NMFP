using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class N_WeaponStates : MonoBehaviour
{
    public GameObject WeaponPrefab;
    public N_WeaponData Data;

    private void Awake()
    {
        Data.CurAmmo = Data.MaxAmmo;
        //WeaponPrefab.SetActive(false);
    }
}
