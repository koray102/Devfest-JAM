using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffectMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;   // Speed of movement
    [SerializeField] private float moveRange = 20f;  // How far the fire moves

    private RectTransform rectTransform; // Reference to the RectTransform (because it's on the canvas)
    private bool moveInX; // Determines whether to move in the x-axis or y-axis

    void Start()
    {
        // Get the RectTransform component of the fire effect
        rectTransform = GetComponent<RectTransform>();

        // Randomly decide whether to move along the x-axis or y-axis
        moveInX = Random.value > 0.5f;
    }

    void Update()
    {
        // Calculate the new position based on a sine wave for smooth oscillation
        float movement = Mathf.Sin(Time.time * moveSpeed) * moveRange;

        if (moveInX)
        {
            // Apply movement along the x-axis
            rectTransform.anchoredPosition = new Vector2(movement, rectTransform.anchoredPosition.y);
        }
        else
        {
            // Apply movement along the y-axis
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, movement);
        }
    }
}
