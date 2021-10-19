using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Key : MonoBehaviour
{
    public new string name;
    public GameObject[] lockedIcon;
    public float offset;
    public float tickOffcet;
    private Vector2 _startPosition;
    private bool upNow = true;
    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        // Debug.Log(transform.position.y - _startPosition.y);
        transform.position +=(Vector3)(upNow ? Vector2.up : Vector2.down) * tickOffcet;
        if (transform.position.y - _startPosition.y > offset)
            upNow = false;
       else if (_startPosition.y - transform.position.y > offset)
            upNow = true;
    }

    private void OnDestroy()
    {
        foreach (var lc in lockedIcon)
        {
            Destroy(lc);
        }
        
    }
}
