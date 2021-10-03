using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame ()
    {
        SceneManager.LoadScene("SpaceShip");
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!"); // because you can't quit from unity editor
        Application.Quit();
    }

}
