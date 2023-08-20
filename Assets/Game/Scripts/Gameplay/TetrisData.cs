using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TetrisData", menuName = "Tetris/Data")]
public class TetrisData : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
    public int id;
}