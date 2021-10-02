using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class MonsterScript : Entity
{
    // Start is called before the first frame update
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToAtk;
    [SerializeField] private float _timeToAtk;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _readyToAtk;
    
    private static readonly int Move1 = Animator.StringToHash("move");
    private static readonly int die = Animator.StringToHash("die");
    private static readonly int Attack = Animator.StringToHash("atk");

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _readyToAtk = true;
    }


    void Update()
    {
        if (Hp > 0)
        {
            Debug.DrawRay(_rb.position, (Vector2)_target.position - _rb.position, Color.white);
            //Debug.Log(Vector2.Angle((Vector2)_target.position -_rb.position , Math.Abs(_rb.velocity.x) / _rb.velocity.x * Vector2.right)* 180 / Math.PI);
            if (Vector2.Distance(_target.position, _rb.position) > _distanceToAtk )
            {
                Move();
            }
            else
            {
                Atk();
            }
        }
    }

    void Move()
    {
        _rb.velocity = new Vector2(
            (_target.position.x -
            _rb.position.x) / Math.Abs(_target.position.x - _rb.position.x) * Speed * Time.deltaTime,
            _rb.velocity.y);
        _rb.transform.localScale = new Vector3(Math.Abs(_rb.velocity.x) / _rb.velocity.x, 1, 1);
        if (Math.Abs(_rb.velocity.x) > 0.1f)
            _animator.SetBool(Move1, true);
        else
            _animator.SetBool(Move1, false);
    }

    public override void TakeDamage(int damage)
    {
        Hp -= Damage;
        if (Hp <= 0)
        {
            _animator.SetTrigger(die);
        }
    }

    
    void Atk()
    {
        if (!_readyToAtk) return;
        
        _animator.SetTrigger(Attack);
        _readyToAtk = false;
        StartCoroutine(PrepairingToATK());
    }

    void DealDamage()
    {
        var hits =  Physics2D.RaycastAll(_rb.position, (Vector2)_target.position - _rb.position, _distanceToAtk).ToList();
        if (hits.Find(hit2D => hit2D.transform.gameObject == _target.gameObject ))
        {
            _target.GetComponent<IDamagable>()?.TakeDamage(Damage);
        }
            
    }
    IEnumerator PrepairingToATK()
    {
        yield return new WaitForSeconds(_timeToAtk);
        _readyToAtk = true;
    }
}
