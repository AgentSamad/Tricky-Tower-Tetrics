using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private GameConfig _gameConfig;

    private void OnEnable()
    {
        singlePlayerButton.onClick.AddListener(OnSoloModePressed);
        multiplayerButton.onClick.AddListener(OnVersusModePressed);
    }

    private void OnDisable()
    {
        singlePlayerButton.onClick.RemoveAllListeners();
        multiplayerButton.onClick.RemoveAllListeners();
    }

    private void OnSoloModePressed()
    {
        StartCoroutine(LoadingSceneAsync("Soloplayer"));
        _gameConfig.gameMode = GameConfig.GameMode.Solo;
    }

    private void OnVersusModePressed()
    {
        StartCoroutine(LoadingSceneAsync("Multiplayer"));
        _gameConfig.gameMode = GameConfig.GameMode.Multiplayer;
    }


    private IEnumerator LoadingSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}