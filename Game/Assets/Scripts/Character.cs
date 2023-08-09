using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController cc;
    private PlayerInput playerinput;
    private Animator anim;

    private Vector3 MovementVelocity;
    private float VerticalVelocity;

    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;

    //Enemy
    private UnityEngine.AI.NavMeshAgent Agent;
    private Transform TargetPlayer;

    public bool IsPlayer = true;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        
        if(!IsPlayer)
        {
            Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            TargetPlayer = GameObject.FindWithTag("Player").transform;
            Agent.speed = MoveSpeed;
        }
        else 
        {
            playerinput = GetComponent<PlayerInput>();
        }
    }

    private void CalculatePlayerMovement()
    {
        MovementVelocity.Set(playerinput.HorizonItalInput, 0f, playerinput.VerticalInput);
        MovementVelocity.Normalize();
        MovementVelocity = Quaternion.Euler(0f, -45f, 0f) * MovementVelocity;
        anim.SetFloat("Speed", MovementVelocity.magnitude);
        MovementVelocity *= MoveSpeed * Time.deltaTime;

        if(MovementVelocity != Vector3.zero )
            transform.rotation = Quaternion.LookRotation(MovementVelocity);

        anim.SetBool("IsFall", !cc.isGrounded);
    }
    private void CalculateEnemyMovement()
    {
        if(Vector3.Distance(TargetPlayer.position, transform.position) >= Agent.stoppingDistance)
        {
            Agent.SetDestination(TargetPlayer.position);
            anim.SetFloat("Speed", 0.2f);
        }
        else 
        {
            Agent.SetDestination(TargetPlayer.position);
            anim.SetFloat("Speed", 0f);
        }
    }

    private void FixedUpdate()
    {
        if(IsPlayer)
        {
            CalculatePlayerMovement();
            //중력 적용
            if (cc.isGrounded == false)
                VerticalVelocity = Gravity;
            else
                VerticalVelocity = Gravity * 0.3f;

            MovementVelocity += VerticalVelocity * Vector3.up * Time.deltaTime;

            cc.Move(MovementVelocity);
        }
        else
            CalculateEnemyMovement();
       
    }
}

