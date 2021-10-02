using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PistolBullet : Bullet
{
    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        collider = gameObject.GetComponent<CircleCollider2D>();
        shootPos = gameObject.transform;
        speed = 0; //setValue
        damage = 0; //setValue
        flyDistance = 0; //setValue
        attackCooldown = 0; //setValue
    }

    public override void DealDamage(GameObject enemy)
    {
        enemy.GetComponent<IDamagable>().TakeDamage(damage);
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