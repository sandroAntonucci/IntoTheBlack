using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepUpStairs : MonoBehaviour
{
    public float playerHeight = 2.0f;
    public float stairHeight = 0.5f;
    public float detectionDistance = 1.0f;
    public float climbSpeed = 0.1f;
    public LayerMask stairsLayer;

    private Transform playerTransform;
    private Vector3 heightOffset;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        heightOffset = new Vector3(0, playerHeight, 0);
    }

    void Update()
    {
        RaycastHit hit;
        Vector3 rayOrigin = playerTransform.position + heightOffset;
        Vector3 rayDirection = playerTransform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, detectionDistance, stairsLayer))
        {
            Vector3 climbPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + stairHeight, playerTransform.position.z);
            playerTransform.position = Vector3.Lerp(playerTransform.position, climbPosition + playerTransform.forward, climbSpeed);
        }
    }
}
