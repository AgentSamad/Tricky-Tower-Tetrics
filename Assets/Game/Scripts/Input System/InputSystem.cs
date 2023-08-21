using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public abstract class InputSystem : ScriptableObject
{
    public Vector2 bounds;
    public float snappingDistance;

    public virtual void Init()
    {
    }

    public virtual void ControlTetris(Transform player, Participant p)
    {
    }
}