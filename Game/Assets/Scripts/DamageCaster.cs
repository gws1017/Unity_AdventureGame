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
}
