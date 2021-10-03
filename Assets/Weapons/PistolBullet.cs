using UnityEngine;

public class PistolBullet : Bullet
{
    public float overheatAttackCooldownCoefficient = 1.5f;
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }

    public override void OnCollisionWithGround(Collision2D other)
    {
        Destroy(gameObject);
    }

    public override void Overheat()
    {
        attackCooldown *= overheatAttackCooldownCoefficient;
    }
    

    public override void Cooling()
    {
        attackCooldown /= overheatAttackCooldownCoefficient;
    }
}