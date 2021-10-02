using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : Bullet
{
    [SerializeField] private int reflections = 2;
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
    }

    public override void OnCollisionWithGround(Collision2D other)
    {
        var reflection = Vector2.Reflect(velocity, other.contacts[0].normal);
        rigidbody.velocity = reflection;
        transform.right = reflection.normalized;
    }

    public override void Overheat()
    {
        //attackCooldown *= overheatAttackCooldownCoefficient;
    }
    

    public override void Cooling()
    {
        //attackCooldown /= overheatAttackCooldownCoefficient;
    }
}
