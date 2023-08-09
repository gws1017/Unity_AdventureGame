using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHelath;
    public int CurrentHelath;
    private Character character;

    private void Awake()
    {
        CurrentHelath = MaxHelath;
        character = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHelath -= damage;
        Debug.Log(gameObject.name + " damage " + damage);
        Debug.Log(gameObject.name + " hp " + CurrentHelath);
    }
}
