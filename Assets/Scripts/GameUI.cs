using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public string sceneNameToLoad;
    [SerializeField] private GameManager gameManager;
    public void StartGame()
    {
        gameManager.StartGame();
    }

  

    public void ReplayGame()
    {
        gameManager.ReplayGame();
    }

   
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        gameManager.ResumeGame();
    }

    public void PauseGame()
    {
        gameManager.PauseGameMenu();
    }
    public void WinGame()
    {
        gameManager.GameWinMenu();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNameToLoad);
       
    }
}
