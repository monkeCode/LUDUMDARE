using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float attackCooldown;
    public new Rigidbody2D rigidbody;
    public LayerMask enemies;
    public LayerMask ground;
    public int damage;
    public Transform shootPos;
    public float flyDistance;
    public new CircleCollider2D collider;
    public Animator animator;
    

    void FixedUpdate()
    {
        rigidbody.velocity = (transform.up + transform.right) * speed;
        if ((transform.position - shootPos.position).magnitude >= flyDistance)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemies)
        {
            DealDamage(other.gameObject);
        }

        if (other.gameObject.layer == ground)
        {
            SelfDestroy();
        }
    }
    public abstract void DealDamage(GameObject enemy);
    public abstract void SelfDestroy();
    public abstract void Overheat();

    public abstract void Cooling();
}