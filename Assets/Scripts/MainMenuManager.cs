using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "Scene2";
    public string levelGame = "Level";
    public void StartGame()
    {
 
        SceneManager.LoadScene(gameSceneName);
        Time.timeScale = 1f;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Level()
    {

        SceneManager.LoadScene(levelGame);
        Time.timeScale = 1f;
    }
}