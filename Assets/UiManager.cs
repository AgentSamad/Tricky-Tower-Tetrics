using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Search;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("UI Canvas References")] [SerializeField]
    private TextMeshProUGUI heightText;

    [SerializeField] private Transform livesParent;
    [SerializeField] private Image nextTetrisImage;


    [Header("Pannels")] [SerializeField] private GameObject gameplayPannel;
    [SerializeField] private GameObject pausePannel;
    [SerializeField] private GameObject gameOverPannel;
    [SerializeField] private GameObject buttonsPannel;

    [Header("Buttons")] [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button pauseBtn;
    [Header("Prefabs")] [SerializeField] private GameObject hearts;

    [Header("Game Configurations")] [SerializeField]
    private GameConfig _gameConfig;


    private List<Image> spawnedHearts = new List<Image>();
    private int currentLives;

    void Awake()
    {
        GameEvents.OnGameStarted += SetUI;
        GameEvents.OnLivesChanged += SetLives;
        GameEvents.OnGameOver += GameOver;

        pauseBtn.onClick.AddListener(PauseButtonEvent);
        restartBtn.onClick.AddListener(RestartButtonEvent);
        mainMenuBtn.onClick.AddListener(MainMenuButtonEvent);

        currentLives = _gameConfig.initialLives;
        _gameConfig.isPaused = false;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= SetUI;
        GameEvents.OnLivesChanged -= SetLives;
        GameEvents.OnGameOver -= GameOver;
        
        pauseBtn.onClick.RemoveAllListeners();
        restartBtn.onClick.RemoveAllListeners();
        mainMenuBtn.onClick.RemoveAllListeners();
    }


    void SetUI()
    {
        SpawnHearts();
    }


    void SpawnHearts()
    {
        for (int i = 0; i < _gameConfig.initialLives; i++)
        {
            spawnedHearts.Add(Instantiate(hearts, livesParent).GetComponent<Image>());
        }
    }


    void SetLives()
    {
        currentLives--;

        if (currentLives <= 0)
            GameEvents.InvokeGameOver();


        int index = currentLives;

        if (index >= 0)
        {
            spawnedHearts[index].transform.DOScale(1.5f, 1f).SetEase(Ease.OutElastic);
            spawnedHearts[index].transform.DOScale(0f, 0.5f).SetEase(Ease.InOutElastic).SetDelay(0.5f);
            spawnedHearts[index].DOFade(0, 0.5f).SetDelay(1f);
        }
    }

    void PauseButtonEvent()
    {
        if (!_gameConfig.isPaused)
        {
            _gameConfig.isPaused = true;
            restartBtn.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
        }
        else
        {
            _gameConfig.isPaused = false;
            restartBtn.gameObject.SetActive(false);
            mainMenuBtn.gameObject.SetActive(false);
        }
    }

    void RestartButtonEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void MainMenuButtonEvent()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void GameOver()
    {
        gameplayPannel.SetActive(false);
        gameOverPannel.SetActive(true);

        restartBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
    }
}