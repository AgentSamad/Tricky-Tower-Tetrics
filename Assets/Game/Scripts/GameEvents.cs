using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents
{
    public static Action OnTetrisRotate;
    public static Action OnTetrisDash;


    public static Action OnGameStarted;
    public static Action OnGameOver;
    public static Action OnLivesChanged;

    public static void TetrisRotateInvoke()
    {
        OnTetrisRotate?.Invoke();
    }

    public static void TetrisDashInvoke()
    {
        OnTetrisDash?.Invoke();
    }

    public static void InvokeGameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void InvokeGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public static void InvokeLivesChanged()
    {
        OnLivesChanged?.Invoke();
    }
}