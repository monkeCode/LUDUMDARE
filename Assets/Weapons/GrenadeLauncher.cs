using System.Collections;
using UnityEngine;

public class GrenadeLauncher : Bullet
{
    public float explosionRadius;
    public float overheatTimeToExplosion = .3f;
    public GameObject explosion;
        
    public override void DealDamage(IDamagable enemy)
    {
        enemy.TakeDamage(damage);
    }

    public override void OnCollisionWithGround(Collision2D other)
    {
        Explosion();
    }
        
    public override void Overheat()
    {
        var rnd = Random.Range(0, 200);
        if (rnd < 30)
        {
            StartCoroutine(OverheatExplosion());
        }
    }

    public override void Cooling()
    {
        
    }

    private void Explosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        var enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerEnemies);
        foreach (var enemy in enemies)
        {
            DealDamage(enemy.GetComponent<IDamagable>());
        }
        Destroy(gameObject);
    }

    private IEnumerator OverheatExplosion()
    {
        yield return new WaitForSeconds(overheatTimeToExplosion);
        Explosion();
    }
}