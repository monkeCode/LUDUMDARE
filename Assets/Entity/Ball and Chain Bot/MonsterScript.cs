using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine.Utility;
using Unity.Mathematics;
using UnityEngine;

public class MonsterScript : Entity
{
    // Start is called before the first frame update
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToAtk;
    [SerializeField] private float _timeToAtk;
    [SerializeField] private float _atkAngle;
    [SerializeField] private LayerMask lookMask;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _readyToAtk;
    private AudioSource _source;
    
    private static readonly int Move1 = Animator.StringToHash("move");
    private static readonly int die = Animator.StringToHash("die");
    private static readonly int Attack = Animator.StringToHash("atk");
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _readyToAtk = true;
        _source = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Hp > 0)
        {
            //Debug.DrawRay(_rb.position, (Vector2)_target.position - _rb.position, Color.white);
            //Debug.Log(Vector2.Angle(((Vector2)_target.position -_rb.position).Abs() , Vector2.right));
            int dir = _target.position.x - _rb.position.x > 0 ? 1 : -1;
            if (Vector2.Distance(_target.position, _rb.position) > _distanceToAtk /* || Vector2.Angle(((Vector2)_target.position -_rb.position).Abs() , Vector2.right) > _atkAngle */)
            {
                Move(dir);
            }
            else if(LookAtPlayer())
            {
                Atk();
            }
            else
            {
                _animator.SetBool(Move1, false);
            }
            
            _rb.transform.localScale = new Vector3(dir * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    bool LookAtPlayer()
    {
        var vec = (Vector2) _target.position - _rb.position;
        Debug.DrawRay(_rb.position, vec);
        var hit =  Physics2D.Raycast(_rb.position, vec, vec.magnitude,lookMask);
        return hit.rigidbody?.gameObject?.tag == "Player";
    }
    void Move(int dir)
    {
        _animator.SetBool(Move1, true);
       
        _rb.velocity = new Vector2( dir * Speed * Time.deltaTime,
            _rb.velocity.y);
        
      
    }

    public override void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0 && isLife)
        {
            _animator.SetTrigger(die);
            isLife = false;
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
        _source.Play();
        if (Vector2.Angle(((Vector2) _target.position - _rb.position).Abs(), Vector2.right) < _atkAngle)
        {
            var hits = Physics2D.RaycastAll(_rb.position, (Vector2) _target.position - _rb.position, _distanceToAtk)
                .ToList();
            if (hits.Find(hit2D => hit2D.transform.gameObject == _target.gameObject))
            {
                _target.GetComponent<IDamagable>()?.TakeDamage(Damage);
            }
        }
    }
    IEnumerator PrepairingToATK()
    {
        yield return new WaitForSeconds(_timeToAtk);
        _readyToAtk = true;
    }
}
