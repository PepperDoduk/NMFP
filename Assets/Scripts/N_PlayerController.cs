using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerController : MonoBehaviour
{
    public int speed;
    float hAxis;
    float vAxis;

    Vector3 moveVec;

    void Start()
    {
        
    }
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
    }
}
