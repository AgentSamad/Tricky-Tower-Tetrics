using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputSystem input;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float snappingDistance = 0.1f;
    private bool gameOver;


    private void Start()
    {
        TetrisSpawner.SpawnTetris?.Invoke(spawnPoint);
        GameEvents.OnGameOver += GameOver;

        if (TetrisSpawner.SpawnTetris == null)
            print("Its Null");

        input.Init();

        print("I am enabled");
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= GameOver;
    }

    private void Update()
    {
        if(gameOver || _gameConfig.isPaused) return;
        input.ControlTetris(this.transform, snappingDistance);
    }

    void GameOver()
    {
        gameOver = true;
    }
}