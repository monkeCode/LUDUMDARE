using System;
using UnityEngine;

namespace ReactorScripts
{
    public class Reactor : MonoBehaviour, IDamagable
    {
        public int health;
        public int maxHealth;

        public ItemData requiredItem;
        public int requirementHpForRequestItem; 

        public int damagePerPeriod = 1;
        public float delayBetweenDamage = 1f;
        
        private float lastTimeDamageTaken;
        private bool isRequested;

        public event EventHandler<ReactorEventHealth> OnHealthChanged;
        public event EventHandler<ReactorEventRequirement> OnItemRequired; 

        private void LateUpdate()
        {
            if (Time.time - lastTimeDamageTaken > delayBetweenDamage)
            {
                lastTimeDamageTaken = Time.time;
                TakeDamage(damagePerPeriod);
            }
        }

        public void GetRepair(ItemData item)
        {
            if (item.Type != requiredItem.Type)
                return;
            isRequested = false;
            health = Mathf.Min(health + item.Repair, maxHealth);
            Notify(new ReactorEventHealth(health, false));
        }

        public void TakeDamage(int damage)
        {
            health = Mathf.Max(health - damage, 0);
            Notify(new ReactorEventHealth(health, health == 0));
            if (health < requirementHpForRequestItem && isRequested == false)
            {
                isRequested = true;
                OnItemRequired?.Invoke(this, new ReactorEventRequirement(requiredItem.Type));
            }
        }

        private void Notify(ReactorEventHealth eventHealth)
        {
            OnHealthChanged?.Invoke(this, eventHealth);
        }
    }
}
