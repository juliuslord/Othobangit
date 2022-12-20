using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour    // This script it used exclusively in the Main Menu scene //
{
    public void MenuScene()
    {
        Debug.Log("Changing scene to main menu");       // These functions are accessed by 
        SceneManager.LoadScene(0);
    }

    public void TutorialScene()
    {
        Debug.Log("changing scene to gameplay");
        SceneManager.LoadScene(1);
    }

    public void LevelOneScene()
    {
        Debug.Log("changing scene to gameplay");
        SceneManager.LoadScene(2);
    }

    public void LevelTwoScene()
    {
        Debug.Log("changing scene to gameplay");
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
}
