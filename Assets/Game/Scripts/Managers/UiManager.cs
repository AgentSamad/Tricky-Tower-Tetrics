using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Search;
using UnityEngine.Serialization;
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
    [SerializeField] private GameObject gameWinPannel;
    [SerializeField] private GameObject buttonsPannel;
    [SerializeField] private GameObject aiRender;

    [Header("Buttons")] [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button pauseBtn;
    [Header("Prefabs")] [SerializeField] private GameObject hearts;


    [Header("Particles")] [SerializeField] private ParticleSystem confetti;

    [Header("Game Configurations")] [SerializeField]
    private GameConfig gameConfig;


    private List<Image> spawnedHearts = new List<Image>();

    public static Action<int> onDecreaseHearts;
    public static Action<int> onHeightIncrease;

    void Awake()
    {
        onDecreaseHearts += DecreaseUIHearts;
        onHeightIncrease += IncreaseHeight;
        GameEvents.OnGameStarted += SetUI;
        GameEvents.OnNextTetrisImage += SetNextTetris;
        GameEvents.OnGameOver += GameOver;
        GameEvents.OnGameWin += GameWin;

        pauseBtn.onClick.AddListener(PauseButtonEvent);
        restartBtn.onClick.AddListener(RestartButtonEvent);
        mainMenuBtn.onClick.AddListener(MainMenuButtonEvent);


        aiRender.SetActive(gameConfig.gameMode == GameConfig.GameMode.Multiplayer);


        gameConfig.isPaused = false;
    }

    private void OnDisable()
    {
        onDecreaseHearts -= DecreaseUIHearts;
        onHeightIncrease -= IncreaseHeight;
        GameEvents.OnGameStarted -= SetUI;
        GameEvents.OnNextTetrisImage -= SetNextTetris;
        GameEvents.OnGameOver -= GameOver;
        GameEvents.OnGameWin -= GameWin;

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
        for (int i = 0; i < gameConfig.initialLives; i++)
        {
            spawnedHearts.Add(Instantiate(hearts, livesParent).GetComponent<Image>());
        }
    }


    void DecreaseUIHearts(int index)
    {
        if (index >= 0)
        {
            spawnedHearts[index].transform.DOScale(1.5f, 1f).SetEase(Ease.OutElastic);
            spawnedHearts[index].transform.DOScale(0f, 0.5f).SetEase(Ease.InOutElastic).SetDelay(0.5f);
            spawnedHearts[index].DOFade(0, 0.5f).SetDelay(1f);
        }
    }

    void IncreaseHeight(int height)
    {
        heightText.SetText($"{height}m");
        heightText.transform.DOShakeScale(.4f, 1, 10, 90);
    }

    void PauseButtonEvent()
    {
        if (!gameConfig.isPaused)
        {
            gameConfig.isPaused = true;
            restartBtn.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
            pausePannel.SetActive(true);
        }
        else
        {
            gameConfig.isPaused = false;
            restartBtn.gameObject.SetActive(false);
            mainMenuBtn.gameObject.SetActive(false);
            pausePannel.SetActive(false);
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


    void SetNextTetris(Sprite sprite)
    {
        nextTetrisImage.sprite = sprite;
    }

    void GameOver()
    {
        gameplayPannel.SetActive(false);
        gameOverPannel.SetActive(true);
        aiRender.SetActive(false);

        restartBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    void GameWin()
    {
        gameplayPannel.SetActive(false);
        gameWinPannel.SetActive(true);
        aiRender.SetActive(false);

        restartBtn.gameObject.SetActive(true);
        mainMenuBtn.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);

        confetti.Play();
    }
}