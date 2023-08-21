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
    [SerializeField] private int initialPoolSize;
    private Dictionary<int, Queue<GameObject>> tetrisDataPool = new Dictionary<int, Queue<GameObject>>();
    private TetrisData NextPieceData { get; set; }
    private bool canSpawn;

    //Actions
    public static Action<Transform, Participant> SpawnTetris;

    public static List<Tetris> playerActiveTetris { get; } = new List<Tetris>();
    public static List<Tetris> aiActiveTetris { get; } = new List<Tetris>();

    private void Awake()
    {
        SpawnTetris += SpawnPiece;
        GameEvents.OnGameOver += StopSpawning;
        GameEvents.OnGameWin += StopSpawning;

        // Initialize the pool for each type of TetrisData.
        InitializePool();

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

    #region Spawning

    private void StopSpawning()
    {
        canSpawn = false;
    }

    private void SpawnPiece(Transform spawnParent, Participant participant)
    {
        if (NextPieceData == null || !canSpawn || _gameConfig.isPaused) return;
        StartCoroutine(Spawn(spawnParent, participant));
    }

    private IEnumerator Spawn(Transform spawnParent, Participant participant)
    {
        //TODO: Dosomething

        yield return new WaitForSeconds(spawnDelay);
        var currentPieceData = NextPieceData;
        Tetris tetris = GetTetrisData(currentPieceData.id, spawnParent).GetComponent<Tetris>();

        //SetData
        tetris.SetData(currentPieceData.id, participant);
        tetris.OnTetrisPlaced.AddListener(() => OnTetrisPlacedHandler(spawnParent, participant));
        tetris.OnTetrisFall.AddListener(() => OnTetrisFallHandler(participant, tetris));
        AddTetris(participant, tetris);
        NextPieceData = GetRandomData();
    }

    #endregion

    #region Events

    public void OnTetrisFallHandler(Participant participant, Tetris tetris)
    {
        GameEvents.InvokeLivesChanged(participant);
        RemoveTetris(participant, tetris);
        ReturnTetrisDataToPool(tetris.GetId(), tetris.gameObject);
    }

    public void OnTetrisPlacedHandler(Transform spawnParent, Participant participant)
    {
        SpawnPiece(spawnParent, participant);

        //calculate height
        GameEvents.InvokeHeightChanged(participant);
    }

    #endregion


    #region Pooling

    private void InitializePool()
    {
        foreach (var tetrisDataPrefab in tetrisBlocks)
        {
            CreateTetrisDataPool(tetrisDataPrefab);
        }
    }

    private void CreateTetrisDataPool(TetrisData tetrisData)
    {
        int key = tetrisData.id;
        string name = tetrisData.prefab.name;

        if (!tetrisDataPool.ContainsKey(key))
        {
            tetrisDataPool[key] = new Queue<GameObject>();
        }

        //Fly Weight
        var runtimeObject = Instantiate(tetrisData.prefab, this.transform);
        runtimeObject.name = $"{name} {1}";

        //clone objects

        for (int i = 0; i < initialPoolSize - 1; i++)
        {
            runtimeObject = Instantiate(runtimeObject, this.transform);
            runtimeObject.name = $"{name} {i + 2}";
            tetrisDataPool[key].Enqueue(runtimeObject);
        }
    }

    public GameObject GetTetrisData(int key, Transform newParent)
    {
        if (tetrisDataPool.ContainsKey(key) && tetrisDataPool[key].Count > 0)
        {
            GameObject tetris = tetrisDataPool[key].Dequeue();
            tetris.transform.SetParent(newParent, false);
            tetris.SetActive(true);
            return tetris;
        }


        return null; // No available objects in the pool.
    }


    public void ReturnTetrisDataToPool(int key, GameObject tetris)
    {
        tetris.transform.SetParent(this.transform, false);
        tetrisDataPool[key].Enqueue(tetris);
    }

    #endregion
}