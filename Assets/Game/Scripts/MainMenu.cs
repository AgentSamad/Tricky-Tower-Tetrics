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
    }

    private void OnVersusModePressed()
    {
        StartCoroutine(LoadingSceneAsync("Multiplayer"));
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