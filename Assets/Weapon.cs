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
    public Dictionary<TypeItem, Bullet> ConvertTypeItemToBulletType;
    public ConvertItemToBullet convertor;

    private void Start()
    {
        weaponPos = gameObject.transform;
        ConvertTypeItemToBulletType = convertor.GetBulletDictionary();
    }

    public void Shoot(Vector3 direction, ItemData bullet)
    {
        if (!onCooldown)
        {
            bulletType = ConvertTypeItemToBulletType[bullet.type];
            attackCooldown = bulletType.attackCooldown;
            var roatZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0, 0, roatZ + offset);
            var flyingBullet = Instantiate(bulletType, weaponPos.position, rotation);
            if (overheat > 33)
                flyingBullet.Overheat();
            if (overheat > 66)
                flyingBullet.Overheat();
            if (overheat > 99)
                flyingBullet.Overheat();
            if (overheat < 100)
                overheat++;
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
