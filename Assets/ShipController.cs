using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ShipController : MonoBehaviour
{
    private GameObject LevelCamera;
    private GameObject Player; 


    private float spinTime = 0;
    private float turningBackTime = 0;
    private float timeAfterTurnBack = 0;
    private float timeTurningBack = 0;
    private PlayerInput input;
    
    [Header("Select manually (SpaceShip/Canvas)")]
    public GameObject DeadMenu;
    public GameObject OffCourseMenu;


    [Header("Stats")] 
    public float speedBackSmoothener; // suggestion 125
    public float maxTurnBackSpeed = 0.5f;
    public float timeSpeedDependence;
    public bool EbuttonIsPressed = false;
    public bool playerNearYoke = false;
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
    public int turnDirection;
    private bool isTurningAway = false;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
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
        timeTurningBack = 0;
        turnBackSpeed = 0;
        isTurningAway = true;
        currentTurnSpeed += spinTime * timeSpeedDependence;
        LevelCamera.transform.Rotate(0.0f, 0.0f, turnDirection*currentTurnSpeed);
    }

    void TurnBack()
    {
        timeTurningBack += Time.deltaTime;
        spinTime = 0;
        isTurningBack = true;
        turnBackSpeed = Mathf.Lerp(turnBackSpeed, maxTurnBackSpeed, timeTurningBack/speedBackSmoothener);
        //Debug.Log("TurnBackSpeed = " + turnBackSpeed);
        
       // Debug.Log("IF +1 " + (LevelCamera.transform.localRotation.eulerAngles.z - turnBackSpeed));
        if (LevelCamera.transform.localRotation.eulerAngles.z - turnBackSpeed <= 0 && turnDirection == 1)
        {
           // Debug.Log("1 here 1");
            LevelCamera.transform.localRotation = Quaternion.Euler(LevelCamera.transform.localRotation.eulerAngles.x,LevelCamera.transform.localRotation.eulerAngles.y , 0);
        }
        else
        {
            if (turnDirection == 1)
            {
                //Debug.Log("1 here 2");
                LevelCamera.transform.Rotate(0.0f, 0.0f, turnDirection*(-turnBackSpeed));
            }
        }
        
        Debug.Log(LevelCamera.transform.localRotation.eulerAngles.z);
        // Debug.Log("IF -1 " + (LevelCamera.transform.localRotation.eulerAngles.z + turnBackSpeed));
        if ((LevelCamera.transform.localRotation.eulerAngles.z + turnBackSpeed >= -1) && (LevelCamera.transform.localRotation.eulerAngles.z + turnBackSpeed <= 0.7f) && (turnDirection == -1))
        {
         //   Debug.Log("2 here 1");
            LevelCamera.transform.localRotation = Quaternion.Euler(LevelCamera.transform.localRotation.eulerAngles.x,LevelCamera.transform.localRotation.eulerAngles.y , 0);
        }
        else
        {
            if (turnDirection == -1)
            {
             //   Debug.Log("2 here 2");
                LevelCamera.transform.Rotate(0.0f, 0.0f, turnDirection*(-turnBackSpeed));
            }
        }
        
        if (LevelCamera.transform.localRotation.eulerAngles.z == 0)
        {
            timeAfterTurnBack = 0;
            timeTurningBack = 0;
            maxTimeAfterTurnBack = UnityEngine.Random.Range(minTimeStartSpinning,maxTimeStartSpinning);
            spinTime = 0;
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
        rapidTurnDegreeSpeed = Mathf.Lerp(0, 0.01f, spinTime);
        LevelCamera.transform.Rotate(0.0f,0.0f, turnDirection*rapidTurnDegreeSpeed);
        rapidTurnDegree -= rapidTurnDegreeSpeed;
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            playerNearYoke = true;
        }
        else
        {
            playerNearYoke = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearYoke = false;
        }
    }

    void Start()
    {
        var inputPlayer = Player.GetComponent<Player>().Input;
        inputPlayer.Player.Action.performed += ctx => EbuttonIsPressed = true;
        inputPlayer.Player.Action.canceled += ctx => EbuttonIsPressed = false;
        LevelCamera = GameObject.FindWithTag("MainCamera");
        OffCourseMenu.SetActive(false);
        DeadMenu.SetActive(false);
        spinTime = 0.0f;
        maxTimeAfterTurnBack = timeNotSpinAfterLevelStart;
        ChooseTurnDirection();
    }

    void FixedUpdate()
    {
        spinTime += Time.deltaTime;
        timeAfterTurnBack += Time.deltaTime;
        
        if ((EbuttonIsPressed)  && (playerNearYoke) && (isTurningAway))
        {
            TurnBack();
        }
        else
        {
            isTurningBack = false;
        }
        
   //     Debug.Log(LevelCamera.transform.localRotation.eulerAngles.z);
   

        if (!isTurningAway)
        {
            spinTime = 0;
            currentTurnSpeed = 0;
        }

        
        if (!isTurningBack && (timeAfterTurnBack > maxTimeAfterTurnBack))
        {
            turnBackSpeed = 0;
            timeTurningBack = 0;
            TurnAway();
            
        }
        
     //   Debug.Log("IsTurningBack = " + isTurningBack);
       // Debug.Log("CurrentDirection = " + turnDirection);
        //Debug.Log("CurrentAngle = " + LevelCamera.transform.localRotation.eulerAngles.z);
        
        if (!isTurningBack)
        {
            if (turnDirection < 0)
            {
                if ((LevelCamera.transform.localRotation.eulerAngles.z <= 180) && !DeadMenu.activeSelf && (LevelCamera.transform.localRotation.eulerAngles.z > 0))
                {
                    OffCourseMenu.SetActive(true);
                    Player.GetComponent<Player>().OnDisable();
                    Time.timeScale = 0;

                    //Debug.Log("Dead 1");
                }
            }
            else
            {
                if ((LevelCamera.transform.localRotation.eulerAngles.z >= 180) && !DeadMenu.activeSelf)
                {
                    OffCourseMenu.SetActive(true);
                    Player.GetComponent<Player>().OnDisable();
                    Time.timeScale = 0;

                    //Debug.Log("Dead 2");
                }
            }
        }
    }
}
