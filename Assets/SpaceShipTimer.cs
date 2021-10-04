using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipTimer : MonoBehaviour
{
    public float timeToWin;
    public float timeTick;

    private float lastTimeTick;
    private float currentTime;

    public static event EventHandler<(float Current, float Max)> TimeChanged;

    private void FixedUpdate()
    {
        if (Time.time - lastTimeTick > timeTick)
        {
            lastTimeTick = Time.time;
            currentTime += timeTick;
            TimeChanged?.Invoke(this, (currentTime, timeToWin));
        }
    }
}
