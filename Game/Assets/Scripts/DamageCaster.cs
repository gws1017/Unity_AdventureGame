using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Collider DamageCasterCollider;
    private List<Collider> DamageTargetList;
    public int Damage = 30;
    public string TargetTag;


    private void Awake()
    {
        DamageCasterCollider = GetComponent<Collider>();
        DamageCasterCollider.enabled = false;
        DamageTargetList= new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == TargetTag && !DamageTargetList.Contains(other)) 
        { 
            Character Target = other.GetComponent<Character>();
            if(Target != null )
            {
                Target.ApplyDamage(Damage);

                PlayerVFXManager player_vfx_manager = transform.parent.GetComponent<PlayerVFXManager>();
                if(player_vfx_manager != null)
                {
                    RaycastHit hit;
                    Vector3 originalPos = transform.position + (-DamageCasterCollider.bounds.extents.z) * transform.forward;

                    bool isHit = Physics.BoxCast(originalPos, DamageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation,
                         DamageCasterCollider.bounds.extents.z, 1 << 6);

                    if(isHit)
                    {
                        player_vfx_manager.PlaySlash(hit.point + new Vector3(0f,0.5f,0f));
                    }
                }
            }
            DamageTargetList.Add(other);
        }
    }

    public void AttackCollsionEnable()
    {
        DamageTargetList.Clear();
        DamageCasterCollider.enabled = true;
    }
    public void AttackCollsionDisable()
    {
        DamageTargetList.Clear();
        DamageCasterCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        if(DamageCasterCollider == null)
            DamageCasterCollider = GetComponent<Collider>();

        RaycastHit hit;
        Vector3 originalPos = transform.position + (-DamageCasterCollider.bounds.extents.z ) * transform.forward;

        bool isHit = Physics.BoxCast(originalPos,DamageCasterCollider.bounds.extents/2, transform.forward, out hit, transform.rotation,
             DamageCasterCollider.bounds.extents.z,1<<6);

        if(isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    
    }
}
