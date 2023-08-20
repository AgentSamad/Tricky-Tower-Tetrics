using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameConfig))]
public class GameConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameConfig gameConfig = (GameConfig)target;

        EditorGUILayout.LabelField("Game Settings", EditorStyles.boldLabel);

        // Display and edit the GameMode enum.
        // gameConfig.gameMode = (GameConfig.GameMode)EditorGUILayout.EnumPopup("Game Mode", gameConfig.gameMode);

        // Edit other fields as needed.
        gameConfig.initialLives = EditorGUILayout.IntSlider("Initial Lives", gameConfig.initialLives, 1, 5);
        gameConfig.PieceSpeed = EditorGUILayout.Slider("Piece Speed", gameConfig.PieceSpeed, 1f, 10f);
        gameConfig.PieceRotateSpeed = EditorGUILayout.Slider("Piece Rotate Speed", gameConfig.PieceRotateSpeed, 20f, 60f);
        gameConfig.DashSpeed = EditorGUILayout.Slider("Dash Speed", gameConfig.DashSpeed, 1f, 10f);
        gameConfig.maxHeightToWin = EditorGUILayout.Slider("Max Height to Win", gameConfig.maxHeightToWin, 10f, 50f);

        // Apply any changes to the ScriptableObject.
        if (GUI.changed)
        {
            EditorUtility.SetDirty(gameConfig);
        }
    }
}