using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMangerk : MonoBehaviour
{
  
    public static playerMangerk instance;
     void Awake()
    {
        instance = this;
    }


    public GameObject player;
}
