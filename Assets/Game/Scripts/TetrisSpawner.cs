using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrisData> tetrisBlocks = new List<TetrisData>();

    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private float spawnDelay = 0f;
    private TetrisData NextPieceData { get; set; }
    private bool canSpawn;

    //Actions
    public static Action<Transform, Participant> SpawnTetris;

    public static List<Tetris> playerActiveTetris = new List<Tetris>();
    public static List<Tetris> aiActiveTetris = new List<Tetris>();

    private void Awake()
    {
        SpawnTetris += SpawnPiece;
        GameEvents.OnGameOver += StopSpawning;
        GameEvents.OnGameWin += StopSpawning;
        canSpawn = true;
        NextPieceData = GetRandomData();
    }

    private void OnDisable()
    {
        SpawnTetris -= SpawnPiece;
        GameEvents.OnGameOver -= StopSpawning;
        GameEvents.OnGameWin -= StopSpawning;
    }


    private TetrisData GetRandomData()
    {
        var sortedIndex = Random.Range(0, tetrisBlocks.Count);
        var data = tetrisBlocks[sortedIndex];
        GameEvents.InvokeNextTetrisImage(data.icon);
        return data;
    }

    private void AddTetris(Participant participant, Tetris tetris)
    {
        switch (participant)
        {
            case Participant.player:
                playerActiveTetris.Add(tetris);
                break;
            case Participant.ai:
                aiActiveTetris.Add(tetris);
                break;
        }
    }

    private void RemoveTetris(Participant participant, Tetris tetris)
    {
        switch (participant)
        {
            case Participant.player:
                playerActiveTetris.Remove(tetris);
                playerActiveTetris.TrimExcess();
                break;
            case Participant.ai:
                aiActiveTetris.Remove(tetris);
                aiActiveTetris.TrimExcess();
                break;
        }
    }


    private void StopSpawning()
    {
        canSpawn = false;
    }

    private void SpawnPiece(Transform spawnParent, Participant participant)
    {
        if (NextPieceData == null || !canSpawn || _gameConfig.isPaused) return;
        StartCoroutine(Spawn(spawnParent, participant));
    }


    public void OnTetrisFallHandler(Participant participant, Tetris tetris)
    {
        GameEvents.InvokeLivesChanged(participant);
        RemoveTetris(participant, tetris);
    }

    public void OnTetrisPlacedHandler(Transform spawnParent, Participant participant)
    {
        SpawnPiece(spawnParent, participant);
        
        //calculate height
        GameEvents.InvokeHeightChanged(participant);
    }

    private IEnumerator Spawn(Transform spawnParent, Participant participant)
    {
        //TODO: Dosomething

        yield return new WaitForSeconds(spawnDelay);
        var currentPieceData = NextPieceData;
        Tetris tetris = Instantiate(currentPieceData.prefab, spawnParent).GetComponent<Tetris>();
        tetris.OnTetrisPlaced.AddListener(() => OnTetrisPlacedHandler(spawnParent, participant));
        tetris.OnTetrisFall.AddListener(() => OnTetrisFallHandler(participant, tetris));
        AddTetris(participant, tetris);
        NextPieceData = GetRandomData();
    }
}