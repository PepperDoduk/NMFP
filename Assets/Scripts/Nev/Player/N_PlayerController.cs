using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_PlayerController : MonoBehaviour
{
    private N_IPlayerModel m_playerModel = new N_PlayerModel();

    public Transform caemraTransform;
    public CharacterController characterController;

    private float m_moveSpeed => m_playerModel.MoveSpeed;
    private float m_jumpSpeed => m_playerModel.JumpSpeed;

    private float MoveSpeed;
    private float JumpSpeed;
    private float gravity;
    private float yVelocity;

    public bool isGround = true;

    private void Awake()
    {
        m_playerModel = GetComponent<N_PlayerModel>();

        MoveSpeed = m_moveSpeed;
        JumpSpeed = m_jumpSpeed;
        gravity = -10f;
        yVelocity = 0;
    }

    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float VAxis = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(hAxis, 0, VAxis);

        moveDir = caemraTransform.TransformDirection(moveDir) * MoveSpeed;
        
        if(characterController.isGrounded)
        {
            yVelocity = 0;
            isGround = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            yVelocity = JumpSpeed;
            isGround = false;
        }

        yVelocity += (gravity * Time.deltaTime);
        moveDir.y = yVelocity;

        characterController.Move(moveDir * Time.deltaTime);
    }
}
