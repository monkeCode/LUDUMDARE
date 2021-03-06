using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected int Hp;
    [SerializeField]
    protected int MaxHp;
    [SerializeField]
    protected int Damage;
    [SerializeField]
    protected float Speed;

    protected bool isLife = true;
    public event EventHandler<(int Current, int Max)> HealthChanged;

    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;
        HealthChanged?.Invoke(this, (Hp, MaxHp));
        if (Hp <= 0)
            Die();
    }

    public virtual void RestoreHp(int restore)
    {
        Hp = Mathf.Min(Hp + restore, MaxHp);
        HealthChanged?.Invoke(this, (Hp, MaxHp));
    }

    protected virtual void Die()
    {
        Destroy(this.gameObject);
    }
}
