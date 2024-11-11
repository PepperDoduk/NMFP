using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_bossTrigger : MonoBehaviour
{
    public GameObject boss;
    public GameObject Enter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Invoke("BOSS", 1f);
        }
    }
    private void BOSS()
    {
        Enter.SetActive(true);
        boss.SetActive(true);
    }
}
