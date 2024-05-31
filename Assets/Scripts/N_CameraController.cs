using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        
    }
    void Update()
    {
        transform.position = target.position + offset;
    }
}
