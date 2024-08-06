using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_GameManager : MonoBehaviour
{
    private bool CursorLocked = true;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(CursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                CursorLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                CursorLocked = true;
            }
        }
    }
}
