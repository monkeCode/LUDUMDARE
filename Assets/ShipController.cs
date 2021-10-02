using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShipController : MonoBehaviour
{
    private GameObject LevelCamera;
    private GameObject Player;
    private float SpinTime = 0;
    private float TimeAfterTurnBack = 0;
    private PlayerInput input;


    [Header("Stats")]
    public float maxTurnBackSpeed = 0.5f;
    public float timeSpeedDependence;
    public bool EbuttonIsPressed = false;
    private float maxTimeAfterTurnBack;
    public float minTimeStartSpinning;
    public float maxTimeStartSpinning;
    public float timeNotSpinAfterLevelStart;

    private float currentTurnSpeed = 0;
    private float turnBackSpeed = 0.0f;
    private float rapidTurnDegree;
    private float rapidTurnDegreeSpeed;
    private bool isRapidTurnAway = false;
    private bool isTurningBack = false;
    private int turnDirection;
    private bool isTurningAway = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        var inputPlayer = Player.GetComponent<Player>().Input;
        inputPlayer.Player.Action.performed += ctx => EbuttonIsPressed = true;
        inputPlayer.Player.Action.canceled += ctx => EbuttonIsPressed = false;
    }

    void ChooseTurnDirection()
    {
        turnDirection = UnityEngine.Random.Range(-2, 1);
        if (turnDirection == -2)
        {
            turnDirection += 1;
        }

        if (turnDirection == 0)
        {
            turnDirection += 1;
        }
    }
    void TurnAway()
    {
        isTurningAway = true;
        turnBackSpeed = 0.0f;
        currentTurnSpeed += SpinTime * timeSpeedDependence;
        LevelCamera.transform.Rotate(0.0f, 0.0f, turnDirection*currentTurnSpeed);
    }

    void TurnBack()
    {
        isTurningBack = true;
        turnBackSpeed = Mathf.Lerp(turnBackSpeed, maxTurnBackSpeed, SpinTime);
        if (LevelCamera.transform.localRotation.eulerAngles.z - turnBackSpeed < 0)
        {
            LevelCamera.transform.localRotation = Quaternion.Euler(LevelCamera.transform.localRotation.eulerAngles.x,LevelCamera.transform.localRotation.eulerAngles.y , 0);
        }
        else
        {
            LevelCamera.transform.Rotate(0.0f, 0.0f, turnDirection*(-turnBackSpeed));   
        }
        
        if (LevelCamera.transform.localRotation.eulerAngles.z == 0)
        {
            TimeAfterTurnBack = 0;
            maxTimeAfterTurnBack = UnityEngine.Random.Range(minTimeStartSpinning,maxTimeStartSpinning);
            SpinTime = 0;
            currentTurnSpeed = 0;
            ChooseTurnDirection();
            isTurningAway = false;
        }
    }

    public void StartRapidTurnAway()
    {
        isRapidTurnAway = true;
        rapidTurnDegree = UnityEngine.Random.Range(5, 15);
    }
    
    void RapidTurnAway()
    {
        rapidTurnDegreeSpeed = Mathf.Lerp(0, 0.01f, SpinTime);
        LevelCamera.transform.Rotate(0.0f,0.0f, turnDirection*rapidTurnDegreeSpeed);
        rapidTurnDegree -= rapidTurnDegreeSpeed;
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if ((EbuttonIsPressed)  && (other.gameObject.CompareTag("Player")))
        {

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
        SpinTime = 0.0f;
        maxTimeAfterTurnBack = timeNotSpinAfterLevelStart;
        ChooseTurnDirection();
    }

    void Update()
    {

        SpinTime += Time.deltaTime;
        TimeAfterTurnBack += Time.deltaTime;

        if (!isTurningAway)
        {
            SpinTime = 0;
            currentTurnSpeed = 0;
        }

        
        if (!isTurningBack && (TimeAfterTurnBack > maxTimeAfterTurnBack))
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
