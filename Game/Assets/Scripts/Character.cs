using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController cc;
    private PlayerInput playerinput;
    private Animator anim;
    private MaterialPropertyBlock m_MaterialPropertyBlock;
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;


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

        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_MaterialPropertyBlock = new MaterialPropertyBlock();
        m_SkinnedMeshRenderer.GetPropertyBlock(m_MaterialPropertyBlock);

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
            return;
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
            //�߷� ����
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
                if(m_DamageCaster != null)
                    AttackCollisionDisable();
                break;
        }
        //Enter
        switch (new_state)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:

                if (!IsPlayer)
                {
                    Quaternion rot = Quaternion.LookRotation(TargetPlayer.position - transform.position);
                    transform.rotation = rot;
                }
                else
                    AttackStartTime = Time.time;

                anim.SetTrigger("Attack");
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
        
        if(!IsPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayHitEffect(AttackerPos);
        }

        StartCoroutine(MaterialBlink());
    }

    public void AttackCollisionEnable()
    {
        m_DamageCaster.AttackCollsionEnable();
    }

    public void AttackCollisionDisable()
    {
        m_DamageCaster.AttackCollsionDisable();
    }

    IEnumerator MaterialBlink()
    {
        m_MaterialPropertyBlock.SetFloat("_blink", 0.4f);
        m_SkinnedMeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
        yield return new WaitForSeconds(0.2f);
        m_MaterialPropertyBlock.SetFloat("_blink", 0f);
        m_SkinnedMeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
    }
}

