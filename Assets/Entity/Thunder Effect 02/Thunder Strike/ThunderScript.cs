using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = System.Random;

public class ThunderScript : MonoBehaviour
{
    [SerializeField] private int damage;
    private List<IDamagable> _entityList = new List<IDamagable>();
    private Light2D light;
    void DealDamage()
    {
        foreach (var entity in _entityList)
        {
            entity.TakeDamage(damage);
        }
    }

    private void Start()
    {
        light = GetComponent<Light2D>();
    }

    private void Update()
    {
        light.intensity = new Random().Next(100,300) / 100f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent(out IDamagable component);
        if (component != null)
        {
            _entityList.Add(component);
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        other.TryGetComponent(out IDamagable component);
        if (component != null)
        {
            _entityList.Remove(component);
        }
    }
}
