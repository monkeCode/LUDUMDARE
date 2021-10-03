using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    void Start()
    {
        LaserManager.Instance.AddLaser(this);
    }
    
}