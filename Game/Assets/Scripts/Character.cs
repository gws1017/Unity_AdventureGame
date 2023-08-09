using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController cc;
    private PlayerInput playerinput;

    private Vector3 MovementVelocity;
    private float VerticalVelocity;

    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        playerinput = GetComponent<PlayerInput>();

    }

    private void CaculatePlayerMovement()
    {
        MovementVelocity.Set(playerinput.HorizonItalInput, 0f, playerinput.VerticalInput);
        MovementVelocity.Normalize(); //���ϸ� �밢�� �̵��ӵ��� ������
        MovementVelocity = Quaternion.Euler(0f, -45f, 0f) * MovementVelocity;
        MovementVelocity *= MoveSpeed * Time.deltaTime;

        if(MovementVelocity != Vector3.zero )
            transform.rotation = Quaternion.LookRotation(MovementVelocity);
    }
    private void FixedUpdate()
    {
        CaculatePlayerMovement();

        //�߷� ����
        if(cc.isGrounded == false)
            VerticalVelocity = Gravity;
        else
            VerticalVelocity = Gravity*0.3f;

        MovementVelocity += VerticalVelocity * Vector3.up * Time.deltaTime;

        cc.Move(MovementVelocity);
    }
}

