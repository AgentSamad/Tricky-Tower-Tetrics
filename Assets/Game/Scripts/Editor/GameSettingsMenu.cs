using UnityEditor;
using UnityEngine;

public class GameSettingsMenu : MonoBehaviour
{
    [MenuItem("Game Settings/Settings")]
    public static void OpenGameConfig()
    {
        string gameConfigPath = "Assets/Game/Scripts/Scriptable Objects/Game Config/GameConfig.asset";
        GameConfig gameConfig = AssetDatabase.LoadAssetAtPath<GameConfig>(gameConfigPath);

        if (gameConfig != null)
        {
            Selection.activeObject = gameConfig;
            EditorGUIUtility.PingObject(gameConfig);
        }
        else
        {
            Debug.LogError("GameConfig not found. Make sure it's in a 'Resources' folder.");
        }
    }
}
