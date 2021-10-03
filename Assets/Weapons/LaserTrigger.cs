using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    public LayerMask layerEnemies;
    public LaserBullet source;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layerEnemies) != 0)
        {
            source.DealDamage(other.gameObject.GetComponent<IDamagable>());
        }
    }
}
