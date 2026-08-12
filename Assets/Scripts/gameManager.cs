using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentMoney;
    [SerializeField] private int moneyThreshold = 20;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemy;
    private bool bossCalled = false;
    [SerializeField] private Image moneyBar;
    [SerializeField] GameObject gameUI;
    [SerializeField] private EnemySpawn enemySpawn;
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameWinMenu;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerController player;
    [SerializeField] private float healValue = 20f;
   
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    void Start()
    {

        currentMoney = 0;
        UpdateMoney();
        boss.SetActive(false);
    }



    public void AddMoney()
    {
        if (bossCalled) return;

        currentMoney += 1;
        UpdateMoney();
        if (currentMoney == moneyThreshold)
        {
            CallBoss();

        }
    }

    private void CallBoss()
    {
        enemySpawn.ClearEnemy();
        bossCalled = true;
        boss.SetActive(true);
        enemy.SetActive(false);
        gameUI.SetActive(true);
        audioManager.PlayBossAudio();

    }

    private void UpdateMoney()
    {
        if (moneyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentMoney / moneyThreshold);
            moneyBar.fillAmount = fillAmount;
        }

    }

    public void HealPlayer()
    {
        player.Heal(healValue);
    }

    public void GameOverMenu()
    {
        enemy.SetActive(false);
        gameOverMenu.SetActive(true);
        pauseGameMenu.SetActive(false);
        gameWinMenu.SetActive(false);
        Time.timeScale = 0f;

    }

    public void GameWinMenu()
    {
        gameWinMenu.SetActive(true);
        Time.timeScale = 1f;
    }
     
    public void PauseGameMenu()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;

        if(bossCalled != true)
            audioManager.StopAudioDefault();
        else if (bossCalled == true)
            audioManager.StopAudioBoss();
    }

    public void StartGame()
    {
    
        gameOverMenu.SetActive(false);
        pauseGameMenu.SetActive(false);
   
        gameWinMenu.SetActive(false);
        Time.timeScale = 1f;
        audioManager.PlayDefaultAudio();

    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    public void ResumeGame()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;

        if(bossCalled != true)
            audioManager.ContinueAudioDefault();
        else if(bossCalled == true)
            audioManager.ContinueAudioBoss();
        if (pauseGameMenu != null)
        {
            pauseGameMenu.SetActive(false);
        }
        
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (gameWinMenu != null) gameWinMenu.SetActive(false);
    }

}
