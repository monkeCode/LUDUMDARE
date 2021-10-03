using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class FlamethrowerBullet : Bullet
    {
        public int burnDamage = 1;
        public int burnTimes = 3;
        public float timeBetweenBurn = 1;
        public float timeAlive = 5;

        private static Coroutine _playerBurning;

        private new void Start()
        {
            base.Start();
            Rigidbody.velocity = Vector2.zero;
            Destroy(gameObject.GetComponent<Collider2D>(), timeAlive);
        }
        
        public override void DealDamage(IDamagable enemy)
        {
            enemy.TakeDamage(damage);
            StartCoroutine(Burn(enemy));
        }

        public override void OnCollisionWithGround(Collision2D other)
        {
            
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & layerEnemies) != 0)
            {
                DealDamage(other.gameObject.GetComponent<IDamagable>());
            }
        }

        public override void Overheat()
        {
            if (_playerBurning != null)
                return;
            var player = FindObjectOfType<Player>();
            _playerBurning = StartCoroutine(Burn(player, true));
        }

        public override void Cooling()
        {
            if (_playerBurning != null)
                StopCoroutine(_playerBurning);
        }

        private IEnumerator Burn(IDamagable enemy, bool isPlayer = false)
        {
            for (var _ = 0; _ < burnTimes; _++)
            {
                yield return new WaitForSeconds(timeBetweenBurn);
                enemy.TakeDamage(burnDamage);
            }
            if (isPlayer)
                _playerBurning = null;
            Destroy(gameObject, timeAlive);
        }
    }
}