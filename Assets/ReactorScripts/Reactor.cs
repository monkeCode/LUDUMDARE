using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        internal bool isRequested;

        public static event EventHandler<ReactorEventHealth> OnHealthChanged;
        public static event EventHandler<TypeItem> OnItemRequired;
        
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            isRequested = true;
            requiredItem.type = TypeItem.Flamethrower;
            OnItemRequired?.Invoke(this, requiredItem.type);
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
            OnItemRequired?.Invoke(this, TypeItem.Default);
            Notify(new ReactorEventHealth(health, state, false));
        }

        public void TakeDamage(int damage)
        {
            health = Mathf.Max(health - damage, 0);
            Notify(new ReactorEventHealth(health, state, health == 0));
            if (health < requirementHpForRequestItem && isRequested == false)
            {
                isRequested = true;
                GenerateRequirement();
                OnItemRequired?.Invoke(this, requiredItem.type);
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

        private void GenerateRequirement()
        {
            var values = Enum.GetValues(typeof(TypeItem));
            var random = new System.Random();
            var randomItem = (TypeItem)values.GetValue(random.Next(values.Length - 1) + 1);
            requiredItem.type = randomItem;
        }
    }
}
