using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Tetris : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;

    public LayerMask filterLayers;

    private Rigidbody rb;
    private bool canMove;

    private ParticleSystem vfxClouds;
    public UnityEvent OnTetrisPlaced;
    public UnityEvent OnTetrisFall;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vfxClouds = GetComponentInChildren<ParticleSystem>();
        rb.useGravity = false;
        canMove = true;

        vfxClouds.Play();
        GameEvents.OnTetrisRotate += Rotate;
        GameEvents.OnTetrisDash += Dash;
    }

    private void OnDisable()
    {
        GameEvents.OnTetrisRotate -= Rotate;
        GameEvents.OnTetrisDash -= Dash;

        OnTetrisPlaced.RemoveAllListeners();
        OnTetrisFall.RemoveAllListeners();
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
        rb.MovePosition(rb.position + movement);
    }

    public void Rotate()
    {
        if (!canMove || _gameConfig.isPaused) return;
        transform.Rotate(new Vector3(0, 0, 90), Space.Self);
    }

    public void Dash()
    {
        if (!canMove || _gameConfig.isPaused) return;
        rb.AddForce(Vector3.down * _gameConfig.DashSpeed, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!canMove)
            return;

        LayerMask otherColliderLayer = collision.gameObject.layer;
        if (filterLayers.Contains(otherColliderLayer))
        {
            StopAllCoroutines();

            canMove = false;
            rb.drag = 0.5f;
            rb.useGravity = true;
            transform.parent = null;
            OnTetrisPlaced.Invoke();
            vfxClouds.Play();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            if (canMove)
            {
                OnTetrisPlaced?.Invoke();
            }
            else
            {
                OnTetrisFall.Invoke();
            }


            Destroy(this.gameObject);
        }
    }
}