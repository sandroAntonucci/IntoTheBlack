using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    // Movement variables
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float runningSpeed;

    // Crouch variables
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 4f;

    [SerializeField] private GameObject playerModel;

    [SerializeField] private Transform orientation;

    [SerializeField] private PlayerControls controls;

    [SerializeField] private PlayerStamina playerStamina;

    private bool isCrouching = false;

    private float moveSpeed;

    private float horizontalInput;
    private float verticalInput;

    private bool canRun;

    private Vector3 moveDirection;

    private Rigidbody rb;


    // Creates a new instance of the PlayerControls class
    private void Awake()
    {
        controls = new PlayerControls();
    }

    // Assigns variables and binds input actions
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = baseMoveSpeed;

        // Bind input actions
        controls.Player.Move.performed += ctx => MyInput(ctx);
        controls.Player.Move.canceled += ctx => MyInput(ctx);
        controls.Player.Crouch.performed += ctx => Crouch();
        controls.Player.Crouch.canceled += ctx => StandUp();
        controls.Player.Run.started += ctx => StartRunning();
        controls.Player.Run.canceled += ctx => StopRunning();
    }

    // Enables Controls
    private void OnEnable()
    {
        controls.Enable();
    }

    // Disables Controls    
    private void OnDisable()
    {
        controls.Disable();
    }

    // Moves the player (set to fixed update so the player cant run faster on faster computers)
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Updates the player's speed (set to update so the player inputs aren't affected by fixed frames)
    private void Update()
    {
        SpeedControl();
    }

    // Changes direction based on the inputs from the player
    private void MyInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>(); // Read the 2D vector from the input action
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    // Moves the player based on the inputs from the player
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;

        rb.AddForce(moveDirection.normalized * currentSpeed * 10f, ForceMode.Force);
    }

    // Limits the player's speed
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

    // Crouches the player
    private void Crouch()
    {
        if (isCrouching) return;

        isCrouching = true;

        // Reduce the player's height
        playerModel.transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);

        // Lower the player's position slightly
        playerModel.transform.position = new Vector3(transform.position.x, transform.position.y - (playerHeight - crouchHeight) / 2, transform.position.z);
    }

    // Stands the player up
    private void StandUp()
    {
        if (!isCrouching) return;

        isCrouching = false;

        // Restore the player's height
        playerModel.transform.localScale = new Vector3(transform.localScale.x, playerHeight, transform.localScale.z);

        // Raise the player's position slightly
        playerModel.transform.position = new Vector3(transform.position.x, transform.position.y + (playerHeight - crouchHeight) / 2, transform.position.z);
    }

    private void StartRunning()
    {
        playerStamina.StartConsumingStamina();
        baseMoveSpeed = moveSpeed;
        moveSpeed = runningSpeed;
    }

    public void StopRunning()
    {
        playerStamina.StopConsumingStamina();
        moveSpeed = baseMoveSpeed;
    }

}
