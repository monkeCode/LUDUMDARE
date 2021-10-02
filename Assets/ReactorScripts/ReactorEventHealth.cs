namespace ReactorScripts
{
    public class ReactorEventData
    {
        public int Health;
        public bool IsExplosion;

        public ReactorEventData(int health, bool isExplosion)
        {
            Health = health;
            IsExplosion = isExplosion;
        }
    }
}