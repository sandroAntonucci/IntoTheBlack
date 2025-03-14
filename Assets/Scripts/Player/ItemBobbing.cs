using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemBobbing : MonoBehaviour
{
    public Transform itemPosition;
    public Transform player; // Reference to the player object

    public float bobFrequency = 3f; // Speed of the bobbing effect
    public float bobAmplitude = 0.05f; // Height of the bobbing effect

    float bobOffset = 0f;

    private float timer = 0f;
    private Vector3 originalPosition;

    private bool itemAtLowestPoint = false;
    public static event Action ItemAtLowestPoint;

    private void Start()
    {
        originalPosition = transform.localPosition; // Store the original local position of the camera
    }

    private void Update()
    {
        HandleItemBobbing();
        transform.position = new Vector3(itemPosition.position.x, itemPosition.position.y + bobOffset, itemPosition.position.z);
    }

    private void HandleItemBobbing()
    {
        if (player == null) return;

        // Get player's movement speed (assumes Rigidbody movement)
        float speed = player.GetComponent<Rigidbody>().velocity.magnitude;

        if (speed > 0.1f) // Only bob if the player is moving
        {
            timer += Time.deltaTime * bobFrequency * speed; // Scale bobbing with speed
            bobOffset = Mathf.Sin(timer) * bobAmplitude;
        }
        else
        {
            timer = 0; // Reset when stopped to avoid jerky behavior
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
        }

        if (bobOffset <= -bobAmplitude + 0.01f && !itemAtLowestPoint) // Small tolerance to account for float precision
        {
            itemAtLowestPoint = true;
            ItemAtLowestPoint?.Invoke();
        }
        else if (bobOffset >= -bobAmplitude + 0.05f && itemAtLowestPoint)
        {
            itemAtLowestPoint = false;
        }

    }
}
