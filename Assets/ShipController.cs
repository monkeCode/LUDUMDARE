using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShipController : MonoBehaviour
{
    private GameObject LevelCamera;
    private GameObject Player;
    private float GameTime = 0;
    private PlayerInput input;


    [Header("Stats")]
    public float maxTurnBackSpeed = 0.5f;
    public float timeSpeedDependence;
    public bool EbuttonIsPressed = false;

    private float currentTurnSpeed = 0;
    private float turnBackSpeed = 0.0f;
    private float rapidTurnDegree;
    private float rapidTurnDegreeSpeed;
    private bool isRapidTurnAway = false;
    private bool isTurningBack = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        var inputPlayer = Player.GetComponent<Player>().Input;
        inputPlayer.Player.Action.performed += ctx => EbuttonIsPressed = true;
        inputPlayer.Player.Action.canceled += ctx => EbuttonIsPressed = false;
    }

    void TurnAway()
    {
        turnBackSpeed = 0.0f;
        currentTurnSpeed += GameTime * timeSpeedDependence;
        LevelCamera.transform.Rotate(0.0f, 0.0f, currentTurnSpeed);
    }

    void TurnBack()
    {
        turnBackSpeed = Mathf.Lerp(turnBackSpeed, maxTurnBackSpeed, 2*GameTime);
        if (LevelCamera.transform.localRotation.eulerAngles.z - turnBackSpeed < 0)
        {
            LevelCamera.transform.localRotation = Quaternion.Euler(LevelCamera.transform.localRotation.eulerAngles.x,LevelCamera.transform.localRotation.eulerAngles.y , 0);
        }
        else
        {
            LevelCamera.transform.Rotate(0.0f, 0.0f, -turnBackSpeed);   
        }
    }

    public void StartRapidTurnAway()
    {
        isRapidTurnAway = true;
        rapidTurnDegree = UnityEngine.Random.Range(5, 15);
        Debug.Log(rapidTurnDegree);
    }
    
    void RapidTurnAway()
    {
        rapidTurnDegreeSpeed = Mathf.Lerp(0, 0.01f, GameTime);
        LevelCamera.transform.Rotate(0.0f,0.0f, rapidTurnDegreeSpeed);
        rapidTurnDegree -= rapidTurnDegreeSpeed;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((EbuttonIsPressed) && (LevelCamera.transform.localRotation.eulerAngles.z != 0) && (other.gameObject.CompareTag("Player")))
        {
            Debug.Log("knopka");
            isTurningBack = true;
            TurnBack();
        }
        else
        {
            isTurningBack = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTurningBack = false;
        }
    }

    void Start()
    {
        LevelCamera = GameObject.FindWithTag("MainCamera");
        GameTime = 0.0f;
    }

    void Update()
    {
        if (EbuttonIsPressed)
        {
            Debug.Log("EBUTTONISPRESSED");
        }
        
        GameTime += Time.deltaTime;
        
        if (!isTurningBack)
        {
            TurnAway();
        }

        if (isRapidTurnAway)
        {
            if (rapidTurnDegree <= 0)
            {
                isRapidTurnAway = false;
            }
            else
            {
                RapidTurnAway();
            }
        }
        
    }
}
