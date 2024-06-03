using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_CameraController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 300;
    [SerializeField]
    private float maxRotationX = 80;
    [SerializeField]
    private float minRotationX = -80;

    private float rotationX;
    private float rotationY;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX += mouseY * sensitivity * Time.deltaTime;
        rotationY += mouseX * sensitivity * Time.deltaTime;

        if(rotationX > maxRotationX)
        {
            rotationX = maxRotationX;
        }
        if(rotationX < minRotationX)
        {
            rotationX = minRotationX;
        }

        transform.eulerAngles = new Vector3(-rotationX, rotationY, 0);
    }
}
