using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int Hp;
    [SerializeField]
    private int Damage;
    
    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(this.gameObject);
    }
}
