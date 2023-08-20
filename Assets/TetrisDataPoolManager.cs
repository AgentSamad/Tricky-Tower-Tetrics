using System.Collections.Generic;
using UnityEngine;

public class TetrisDataPoolManager : MonoBehaviour
{
    [SerializeField] private TetrisData[] tetrisDataPrefabs; // Array to store your 7 TetrisData prefabs as GameObjects.
    [SerializeField] private int initialPoolSize = 10; // Initial pool size for each type of TetrisData.

    private Dictionary<string, Queue<GameObject>> tetrisDataPool = new Dictionary<string, Queue<GameObject>>();

    private void Start()
    {
        // Initialize the pool for each type of TetrisData.
        foreach (var tetrisDataPrefab in tetrisDataPrefabs)
        {
            CreateTetrisDataPool(tetrisDataPrefab.prefab);
        }
    }

    private void CreateTetrisDataPool(GameObject prefab)
    {
        string key = prefab.name; // Use the prefab's name as a key.

        if (!tetrisDataPool.ContainsKey(key))
        {
            tetrisDataPool[key] = new Queue<GameObject>();
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject newTetrisData = Instantiate(prefab, this.transform);
            newTetrisData.SetActive(false);
            tetrisDataPool[key].Enqueue(newTetrisData);
        }
    }

    public GameObject GetTetrisData(string key)
    {
        if (tetrisDataPool.ContainsKey(key) && tetrisDataPool[key].Count > 0)
        {
            GameObject tetrisData = tetrisDataPool[key].Dequeue();
            tetrisData.SetActive(true);
            return tetrisData;
        }

        return null; // No available objects in the pool.
    }


    public void ReturnTetrisDataToPool(string key, GameObject tetris)
    {
        tetris.transform.SetParent(this.transform);
        tetris.SetActive(false);
        tetrisDataPool[key].Enqueue(tetris);
    }
}