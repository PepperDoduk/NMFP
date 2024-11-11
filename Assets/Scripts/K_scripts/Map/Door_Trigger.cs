using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Trigger : MonoBehaviour
{
    public GameObject Enter;
    public GameObject Exit;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Invoke("Open",2);
        }
    }
    private void Open()
    {
        Enter.SetActive(true);
        Exit.SetActive(true);
    }

}
