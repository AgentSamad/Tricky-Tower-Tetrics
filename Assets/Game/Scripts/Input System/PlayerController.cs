using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputSystem input;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float snappingDistance = 0.1f;

    private void OnEnable()
    {
        TetrisSpawner.SpawnTetris?.Invoke(spawnPoint);
      if(TetrisSpawner.SpawnTetris== null)
          print("Its Null");
    }

    private void Update()
    {
        input.ControlTetris(snappingDistance);
    }
}