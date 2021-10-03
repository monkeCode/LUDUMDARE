using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Cinemachine.Utility;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float offcetSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem pS;
    [SerializeField] private float lifeTime;
    [SerializeField] private float explosiveRadiusModifier;
    [SerializeField] private float pushForce;
    [SerializeField] private int damage;
    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = ((Vector2)target.position -  _rb.position).normalized * offcetSpeed * Time.deltaTime;
        _rb.velocity += dir;
        if (Math.Abs(_rb.velocity.x) > speed)
            _rb.velocity = new Vector2(_rb.velocity.normalized.x * speed, _rb.velocity.y); 
        if (Math.Abs(_rb.velocity.y) > speed)
            _rb.velocity = new Vector2(_rb.velocity.x , _rb.velocity.normalized.y * speed); 

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Explosive();
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifeTime);
        Explosive();
    }
    private void Explosive()
    {
        pS.Play();
        pS.gameObject.transform.SetParent(null);
        pS.GetComponent<SelfDestroyByTime>().StartDestroy();
       var colliders = Physics2D.OverlapCircleAll(_rb.position, GetComponent<CircleCollider2D>().radius * explosiveRadiusModifier);
       foreach (var collider in colliders)
       {
           collider.TryGetComponent(out IDamagable component);
           if (component != null)
           {
               Debug.Log("entity");
               if (collider.gameObject == target.gameObject)
               { 
                   component.TakeDamage(damage);
               }
               collider.GetComponent<Rigidbody2D>()?.AddForce(-((Vector2)target.position - _rb.position) * pushForce);
           }

       }
        Destroy(this.gameObject);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
    
}
