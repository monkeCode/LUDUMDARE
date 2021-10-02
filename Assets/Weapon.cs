using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int overheat;
    public float offset;
    public Bullet bullet;
    public Transform weaponPos;
    public bool onCooldown;
    public float attackCooldown;

    private void Start()
    {
        attackCooldown = bullet.attackCooldown;
        bullet = gameObject.AddComponent<PistolBullet>();
    }

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
