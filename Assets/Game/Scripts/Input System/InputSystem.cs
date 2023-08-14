using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : ScriptableObject
{
    public Vector2 bounds;


    public virtual void Init()
    {
    }

    public virtual void ControlTetris(Transform player, float snappingDistance)
    {
    }
}