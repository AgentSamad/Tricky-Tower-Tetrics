using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private int playerLives, aiLives;


    private void Awake()
    {
        playerLives = aiLives = _gameConfig.initialLives;
    }

    void Start()
    {
        GameEvents.InvokeGameStarted();
        GameEvents.OnLivesChanged += OnLivesChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnLivesChanged -= OnLivesChanged;
    }


    void OnLivesChanged(Participant participant)
    {
        if (participant == Participant.player)
        {
            playerLives--;
            if (playerLives <= 0)
                GameEvents.InvokeGameOver();

            UiManager.onDecreaseHearts?.Invoke(playerLives);
        }
        else
        {
            aiLives--;
            if (aiLives <= 0)
                GameEvents.InvokeGameWin();
        }
    }

    void OnHeightIncreased(Participant participant)
    {
        if (participant == Participant.player)
        {
            // var height = ActivePieces.GetHighestActivePiece();
            //
            // if (height > currentTowerHeight.value + highestHeightThreshould)
            // {
            //     currentTowerHeight.value = height;
            //     OnHeightIncreased.Raise();
            // }
            //
            // if (height > currentMilestoneHeight.value)
            // {
            //     currentMilestoneHeight.value += difficultyFactor;
            //     StartCoroutine(MoveWinningMarker());
            //     OnMilestoneReached.Raise();
            // }
        }
        else
        {
        }
    }
}