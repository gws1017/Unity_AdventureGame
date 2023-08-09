using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController cc;
    private PlayerInput playerinput;
    private Animator anim;

    private Health m_Health;
    private DamageCaster m_DamageCaster;

    private Vector3 MovementVelocity;
    private float VerticalVelocity;
    private float AttackStartTime;

    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;
    public float AttackSlideDuration = 0.4f;
    public float AttackSlideSpeed = 0.06f;
    //Enemy
    private UnityEngine.AI.NavMeshAgent Agent;
    private Transform TargetPlayer;

    //State
    public enum CharacterState
    {
        Normal,Attack
    }
    public CharacterState CurrentState;

    public bool IsPlayer = true;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        m_Health = GetComponent<Health>();
        m_DamageCaster = GetComponentInChildren<DamageCaster>();

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
        if(playerinput.MouseButtonDown && cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Attack);
            //return;
        }

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
            SwitchStateTo(CharacterState.Attack);
        }
    }

    private void FixedUpdate()
    {
        switch(CurrentState)
        {
            case CharacterState.Normal:
                if(IsPlayer)
                    CalculatePlayerMovement();
                else
                    CalculateEnemyMovement();
                break;
            case CharacterState.Attack:
                if(IsPlayer)
                {
                    MovementVelocity = Vector3.zero;

                    if(Time.time < AttackStartTime + AttackSlideDuration) 
                    {
                        float TimePassed = Time.time - AttackStartTime;
                        float LerpTime = TimePassed / AttackSlideDuration;
                        MovementVelocity = Vector3.Lerp(transform.forward * AttackSlideSpeed, Vector3.zero, LerpTime);
                    }
                }
                break;
        }

        if(IsPlayer)
        {
            //중력 적용
            if (cc.isGrounded == false)
                VerticalVelocity = Gravity;
            else
                VerticalVelocity = Gravity * 0.3f;

            MovementVelocity += VerticalVelocity * Vector3.up * Time.deltaTime;

            cc.Move(MovementVelocity);
        }
       
    }

    private void SwitchStateTo(CharacterState new_state)
    {
        if(IsPlayer)playerinput.MouseButtonDown = false;
        //Exit
        switch(CurrentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                if(!IsPlayer)
                {
                    Quaternion rot = Quaternion.LookRotation(TargetPlayer.position - transform.position);
                    transform.rotation = rot;
                }
                break;
        }
        //Enter
        switch (new_state)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                anim.SetTrigger("Attack");
                if(IsPlayer)
                {
                    AttackStartTime = Time.time;
                }
                break;
        }

        CurrentState = new_state;

        
        //Debug.Log("Swtich State " + CurrentState);
    }

    public void AttackEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage (int damage, Vector3 AttackerPos = new Vector3())
    {
        if(m_Health != null)
        {
            m_Health.ApplyDamage(damage);
        }
    }

    public void AttackCollisionEnable()
    {
        m_DamageCaster.AttackCollsionEnable();
    }

    public void AttackCollisionDisable()
    {
        m_DamageCaster.AttackCollsionDisable();
    }
}

