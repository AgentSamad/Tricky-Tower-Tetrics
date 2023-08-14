using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class TetrisSpawner : MonoBehaviour
{
    [SerializeField] private List<TetrisData> tetrisBlocks = new List<TetrisData>();

    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private float spawnDelay = 1;
    private TetrisData NextPieceData { get; set; }
    private bool canSpawn;


    //Actions
    public static Action<Transform> SpawnTetris;

    private void Awake()
    {
        SpawnTetris += SpawnPiece;
        GameEvents.OnGameOver += GameOver;
        canSpawn = true;
        NextPieceData = GetRandomData();
    }

    private void OnDisable()
    {
        SpawnTetris -= SpawnPiece;
        GameEvents.OnGameOver -= GameOver;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private TetrisData GetRandomData()
    {
        var sortedIndex = Random.Range(0, tetrisBlocks.Count);
        var data = tetrisBlocks[sortedIndex];
        // nextPieceSprite.value = data.icon;
        // OnPieceSpawned.Raise();
        return data;
    }

    private void GameOver()
    {
        canSpawn = false;
    }

    public void SpawnPiece(Transform spawnParent)
    {
        if (NextPieceData == null || !canSpawn || _gameConfig.isPaused) return;
        StartCoroutine(Spawn(spawnParent));
    }


    public void TetrisFall()
    {
        Debug.Log("Tetris Fall");
        GameEvents.InvokeLivesChanged();
    }

    private IEnumerator Spawn(Transform spawnParent)
    {
        //TODO: Play spawn VFX/particles
        yield return new WaitForSeconds(spawnDelay);
        var currentPieceData = NextPieceData;
        Tetris pieceController = Instantiate(currentPieceData.prefab, spawnParent).GetComponent<Tetris>();

        pieceController.OnTetrisPlaced.AddListener(() => { SpawnPiece(spawnParent); });
        pieceController.OnTetrisFall.AddListener(TetrisFall);
        //     .GetComponent<PieceController>();
        // ActivePieces.AddNewPiece(pieceController);
        NextPieceData = GetRandomData();
    }
}