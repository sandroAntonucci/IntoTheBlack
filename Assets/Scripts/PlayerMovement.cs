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

    [SerializeField] private AudioManager walkSFX;

    private bool isCrouching = false;

    private float moveSpeed;

    public bool isRunning;

    private float horizontalInput;
    private float verticalInput;

    public Vector3 moveDirection;

    private Rigidbody rb;

    [SerializeField] private float maxSlopeAngle = 45f;
    private RaycastHit hitInfo;

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
        controls.Player.Move.performed += ctx =>
        {
            MyInput(ctx);
        };

        controls.Player.Move.canceled += ctx =>
        {
            MyInput(ctx);
        };


        controls.Player.Crouch.performed += ctx => Crouch();
        controls.Player.Crouch.canceled += ctx => StandUp();
        controls.Player.Run.started += ctx => StartRunning();
        controls.Player.Run.canceled += ctx =>
        {
            isRunning = false;
            StopRunning();
        };
    }

    // Enables Controls
    private void OnEnable()
    {
        controls.Enable();
        walkSFX.StartRandomSound();
    }

    // Disables Controls    
    private void OnDisable()
    {
        controls.Disable();
        walkSFX.StopRandomSound();
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
        CrouchControl();
    }

    // Manages crouching
    private void CrouchControl()
    {
        if (isCrouching)
        {
            playerModel.transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - (playerHeight - crouchHeight) / 2, transform.position.z);
            playerModel.transform.position = Vector3.MoveTowards(playerModel.transform.position, targetPosition, 6f * Time.deltaTime);
        }

        else if (!isCrouching && playerModel.transform.localScale.y != playerHeight)
        {
            playerModel.transform.localScale = new Vector3(transform.localScale.x, playerHeight, transform.localScale.z);
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + (playerHeight - crouchHeight) / 2, transform.position.z);
            playerModel.transform.position = Vector3.MoveTowards(playerModel.transform.position, targetPosition, 6f * Time.deltaTime);
        }
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
        // Calculamos la dirección de movimiento según la entrada del jugador
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Si la pendiente es suave, agregamos un pequeño impulso hacia arriba
        if (Vector3.Angle(hitInfo.normal, Vector3.up) < maxSlopeAngle)
        {
            // Aseguramos que el jugador se mueve hacia arriba un poco
            Vector3 verticalPush = hitInfo.normal * (verticalInput > 0 ? 0.5f : 0);  // Ajustar el valor según sea necesario

            // Aplicamos el movimiento con la dirección de la pendiente y el pequeño empuje vertical
            rb.AddForce((moveDirection + verticalPush).normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
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

        walkSFX.velocityMultiplier = rb.velocity.magnitude / 4;

    }

    // Crouches the player
    private void Crouch()
    {
        if (isCrouching) return;

        isCrouching = true;

    }

    // Stands the player up
    private void StandUp()
    {
        if (!isCrouching) return;

        isCrouching = false;
    }

    private void StartRunning()
    {
        isRunning = true;
        baseMoveSpeed = moveSpeed;
        moveSpeed = runningSpeed;
    }

    public void StopRunning()
    {
        moveSpeed = baseMoveSpeed;
    }

}
