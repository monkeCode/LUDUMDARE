using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BotScript : Entity
{
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToAtk;
    [SerializeField] private float minimalDistance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gunPos;
    [Range(0,10)]
    [SerializeField] private float bulletForce;
    private Rigidbody2D _rb;
    private Animator _animator;
    private BoxCollider2D _collider;
    private bool _canShoot = true;
    private static readonly int Shoot1 = Animator.StringToHash("shoot");
    private static readonly int Move1 = Animator.StringToHash("move");
    private static readonly int Die1 = Animator.StringToHash("die");

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp > 0)
        {
           bool isWall = WallCheck();
           float distance = Vector2.Distance(target.position, _rb.position);
          if ((distance > distanceToAtk || (distance < minimalDistance && !isWall )) && _canShoot)
            {
                Move((distance > distanceToAtk?1:-1) * (target.position.x - _rb.position.x> 0?1:-1));
            }
            else
            {
                Shoot();
            }
        }
    }

    bool WallCheck()
    {
        var hits = Physics2D.RaycastAll(_rb.position,
            new Vector2(-((Vector2) target.position - _rb.position).normalized.x, 0), minimalDistance/2).ToList();
        
        return hits.Find(hit2D => hit2D.rigidbody?.gameObject?.layer == LayerMask.NameToLayer("Ground") || hit2D.rigidbody?.gameObject?.layer == LayerMask.NameToLayer("Walls") || hit2D.rigidbody?.gameObject?.layer == LayerMask.NameToLayer("SideDoors") );
    }
    void Shoot()
    {
        int dir = (int)((target.position.x - _rb.position.x) / Math.Abs(target.position.x - _rb.position.x));
        if(dir == 0)
            return;
        transform.localScale = new Vector3(dir, transform.localScale.y, transform.localScale.z);
        if (_canShoot)
        {
            _animator.SetTrigger(Shoot1);
            _canShoot = false;
        }
    }

    void DealDamage()
    {
        //target.GetComponent<IDamagable>()?.TakeDamage(Damage);
        var b = Instantiate(bullet);
        b.transform.position = gunPos.position;
        b.GetComponent<Rigidbody2D>().AddForce(new Vector2(_rb.velocity.normalized.x * bulletForce,0));
        b.GetComponent<PortalScript>().SetTarget(target);
        _canShoot = true;
    }
    void Move(int dir)
    {
        _rb.velocity = new Vector2(dir * Speed * Time.deltaTime, _rb.velocity.y);
        transform.localScale = new Vector3(dir * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        _animator.SetBool(Move1,true);
    }

    public override void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            _animator.SetTrigger(Die1);
        }
    }
}
