using System;
using UnityEngine;

public class Reactor : MonoBehaviour, IDamagable
{
    public int health;
    public int maxHealth;

    public int damagePerPeriod = 1;
    public float delayBetweenDamage = 1f;
    private float lastTimeDamageTaken;

    public event EventHandler<ReactorEventData> OnHealthChanged;

    void LateUpdate()
    {
        if (Time.time - lastTimeDamageTaken > delayBetweenDamage)
        {
            lastTimeDamageTaken = Time.time;
            TakeDamage(damagePerPeriod);
        }
    }

    public void GetRepair(int repair)
    {
        health = Mathf.Min(health + repair, maxHealth);
        Notify(new ReactorEventData(health, false));
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(health - damage, 0);
        Notify(new ReactorEventData(health, health == 0));
    }

    private void Notify(ReactorEventData eventData)
    {
        OnHealthChanged?.Invoke(this, eventData);
    }
}
