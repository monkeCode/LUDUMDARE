using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PistolBullet : Bullet
{
    private void Start()
    {
        speed = 10; //setValue
        damage = 10; //setValue
        flyDistance = 10; //setValue
        attackCooldown = 3; //setValue
    }

    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    public override void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public override void Overheat()
    {
        throw new NotImplementedException();
    }
}