using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBullet : Bullet
{
    [SerializeField] private int reflections = 2;
    public LaserTrigger trigger;
    // public List<Tuple<Vector2, Vector2>> reflectionData = new List<Tuple<Vector2, Vector2>>();
    public int currentReflection = 0;
    private LineRenderer lineRenderer;
    
    private new void Start()
    {
        base.Start();
        speed = 10f;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(currentReflection, shootPos);
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
            // reflectionData.Add(new Tuple<Vector2, Vector2>(reflection, other.contacts[0].point));
            Rigidbody.velocity = reflection;
            transform.right = reflection.normalized;
            currentReflection++;
        }
        else
        {
            Destroy(gameObject);
        }
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
