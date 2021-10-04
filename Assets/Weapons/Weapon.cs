using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using ReactorScripts;
using UnityEngine;
using Weapons;

public class Weapon : MonoBehaviour
{
    public float overheat;
    public float offset = 0;
    // public Bullet bulletType;
    public Bullet bulletType;
    public Transform weaponPos;
    public bool onCooldown;
    public float attackCooldown;
    public Dictionary<TypeItem, Bullet> ConvertTypeItemToBulletType;
    public ConvertItemToBullet convertor;

    public event EventHandler<float> OverheatChanged;

    private void Start()
    {
        ConvertTypeItemToBulletType = convertor.GetBulletDictionary();
        FlamethrowerBullet.PlayerBurning = null;
    }

    public void Shoot(Vector3 direction, ItemData bullet)
    {
        if (!onCooldown)
        {
            bulletType = ConvertTypeItemToBulletType[bullet.type];
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
                overheat += flyingBullet.overheatIncrement;
            OverheatChanged?.Invoke(this, overheat);
            attackCooldown = flyingBullet.attackCooldown;
            
            StartCoroutine(AttackCooldown());
        }
    }

    public void Cooling(float cooling)
    {
        overheat = Mathf.Max(0, overheat - cooling);
        OverheatChanged?.Invoke(this, overheat);
        if (FlamethrowerBullet.PlayerBurning != null)
            StopCoroutine(FlamethrowerBullet.PlayerBurning);
    }

    private IEnumerator AttackCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        onCooldown = false;
    }
}
