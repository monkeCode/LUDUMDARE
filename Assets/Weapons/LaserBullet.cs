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
    private float destroyTime = 0.2f;

    private new void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(currentReflection, shootPos);
        // var hits = Physics2D.RaycastAll(transform.position, Rigidbody.velocity, flyDistance, layerEnemies);
        // foreach (var hit in hits)
        // {
        //     DealDamage(hit.rigidbody.gameObject.GetComponent<IDamagable>());
        // }
        StartCoroutine(DestroyAfterTime());
        LaserMagic(shootPos, transform.right);
    }
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
    }

    public override void OnCollisionWithGround(Collision2D other)
    {
        // if (currentReflection < reflections)
        // {
        //     var reflection = Vector2.Reflect(velocity, other.contacts[0].normal);
        //     lineRenderer.positionCount++;
        //     lineRenderer.SetPosition(currentReflection+1, other.contacts[0].point);
        //     // Rigidbody.velocity = reflection;
        //     // transform.right = reflection.normalized;
        //     var hits = Physics2D.RaycastAll(other.contacts[0].point, Rigidbody.velocity, flyDistance, layerEnemies);
        //     foreach (var hit in hits)
        //     {
        //         DealDamage(hit.rigidbody.gameObject.GetComponent<IDamagable>());
        //     }
        //     currentReflection++;
        // }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }

    public void LaserMagic(Vector2 startPosition, Vector2 direction)
    {
        var hit = Physics2D.Raycast(startPosition, direction, flyDistance, map);
        var hits = Physics2D.RaycastAll(startPosition, direction, hit.collider != null?hit.distance:flyDistance, layerEnemies);
        foreach (var enemyHit in hits)
        {
            DealDamage(enemyHit.rigidbody.gameObject.GetComponent<IDamagable>());
        }
        Debug.Log(hit.rigidbody.gameObject);
        if (hit.rigidbody.gameObject != null)
        {
            // Debug.DrawLine(startPosition, hit.point,Color.cyan);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(currentReflection+1, hit.point);
            currentReflection++;
            if (currentReflection < reflections)
            {
               LaserMagic(hit.point + hit.normal, Vector2.Reflect(direction, hit.normal).normalized);
            }
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    protected override void OnTheEdge()
    {
        var point = transform.position;
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(currentReflection+1, point);
    }


    public override void Overheat()
    {
        reflections += overheatReflectionCount;
    }
}
