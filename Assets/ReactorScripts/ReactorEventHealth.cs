using UnityEngine;

namespace ReactorScripts
{
    public class ReactorEventHealth
    {
        public int Health;
        public bool IsExplosion;
        public States State;

        public ReactorEventHealth(int health, States state, bool isExplosion)
        {
            Health = health;
            State = state;
            IsExplosion = isExplosion;
        }
    }
}