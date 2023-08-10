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
    private Vector3 ImpactVector;
    private float VerticalVelocity;
    private float AttackStartTime;
    private float AttackDuration;

    public GameObject ItemToDrop;

    public float MoveSpeed = 5f;
    public float Gravity = -9.8f;
    public float AttackSlideDuration = 0.4f;
    public float AttackSlideSpeed = 0.06f;
    public float SlideSpeed = 9f;

    public int Coin = 0;

    //Enemy
    private UnityEngine.AI.NavMeshAgent Agent;
    private Transform TargetPlayer;

    //State
    public enum CharacterState
    {
        Normal,Attack,Dead,Hit,Slide
    }
    public CharacterState CurrentState;

    public bool IsPlayer = true;
    public bool IsInvincible;

    public float InvincibleDuration = 2f;

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
        else if(playerinput.SpaceDown  && cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Slide);
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
                    if(Time.time < AttackStartTime + AttackSlideDuration) 
                    {
                        float TimePassed = Time.time - AttackStartTime;
                        float LerpTime = TimePassed / AttackSlideDuration;
                        MovementVelocity = Vector3.Lerp(transform.forward * AttackSlideSpeed, Vector3.zero, LerpTime);
                    }

                    if(playerinput.MouseButtonDown && cc.isGrounded)
                    {
                        string CurrentClipName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        AttackDuration = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

                        if (CurrentClipName != "LittleAdventurerAndie_ATTACK_03" && AttackDuration > 0.5f  && AttackDuration <0.7f)
                        {
                            playerinput.MouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attack);
                            CalculatePlayerMovement() ;
                        }
                    }
                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.Hit:
                if(ImpactVector.magnitude > 0.2f)
                {
                    MovementVelocity = ImpactVector * Time.deltaTime;
                }
                ImpactVector = Vector3.Lerp(ImpactVector, Vector3.zero, Time.deltaTime * 5);
                break;
            case CharacterState.Slide:
                MovementVelocity = transform.forward * SlideSpeed * Time.deltaTime;
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
            MovementVelocity = Vector3.zero;
        }

    }

    public void SwitchStateTo(CharacterState new_state)
    {
        if(IsPlayer)playerinput.ClearCache();
        //Exit
        switch(CurrentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                if(m_DamageCaster != null)
                    AttackCollisionDisable();
                if (IsPlayer)
                    GetComponent<PlayerVFXManager>().StopSword();
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.Hit:
                break;
            case CharacterState.Slide:
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
            case CharacterState.Dead:
                cc.enabled = false;
                anim.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.Hit:
                anim.SetTrigger("IsHit");
                if(IsPlayer)
                {
                    IsInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;
            case CharacterState.Slide:
                anim.SetTrigger("Slide");
                break;
        }

        CurrentState = new_state;

        
        //Debug.Log("Swtich State " + CurrentState);
    }

    public void SlideEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void HitEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void AttackEnd()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage (int damage, Vector3 AttackerPos = new Vector3())
    {
        if(IsInvincible)
        {
            return;
        }

        if(m_Health != null)
        {
            m_Health.ApplyDamage(damage);
        }
        
        if(!IsPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayHitEffect(AttackerPos);
        }

        StartCoroutine(MaterialBlink());

        if(IsPlayer)
        {
            SwitchStateTo(CharacterState.Hit);
            AddImpact(AttackerPos, 10f);
        }
    }

    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(InvincibleDuration);
        IsInvincible = false;
    }

    void AddImpact(Vector3 CauserPos, float force)
    {
        Vector3 ImpactDir = transform.position - CauserPos;
        ImpactDir.Normalize();
        ImpactDir.y = 0;
        ImpactVector = ImpactDir * force;
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

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2f);

        float DissolveTimeDuration = 2f;
        float CurrentDissoveTime = 0f;
        float DissovleHeightStart = 20f;
        float DissovleHeightTarget = -10f;
        float DissovleHeight;

        m_MaterialPropertyBlock.SetFloat("_enableDissolve", 1f);
        m_SkinnedMeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);

        while(CurrentDissoveTime < DissolveTimeDuration)
        {
            CurrentDissoveTime += Time.deltaTime;
            DissovleHeight = Mathf.Lerp(DissovleHeightStart, DissovleHeightTarget, CurrentDissoveTime / DissolveTimeDuration);
            m_MaterialPropertyBlock.SetFloat("_dissolve_height", DissovleHeight);
            m_SkinnedMeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
            yield return null;
        }

        DropItem();
    }

    public void DropItem()
    {
        if(ItemToDrop != null)
        {
            Instantiate(ItemToDrop,transform.position,Quaternion.identity);
        }
    }

    public void PickUpItem(PickUp Item)
    {
        switch(Item.Type)
        {
            case PickUp.PickUpType.Heal:
                AddHealth(Item.Value);
                GetComponent<PlayerVFXManager>().PlayHealVFX();
                break;
            case PickUp.PickUpType.Coin:
                AddCoin(Item.Value);
                break;
        }
    }

    private void AddHealth(int value)
    {
        m_Health.AddHealth(value);
    }

    private void AddCoin(int value)
    {
        Coin += value;
    }
}

