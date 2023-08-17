using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private float highestHeightThreshould;
    [SerializeField] private IntValue playerHeight;
    [SerializeField] private IntValue aiHeight;
    [SerializeField] private int heightMulitplayer=1;

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
            var pHeight = p.transform.GetColliderHighetsY();
            if (pHeight > highestHeight)
                highestHeight = pHeight;
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
                print("Player Height" + height);
                UiManager.onHeightIncrease?.Invoke(height * heightMulitplayer);
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
    }
}