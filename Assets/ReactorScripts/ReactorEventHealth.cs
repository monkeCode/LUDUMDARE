namespace ReactorScripts
{
    public class ReactorEventHealth
    {
        public int Health;
        public bool IsExplosion;

        public ReactorEventHealth(int health, bool isExplosion)
        {
            Health = health;
            IsExplosion = isExplosion;
        }
    }
}