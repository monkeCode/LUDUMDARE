using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene("SpaceShip");
    }

    public void GoMainMenu ()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!"); // because you can't quit from unity editor
        Application.Quit();
    }
   

    void Start()
    {
        Time.timeScale = 1;
        // SceneManager.UnloadSceneAsync("SpaceShip");
    }
}
