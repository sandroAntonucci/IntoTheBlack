using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float crouchSpeed = 2.5f;
    [SerializeField] private float crouchHeight = 0.5f;

    private float normalHeight = 1f;
    private Camera mainCamera;

    private InputAction moveAction;
    private InputAction crouchAction;
    private InputAction lookAction;
    private InputAction runAction;

    private CharacterController characterController;

    private float currentSpeed;
    private bool isCrouching = false;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private void OnEnable()
    {
        BindActions();

        moveAction.Enable();
        crouchAction.Enable();
        lookAction.Enable();
        runAction.Enable();

        crouchAction.performed += OnCrouch;
        runAction.performed += OnRun;
        runAction.canceled += OnRun;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        crouchAction.Disable();
        lookAction.Disable();
        runAction.Disable();

        crouchAction.performed -= OnCrouch;
        runAction.performed -= OnRun;
        runAction.canceled -= OnRun;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void BindActions()
    {
        moveAction = playerControls.FindActionMap("Player").FindAction("Move");
        crouchAction = playerControls.FindActionMap("Player").FindAction("Crouch");
        lookAction = playerControls.FindActionMap("Player").FindAction("Look");
        runAction = playerControls.FindActionMap("Player").FindAction("Run");
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        lookInput = lookAction.ReadValue<Vector2>();
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;

        transform.Rotate(Vector3.up * mouseX);
        mainCamera.transform.localRotation *= Quaternion.Euler(-mouseY, 0f, 0f);
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        if (!isCrouching)
        {
            currentSpeed = context.ReadValueAsButton() ? runSpeed : moveSpeed;
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (isCrouching)
        {
            StandUp();
        }
        else
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouching = true;
        characterController.height = crouchHeight;
        currentSpeed = crouchSpeed;
    }

    private void StandUp()
    {
        isCrouching = false;
        characterController.height = normalHeight;
        currentSpeed = moveSpeed;
    }
}
