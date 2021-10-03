using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBullet : Bullet
{
    [SerializeField] private int reflections;
    private int overheatReflectionCount = 2;
    public int currentReflection = 0;
    private LineRenderer lineRenderer;
    private float destroyTime = 10f;
    public LayerMask player;
    
    private new void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(currentReflection, shootPos);
        StartCoroutine(DestroyAfterTime());
    }
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
    }

    public override void OnCollisionWithGround(Collision2D other)
    {
        if (currentReflection < reflections)
        {
            var reflection = Vector2.Reflect(velocity, other.contacts[0].normal);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(currentReflection+1, other.contacts[0].point);
            Rigidbody.velocity = reflection;
            transform.right = reflection.normalized;
            var hits = Physics2D.RaycastAll(other.contacts[0].point, Rigidbody.velocity, flyDistance, layerEnemies);
            foreach (var hit in hits)
            {
                DealDamage(hit.rigidbody.gameObject.GetComponent<IDamagable>());
            }

            var playerHit = Physics2D.Raycast(other.contacts[0].point, Rigidbody.velocity, flyDistance, player);
            if (playerHit)
                DealDamage(playerHit.rigidbody.gameObject.GetComponent<IDamagable>());
            currentReflection++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }


    public override void Overheat()
    {
        reflections += overheatReflectionCount;
    }
    

    public override void Cooling()
    {
        //attackCooldown /= overheatAttackCooldownCoefficient;
    }
}
