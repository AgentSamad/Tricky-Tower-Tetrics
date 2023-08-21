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
    private bool isRotating;
    private Quaternion targetRotation;

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
        ResetRigidBody();
        targetRotation = transform.rotation;
    }

    private void OnDisable()
    {
        GameEvents.OnTetrisRotate -= Rotate;
        GameEvents.OnTetrisDash -= Dash;

        OnTetrisPlaced.RemoveAllListeners();
        OnTetrisFall.RemoveAllListeners();
    }


    private void Update()
    {
        if (isRotating)
        {
            Quaternion startRotation = transform.rotation;
            float step = _gameConfig.PieceRotateSpeed * Time.deltaTime; // Adjust speed for 90-degree rotation
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, step);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                // Stop rotating when close to target rotation
                isRotating = false;
            }
        }
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
        if (!canMove || _gameConfig.isPaused || p != mySpawner || isRotating) return;
        {
            isRotating = true;
            targetRotation *= Quaternion.Euler(0, 0, 90);

            if (mySpawner == Participant.player)
                Vibration.VibratePop();
        }
        //transform.Rotate(new Vector3(0, 0, 90), Space.Self);
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
            rb.drag = 1f;
            rb.angularDrag = 0.5f;
            rb.useGravity = true;
            transform.parent = null;
            OnTetrisPlaced.Invoke();
            vfxClouds.Play();
            if (mySpawner == Participant.player)
                Vibration.VibrateLight();
        }
    }


    void ResetRigidBody()
    {
        rb.drag = 0f;
        rb.angularDrag = 0;
        rb.useGravity = false;
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

            if (mySpawner == Participant.player)
                Vibration.VibrateHeavy();

            this.gameObject.SetActive(false);
        }
    }
}