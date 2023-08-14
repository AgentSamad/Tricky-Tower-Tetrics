using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputSystem input;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float snappingDistance = 0.1f;

    private void Start()
    {
        TetrisSpawner.SpawnTetris?.Invoke(spawnPoint);
        if (TetrisSpawner.SpawnTetris == null)
            print("Its Null");

        input.Init();
        
        print("I am enabled");
    }

    private void Update()
    {
        input.ControlTetris(this.transform, snappingDistance);
    }
}