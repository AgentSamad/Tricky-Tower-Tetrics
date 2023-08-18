using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private float highestHeightThreshould;
    [SerializeField] private IntValue playerHeight;
    [SerializeField] private IntValue aiHeight;
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private float heightMulitplayer = 1;


    private void Awake()
    {
        playerHeight.value = aiHeight.value = 0;
        GameEvents.OnHeightChanged += CalculateHeight;
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

            if (height > playerHeight.value + highestHeightThreshould)
            {
                playerHeight.value = height;
                UiManager.onHeightIncrease?.Invoke((int)(height * heightMulitplayer));
            }
        }

        else
        {
            var ai = GetHighestActivePiece(TetrisSpawner.aiActiveTetris);
            if (ai > aiHeight.value + highestHeightThreshould)
            {
                aiHeight.value = ai;
                print("Ai Height" + ai);
            }
        }


        float tolerence = 0.2f;

        if (Math.Abs(playerHeight.value - _gameConfig.maxHeightToWin) < tolerence)
        {
            GameEvents.InvokeGameWin();
        }

        if (Math.Abs(aiHeight.value - _gameConfig.maxHeightToWin) < tolerence)
        {
            GameEvents.InvokeGameOver();
        }
    }
}