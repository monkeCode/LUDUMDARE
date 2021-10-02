public class PistolBullet : Bullet
{
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    public override void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public override void Overheat()
    {
        attackCooldown *= 1.3f;
    }
    

    public override void Cooling()
    {
        attackCooldown /= 1.3f;
    }
}