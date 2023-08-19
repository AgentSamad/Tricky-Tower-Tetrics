using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private float highestHeightThreshould;
    [SerializeField] private int playerHeight;
    [SerializeField] private int aiHeight;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private float heightMulitplayer = 1;

    private GameManager gameManager;

    private void Awake()
    {
        playerHeight = aiHeight = 0;
        GameEvents.OnHeightChanged += CalculateHeight;
        gameManager = GetComponent<GameManager>();
    }

    private void OnDisable()
    {
        GameEvents.OnHeightChanged -= CalculateHeight;
    }

    int GetHighestActivePiece(List<Tetris> tetrisList)
    {
        int highestHeight = 0;
        foreach (var p in tetrisList)
        {
            if (p != null)
            {
                var pHeight = p.transform.GetColliderHighetsY();
                if (pHeight > highestHeight)
                    highestHeight = pHeight;
            }
        }

        return highestHeight;
    }


    public void CalculateHeight(Participant participant)
    {
        if (participant == Participant.player)
        {
            var height = GetHighestActivePiece(TetrisSpawner.playerActiveTetris);

            if (height > playerHeight + highestHeightThreshould)
            {
                playerHeight = height;
                UiManager.onHeightIncrease?.Invoke((int)(height * heightMulitplayer));
            }
        }

        else
        {
            var ai = GetHighestActivePiece(TetrisSpawner.aiActiveTetris);
            if (ai > aiHeight + highestHeightThreshould)
            {
                aiHeight = ai;
                print("Ai Height " + ai);
            }
        }


        float tolerence = 0.2f;

        if (Math.Abs(playerHeight - _gameConfig.maxHeightToWin) < tolerence)
        {
            if (gameManager.GetState() == GameState.Playing)
                GameEvents.InvokeGameWin();
        }

        if (Math.Abs(aiHeight - _gameConfig.maxHeightToWin) < tolerence)
        {
            if (gameManager.GetState() == GameState.Playing)
                GameEvents.InvokeGameOver();
        }
    }
}