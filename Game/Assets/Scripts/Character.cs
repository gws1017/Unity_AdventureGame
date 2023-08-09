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
        MovementVelocity.Normalize(); //안하면 대각선 이동속도가 빨라짐
        MovementVelocity = Quaternion.Euler(0f, -45f, 0f) * MovementVelocity;
        MovementVelocity *= MoveSpeed * Time.deltaTime;

        if(MovementVelocity != Vector3.zero )
            transform.rotation = Quaternion.LookRotation(MovementVelocity);
    }
    private void FixedUpdate()
    {
        CaculatePlayerMovement();

        //중력 적용
        if(cc.isGrounded == false)
            VerticalVelocity = Gravity;
        else
            VerticalVelocity = Gravity*0.3f;

        MovementVelocity += VerticalVelocity * Vector3.up * Time.deltaTime;

        cc.Move(MovementVelocity);
    }
}

