using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class EyeBallScript : Entity
{
   [SerializeField] private Transform target;
   [SerializeField] private float waitBeforeRoll;
   private Rigidbody2D _rb;
   private Animator _animator;
   private CircleCollider2D _collider;
   private static readonly int Roll1 = Animator.StringToHash("roll");
   private bool roll = true;
   private float lastSpeed = 1;
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
         transform.localScale = new Vector3(lastSpeed, transform.localScale.y, transform.localScale.z);
         if (Vector2.Distance(_rb.position, target.position) < _collider.radius)
         {
            target.GetComponent<IDamagable>()?.TakeDamage(Damage);
            Debug.Log("DAMAGE");
         }

         if (roll)
         {
            _rb.velocity += Vector2.right * Speed * Time.deltaTime * (target.position.x - _rb.position.x) /
                            Math.Abs(target.position.x - _rb.position.x);
            if (lastSpeed != math.abs(_rb.velocity.x) / _rb.velocity.x)
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
         _animator.SetTrigger("die");
   }

   private IEnumerator PrepareRoll()
   {
      yield return new  WaitForSeconds(waitBeforeRoll);
      _animator.SetBool(Roll1, true);
      roll = true;
   }
}
