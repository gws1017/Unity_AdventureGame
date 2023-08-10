using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody m_RigidBody;

    public ParticleSystem HitParticle;
    public float Speed = 2f;
    public int Damage = 10;


    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        m_RigidBody.MovePosition(transform.position+ transform.forward*Speed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Character cc =  other.gameObject.GetComponent<Character>();

        if (cc != null && cc.IsPlayer)
        {
            cc.ApplyDamage(Damage, transform.position);
        }

        Instantiate(HitParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
