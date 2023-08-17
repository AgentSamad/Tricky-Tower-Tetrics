using System;
using UnityEngine;

public class GameEvents
{
    public static Action OnTetrisRotate;
    public static Action OnTetrisDash;
    public static Action<Sprite> OnNextTetrisImage;

    public static Action OnGameStarted;
    public static Action OnGameOver;
    public static Action OnGameWin;

    public static Action<Participant> OnLivesChanged;
    public static Action<Participant> OnHeightChanged;

    public static void TetrisRotateInvoke()
    {
        OnTetrisRotate?.Invoke();
    }

    public static void TetrisDashInvoke()
    {
        OnTetrisDash?.Invoke();
    }

    public static void InvokeGameWin()
    {
        OnGameWin?.Invoke();
    }

    public static void InvokeGameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void InvokeGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public static void InvokeLivesChanged(Participant participant)
    {
        OnLivesChanged?.Invoke(participant);
    }

    public static void InvokeHeightChanged(Participant participant)
    {
        OnHeightChanged?.Invoke(participant);
    }

    public static void InvokeNextTetrisImage(Sprite s)
    {
        OnNextTetrisImage?.Invoke(s);
    }
}