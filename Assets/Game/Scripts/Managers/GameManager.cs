using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private GameConfig _gameConfig;
    private int playerLives, aiLives;


    private void Awake()
    {
        playerLives = aiLives = _gameConfig.initialLives;

        GameEvents.OnGameStarted += GameStarted;
        GameEvents.OnGameWin += GameWin;
        GameEvents.OnGameOver += GameLose;
        GameEvents.OnLivesChanged += OnLivesChanged;
    }

    void Start()
    {
        GameEvents.InvokeGameStarted();
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= GameStarted;
        GameEvents.OnGameWin -= GameWin;
        GameEvents.OnGameOver -= GameLose;
        GameEvents.OnLivesChanged -= OnLivesChanged;
    }


    void OnLivesChanged(Participant participant)
    {
        if (participant == Participant.player)
        {
            playerLives--;
            if (playerLives <= 0)
            {
                if (_gameState == GameState.Playing)
                    GameEvents.InvokeGameOver();
            }


            UiManager.onDecreaseHearts?.Invoke(playerLives);
        }
        else
        {
            aiLives--;
            if (aiLives <= 0)
            {
                if (_gameState == GameState.Playing)
                    GameEvents.InvokeGameWin();
            }
        }
    }

    void GameStarted()
    {
        _gameState = GameState.Playing;

        //Vibration
        Vibration.Init();
    }

    void GameWin()
    {
        _gameState = GameState.Win;
    }

    void GameLose()
    {
        _gameState = GameState.Lose;
    }

    public GameState GetState()
    {
        return _gameState;
    }
}