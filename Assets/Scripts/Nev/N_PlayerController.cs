using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerController : MonoBehaviour
{
    public Transform caemraTransform;
    public CharacterController characterController;

    public float moveSpeed = 10f;
    public float jumpSpeed = 10f;
    public float gravity = - 10f;
    public float yVelocity = 0;

    public bool isGround = true;


    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float VAxis = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(hAxis, 0, VAxis);

        moveDir = caemraTransform.TransformDirection(moveDir);
        moveDir *= moveSpeed;

        
        if(characterController.isGrounded)
        {
            yVelocity = 0;
            isGround = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            yVelocity = jumpSpeed;
            isGround = false;
        }

        yVelocity += (gravity * Time.deltaTime);
        moveDir.y = yVelocity;

        characterController.Move(moveDir * Time.deltaTime);
    }
}
