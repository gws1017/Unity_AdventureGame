using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHelath;
    public int CurrentHelath;
    private Character m_character;

    private void Awake()
    {
        CurrentHelath = MaxHelath;
        m_character = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        CurrentHelath -= damage;
        Debug.Log(gameObject.name + " damage " + damage);
        Debug.Log(gameObject.name + " hp " + CurrentHelath);
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(CurrentHelath <= 0)
        {
            m_character.SwitchStateTo(Character.CharacterState.Dead);
        }
    }
}
