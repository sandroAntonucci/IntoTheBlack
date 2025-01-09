using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Movement variables
    [SerializeField] private float moveSpeed;

    // Crouch variables
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 4f;

    [SerializeField] private GameObject playerModel;

    [SerializeField] private Transform orientation;

    [SerializeField] private PlayerControls controls;

    private bool isCrouching = false;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;


    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Bind input actions
        controls.Player.Move.performed += ctx => MyInput(ctx);
        controls.Player.Move.canceled += ctx => MyInput(ctx);
        controls.Player.Crouch.performed += ctx => Crouch();
        controls.Player.Crouch.canceled += ctx => StandUp();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Update()
    {

        SpeedControl();

    }

    // This function handles the input mapping
    private void MyInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>(); // Read the 2D vector from the input action
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float maxSpeed = isCrouching ? crouchSpeed : moveSpeed;

        if (flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Crouch()
    {
        if (isCrouching) return;

        isCrouching = true;

        // Reduce the player's height
        playerModel.transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);

        // Lower the player's position slightly
        playerModel.transform.position = new Vector3(transform.position.x, transform.position.y - (playerHeight - crouchHeight) / 2, transform.position.z);
    }

    private void StandUp()
    {
        if (!isCrouching) return;

        isCrouching = false;

        // Restore the player's height
        playerModel.transform.localScale = new Vector3(transform.localScale.x, playerHeight, transform.localScale.z);

        // Raise the player's position slightly
        playerModel.transform.position = new Vector3(transform.position.x, transform.position.y + (playerHeight - crouchHeight) / 2, transform.position.z);
    }

}
