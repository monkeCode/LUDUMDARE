using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject PausedMenu;
    private GameObject Player; 
    private PlayerInput input;
    private bool isPaused = false;

    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
    }
    
    public void PauseGame()
    {
        PausedMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PausedMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }

    void Start()
    {
        var inputPlayer = Player.GetComponent<Player>().Input;
        inputPlayer.Absolute.Escape.performed += ctx =>
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        };
    }
}
