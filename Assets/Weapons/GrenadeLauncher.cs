using UnityEngine;

public class GrenadeLauncher : Bullet
{
    public float explosionRadius;
        
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
    }

    public override void OnCollisionWithGround(Collision2D other)
    {
        var enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerEnemies);
        foreach (var enemy in enemies)
        {
            DealDamage(enemy.GetComponent<IDamagable>());
        }
        Destroy(gameObject);
    }
        
    public override void Overheat()
    {
        throw new System.NotImplementedException();
    }

    public override void Cooling()
    {
        throw new System.NotImplementedException();
    }
}