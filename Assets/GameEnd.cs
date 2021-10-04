using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private Camera LevelCamera;
    private CameraManager CameraManagerScript;

    void Start()
    {
        LevelCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>() as Camera;
        CameraManagerScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraManager>();
        SpaceShipTimer.TimeExceeded += (sender, b) => GameWin();
    }

    void Update()
    {
        
    }

    void GameWin()
    {
        CameraManagerScript.FocusOnObject(CameraManagerScript.GameOverFocusPoint, 100);
    }
}
