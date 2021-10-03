using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LaserManager : MonoBehaviour
{
    public static LaserManager Instance;
    private List<LaserGun> lasers = new List<LaserGun>();
    private List<GameObject> lines = new List<GameObject>();
    public GameObject linePrefab;
    private float maxStepDistance = 20;
    void Awake()
        {
            Instance = this;
        }

    public void AddLaser(LaserGun laser)
    {
        lasers.Add(laser);
    }

    public void RemoveLaser(LaserGun laser)
    {
        lasers.Remove(laser);
    }

    public void RemoveOldLines(int linesCount)
    {
        if (lines.Count > linesCount)
        {
            Destroy(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count - 1);
            RemoveOldLines(linesCount);
        }
    }

    public void Update()
    {
        var linesCount = 0;
        foreach (var laser in lasers)
        {
            linesCount += CalcLaserLine(laser.transform.position, laser.transform.forward, linesCount);
        }   
        RemoveOldLines(linesCount);
    }

    int CalcLaserLine(Vector2 startPos, Vector2 direction, int index)
    {
        int result = 1;
        RaycastHit hit;
        Ray ray = new Ray(startPos, direction);
        bool intersect = Physics.Raycast(ray, out hit, maxStepDistance);
        Vector3 hitPosition = hit.point;
        if (!intersect)
        {
            hitPosition = startPos + direction * maxStepDistance;
        }
        DrawLaser(startPos, hitPosition, index);
        if (intersect)
        {
            result += CalcLaserLine(hitPosition, Vector2.Reflect(direction, hit.normal), index);
        }

        return result;
        // int result = 1;
        // if (Physics2D.Raycast(startPos, direction).collider is null)
        // {
        //     Vector2 hitPos = startPos + direction * maxStepDistance;
        //     DrawLaser(startPos, hitPos, index);
        // }
        // if (Physics2D.Raycast(startPos, direction).collider != null)
        // {
        //     var hit = Physics2D.Raycast(startPos, direction);
        //     Vector2 hitPos = hit.point;
        //     result += CalcLaserLine(hitPos, Vector2.Reflect(direction, hit.normal), index+result);
        // }
        //
        // return result;
    }

    public void DrawLaser(Vector2 startPos, Vector2 finishPos, int index)
    {
        LineRenderer line = null;
        if (index < lines.Count)
        {
            line = lines[index].GetComponent<LineRenderer>();
        }
        else
        {
            GameObject laser = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            line = laser.GetComponent<LineRenderer>();
            lines.Add(laser);
        }
        line.SetPosition(0, startPos);
        line.SetPosition(1, finishPos);
    }
}
