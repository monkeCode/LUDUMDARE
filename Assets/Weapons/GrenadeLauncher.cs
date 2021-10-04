using System.Collections;
using UnityEngine;

public class GrenadeLauncher : Bullet
{
    public float explosionRadius;
    public float overheatTimeToExplosion = .3f;
    public GameObject explosion;
    public GameObject sound;

    public override void DealDamage(IDamagable enemy)
    {
        Explosion();
        Destroy(gameObject);
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
        Debug.Log(damage);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(sound, transform.position, Quaternion.identity);
        var enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerEnemies);
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<IDamagable>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    private IEnumerator OverheatExplosion()
    {
        yield return new WaitForSeconds(overheatTimeToExplosion);
        Explosion();
        Destroy(gameObject);
    }
}