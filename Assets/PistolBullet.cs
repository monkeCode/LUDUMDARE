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
        speed = 10; //setValue
        damage = 10; //setValue
        flyDistance = 5; //setValue
        attackCooldown = 3; //setValue
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
        attackCooldown *= 1.3f;
    }
    

    public override void Cooling()
    {
        attackCooldown /= 1.3f;
    }
}