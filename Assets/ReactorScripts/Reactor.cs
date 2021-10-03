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

        public static event EventHandler<ReactorEventHealth> OnHealthChanged;
        public static event EventHandler<ReactorEventRequirement> OnItemRequired;
        
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

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
            if (item.type != requiredItem.type)
                return;
            isRequested = false;
            health = Mathf.Min(health + item.repair, maxHealth);
            Notify(new ReactorEventHealth(health, state, false));
        }

        public void TakeDamage(int damage)
        {
            health = Mathf.Max(health - damage, 0);
            Notify(new ReactorEventHealth(health, state, health == 0));
            if (health < requirementHpForRequestItem && isRequested == false)
            {
                isRequested = true;
                OnItemRequired?.Invoke(this, new ReactorEventRequirement(requiredItem.type));
            }
            if (health > maxHealth * 0.66)
                state = States.fullHP;
            else if (health > maxHealth * 0.33)
                state = States.mediumHP;
            else
                state = States.lowHP;
        }
        public States state
        {
            get
            {
                return (States)animator.GetInteger("state");
            }
            set
            {
                animator.SetInteger("state", (int)value);
            }
        }

        private void Notify(ReactorEventHealth eventHealth)
        {
            OnHealthChanged?.Invoke(this, eventHealth);
        }
    }
}
