using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
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
    private int id;
    private Participant mySpawner;


    private void OnEnable()
    {
        GameEvents.OnTetrisRotate += Rotate;
        GameEvents.OnTetrisDash += Dash;
        canMove = true;

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            vfxClouds = GetComponentInChildren<ParticleSystem>();
        }

        vfxClouds.Play();
        rb.useGravity = false;
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

    public void Rotate(Participant p)
    {
        if (!canMove || _gameConfig.isPaused || p != mySpawner) return;
        transform.Rotate(new Vector3(0, 0, 90), Space.Self);
    }

    public void Dash(Participant p)
    {
        if (!canMove || _gameConfig.isPaused || p != mySpawner) return;
        rb.AddForce(Vector3.down * _gameConfig.DashSpeed, ForceMode.Impulse);
    }

    public void SetData(int myId, Participant spawner)
    {
        id = myId;
        mySpawner = spawner;
    }

    public int GetId()
    {
        return id;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canMove)
            return;

        LayerMask otherColliderLayer = collision.gameObject.layer;
        if (filterLayers.Contains(otherColliderLayer))
        {
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
                OnTetrisPlaced.Invoke();
            }

            OnTetrisFall.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}