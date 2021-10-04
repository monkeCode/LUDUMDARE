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
        Destroy(gameObject);
    }
        
    public override void Overheat()
    {
        var rnd = Random.Range(0, 200);
        if (rnd < 30)
        {
            StartCoroutine(OverheatExplosion());
        }
    }

    private void Explosion()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        var enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerEnemies);
        foreach (var enemy in enemies)
        {
            DealDamage(enemy.GetComponent<IDamagable>());
        }
    }

    private IEnumerator OverheatExplosion()
    {
        yield return new WaitForSeconds(overheatTimeToExplosion);
        Explosion();
        Destroy(gameObject);
    }
}