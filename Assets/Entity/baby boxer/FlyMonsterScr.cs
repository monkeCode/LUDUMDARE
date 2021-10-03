using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonsterScr : Entity
{
    [SerializeField] private Transform target;
    [SerializeField] private float distanceOvertarget;
    [SerializeField] private float attackDistance;
    [SerializeField] private float timeInterAttack;
    [SerializeField] private GameObject thunder;
    private Rigidbody2D _rb;
    private bool _canAtk =true;
    private Animator _animator;
    private static readonly int Move1 = Animator.StringToHash("move");
    private static readonly int Attack1 = Animator.StringToHash("attack");
    private float movementX, movementY = 0;

    private static readonly int Die1 = Animator.StringToHash("die");

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp > 0)
        {
            if(target != null)
                _animator.SetBool(Move1,true);
            if (Vector2.Distance(_rb.position, target.position) > attackDistance || (_rb.position.y - target.position.y < distanceOvertarget && !GroundCheck()))
            {
                Move();
            }
           else Attack();
            _rb.velocity = new Vector2(movementX, movementY) * Speed * Time.deltaTime;
            movementX = 0;
            movementY = 0;
        }
    }

    private void Attack()
    {
        if (_canAtk)
        {
            _animator.SetTrigger(Attack1);
            _canAtk = false;
        }
    }

    private void DealDamage()
    {
        var thun = Instantiate(thunder);
        var hit = Physics2D.Raycast(target.position, Vector2.down, float.MaxValue, LayerMask.GetMask("Ground"));

        thun.transform.position = hit.point;
       StartCoroutine(WaitNewAtk());
    }
    
    IEnumerator WaitNewAtk()
    {
        yield return new WaitForSeconds(timeInterAttack);
        _canAtk = true;
    }
    public override void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0 && isLife)
        {
            _animator.SetTrigger(Die1);
            isLife = false;
        }
    }

    bool GroundCheck()
    {
        Debug.DrawRay(_rb.position, Vector2.up);
        var hit = Physics2D.Raycast(_rb.position, Vector2.up, 1, LayerMask.GetMask("Ground"));
        Debug.Log(hit.rigidbody?.gameObject);
        return hit.rigidbody != null;
    }
    void Move()
    {
        float distance = Vector2.Distance(_rb.position, target.position);
        
       if ( _rb.position.y - target.position.y < distanceOvertarget && !GroundCheck())
       {
           movementY = 1;
       }
       else if( distance > attackDistance)
       {
           movementY = -1;
       }
        
       if (distance > attackDistance)
       {
           movementX = (((Vector2) target.position - _rb.position).x > 0 ? 1 : -1);
       }

       if (movementX != 0)
           transform.localScale = new Vector3(movementX * Math.Abs(transform.localScale.x), transform.localScale.y,
               transform.localScale.z);
      
    }
    
}
