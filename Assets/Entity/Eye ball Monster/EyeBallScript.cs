using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;


public class EyeBallScript : Entity
{
   [SerializeField] private Transform target;
   [SerializeField] private float waitBeforeRoll;
   [SerializeField] private float pushForce;
   [SerializeField] private float groundRadius;
   private Rigidbody2D _rb;
   private Animator _animator;
   private CircleCollider2D _collider;
   private static readonly int Roll1 = Animator.StringToHash("roll");
   private bool _roll = true;
   private int _lastSpeed = 1;
   private static readonly int Die1 = Animator.StringToHash("die");
   private bool _onGround;
   private int dir;
   private void Start()
   {
      _rb = GetComponent<Rigidbody2D>();
      _animator = GetComponent<Animator>();
      _collider = GetComponent<CircleCollider2D>();
      target = GameObject.FindGameObjectWithTag("Player").transform;
    }

   private void Update()
   {
      if (Hp > 0)
      {
         _onGround = Physics2D.OverlapCircle (new Vector2(_rb.position.x, _rb.position.y -  _collider.radius), groundRadius, LayerMask.GetMask("Ground"));
         transform.localScale = new Vector3(_lastSpeed * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
         if (Vector2.Distance(_rb.position, target.position) < _collider.radius)
         {
            DealDamage();
         }

         Move();
      }
   }

   void Move()
   {
      if (_roll)
      {     
            if(_onGround)
               dir = (target.position.x - _rb.position.x)>0 ? 1 : -1;
            _rb.velocity += Vector2.right * Speed * Time.deltaTime * dir;
            if (WallEnter())
            {
               _rb.velocity = new Vector2(-_rb.velocity.x, _rb.velocity.y);
            }
            if (_lastSpeed != (_rb.velocity.x > 0?1:-1) && _onGround)
            {
               _roll = false;
               _animator.SetBool(Roll1, false);
               StartCoroutine(PrepareRoll());
            }

            _lastSpeed = _rb.velocity.x > 0?1:-1;
      }
   }

   bool WallEnter()
   {
      //Debug.DrawRay(new Vector2(_rb.position.x, _rb.position.y + _collider.radius), (new Vector2(_collider.radius, 0)) * (_rb.velocity.x > 0 ? 1 : -1));
      
      var hits = Physics2D.RaycastAll(new Vector2(_rb.position.x, _rb.position.y + _collider.radius),
         _rb.velocity.normalized, _collider.radius).ToList();
      return hits.Find(hit2D => hit2D.rigidbody?.gameObject?.layer == LayerMask.NameToLayer("Ground") || hit2D.rigidbody?.gameObject?.layer == LayerMask.NameToLayer("Walls") );
      
   }
   void DealDamage()
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
      _roll = true;
   }
}
