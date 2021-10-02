using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ReactorScripts;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int overheat;
    public float offset = -45;
    // public Bullet bulletType;
    public Bullet bulletType;
    public Transform weaponPos;
    public bool onCooldown;
    public float attackCooldown;
    public Dictionary<TypeItem, Bullet> Convertor;
    public ConvertItemToBullet convert;

    private void Start()
    {
        weaponPos = gameObject.transform;
        Convertor = convert.GetBulletDictionary();
    }

    public void Shoot(Vector3 direction, ItemData bullet)
    {
        if (!onCooldown)
        {
            bulletType = Convertor[bullet.Type];
            attackCooldown = bulletType.attackCooldown;
            var roatZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0, 0, roatZ + offset);
            Instantiate(bulletType, weaponPos.position, rotation);
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
