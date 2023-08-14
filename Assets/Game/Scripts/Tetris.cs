using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tetris : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;

    [Tooltip("Collision Layers to validate that a piece was placed")]
    public LayerMask filterLayers;

    private Rigidbody rb;
    private bool canMove;
    public UnityEvent OnPiecePlaced;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!canMove || _gameConfig.isPaused) return;
        VerticalMove();
    }

    private void VerticalMove()
    {
        Vector3 movement = Vector3.down * (_gameConfig.PieceSpeed * Time.fixedDeltaTime);
        // Apply the movement to the Rigidbody in local space
        rb.MovePosition(rb.position + transform.TransformDirection(movement));
    }

    public void Rotate()
    {
        if (!canMove || _gameConfig.isPaused) return;
        transform.Rotate(new Vector3(0, 0, 90));
    }

    public void Dash()
    {
        if (!canMove || _gameConfig.isPaused) return;
        rb.AddForce(Vector3.down * _gameConfig.DashSpeed, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!canMove) return;
        LayerMask otherColliderLayer = collision.gameObject.layer;
        if (filterLayers.Contains(otherColliderLayer))
        {
            StopAllCoroutines();

            canMove = false;
            rb.drag = 0.5f;
            rb.useGravity = true;
            transform.parent = null;
            OnPiecePlaced.Invoke();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}