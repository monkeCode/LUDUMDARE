using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;


public class EyeBallScript : Entity
{
   [SerializeField] private Transform target;
   [SerializeField] private float waitBeforeRoll;
   [SerializeField] private float pushForce;
   private Rigidbody2D _rb;
   private Animator _animator;
   private CircleCollider2D _collider;
   private static readonly int Roll1 = Animator.StringToHash("roll");
   private bool roll = true;
   private float lastSpeed = 1;
   private static readonly int Die1 = Animator.StringToHash("die");
  

   private void Start()
   {
      _rb = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
      _collider = GetComponent<CircleCollider2D>();
   }

   private void Update()
   {
      if (Hp > 0)
      {
         transform.localScale = new Vector3(lastSpeed * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
         if (Vector2.Distance(_rb.position, target.position) < _collider.radius)
         {
            target.GetComponent<IDamagable>()?.TakeDamage(Damage);
            try
            {
               target.GetComponent<Rigidbody2D>()?.AddForce((Vector2.left * _rb.velocity.normalized.x + Vector2.up) * pushForce);
            }
            catch (Exception _)
            {
               // ignored
            }
         }

         if (roll)
         {
            _rb.velocity += Vector2.right * Speed * Time.deltaTime * ((target.position.x - _rb.position.x)>0?1:-1);
            if (lastSpeed != _rb.velocity.normalized.x)
            {
               roll = false;
               _animator.SetBool(Roll1, false);
               StartCoroutine(PrepareRoll());
            }

            lastSpeed = math.abs(_rb.velocity.x) / _rb.velocity.x;
         }
      }
   }

   public override void TakeDamage(int damage)
   {
      Hp -= damage;
      if(Hp <=0)
         _animator.SetTrigger(Die1);
   }

   private IEnumerator PrepareRoll()
   {
      yield return new  WaitForSeconds(waitBeforeRoll);
      _animator.SetBool(Roll1, true);
      roll = true;
   }
}
