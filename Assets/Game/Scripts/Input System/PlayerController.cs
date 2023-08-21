using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputSystem input;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Participant participant;
    [SerializeField] private Transform spawnPoint;
    private bool gameOver;


    private void Start()
    {
        TetrisSpawner.SpawnTetris?.Invoke(spawnPoint, participant);
        GameEvents.OnGameOver += StopMoving;
        GameEvents.OnGameWin += StopMoving;

        if (TetrisSpawner.SpawnTetris == null)
            print("Its Null");

        input.Init();

        //  print("I am enabled");
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= StopMoving;
        GameEvents.OnGameWin -= StopMoving;
    }

    private void Update()
    {
        if (gameOver || _gameConfig.isPaused) return;
        input.ControlTetris(this.transform, participant);
    }

    void StopMoving()
    {
        gameOver = true;
    }
}