using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon<TBullet> : MonoBehaviour where TBullet : Bullet
{
    public float attackCooldown;
    public int overhear;
    public float offset;
    public TBullet bullet;
    public Transform weaponPos;
    public bool onCooldown;

    public void Shoot(Vector3 direction)
    {
        if (!onCooldown)
        {
            var roatZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0, 0, roatZ + offset);
            Instantiate(bullet, weaponPos.position, rotation);
            StartCoroutine(AttackCooldown());
        }
    }
    
    IEnumerator AttackCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }
}
