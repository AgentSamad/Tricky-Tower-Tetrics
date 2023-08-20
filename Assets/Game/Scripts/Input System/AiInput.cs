using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "Ai Input", menuName = "Game/AI Input")]
public class AiInput : InputSystem
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float waitTime = 2f;
    private float timeSinceLastMove = 0f;
    private bool isMoving = false;
    private Vector3 targetPosition;

    public override void ControlTetris(Transform player, float snappingDistance, Participant p)
    {
        timeSinceLastMove += Time.deltaTime;

        if (!isMoving && timeSinceLastMove >= waitTime)
        {
            float randomX = Random.Range(bounds.x, bounds.y);
            targetPosition = new Vector3(randomX, player.position.y, player.position.z);
            isMoving = true;
        }

        if (isMoving)
        {
            float step = movementSpeed * Time.deltaTime;
            player.position = Vector3.MoveTowards(player.position, targetPosition, step);

            if (Vector3.Distance(player.position, targetPosition) <= snappingDistance)
            {
                isMoving = false;
                timeSinceLastMove = 0f;

                if (Random.value > 0.4f) GameEvents.TetrisRotateInvoke(p);
            }
        }
    }
}