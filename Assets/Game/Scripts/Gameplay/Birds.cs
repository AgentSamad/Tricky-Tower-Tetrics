using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Birds : MonoBehaviour
{
    public float startX;
    public float endX;
    public float duration; // Time in seconds for the X-axis movement
    public Vector2 yRange; // Range for the Y-coordinate

    private float startTime;
    private bool isMoving = false;
    private List<SpriteRenderer> birds = new List<SpriteRenderer>();

    void Start()
    {
        birds = transform.GetComponentsInChildren<SpriteRenderer>(true).ToList();
        StartMoving();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Calculate the current position based on time and lerp between start and end positions.
            float t = (Time.time - startTime) / duration;
            float newX = Mathf.Lerp(startX, endX, t);

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            if (t >= 1f)
            {
                isMoving = false;
                float newY = Random.Range(yRange.x, yRange.y);
                transform.position = new Vector3(startX, newY, transform.position.z);
                StartMoving();
            }
        }
    }

    void DisableAll()
    {
        foreach (var bird in birds)
        {
            bird.gameObject.SetActive(false);
        }
    }

    void RandomlyActiveBirds()
    {
        int index = Random.Range(0, birds.Count);
        birds[index].gameObject.SetActive(true);
    }

    void StartMoving()
    {
        startTime = Time.time;
        isMoving = true;
        DisableAll();
        RandomlyActiveBirds();
    }
}