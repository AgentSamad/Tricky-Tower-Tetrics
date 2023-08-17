using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig")]
public class GameConfig : ScriptableObject
{
    public enum GameMode
    {
        Solo,
        Multiplayer
    }

    public GameMode gameMode = GameMode.Solo;
    public int initialLives = 3;
    public float PieceSpeed = 4;
    public float DashSpeed = 3;
    public float maxHeightToWin = 40;
    public bool isPaused;
}