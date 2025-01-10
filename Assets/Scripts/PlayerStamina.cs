using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{

    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaRecoveryRate = 1f;
    [SerializeField] private float staminaUseRate = 1f;

    [SerializeField] private PlayerMovement playerMovement;

    public float currentStamina;

    public bool hasStamina;

    private void Start()
    {
        currentStamina = maxStamina;
    }


    // Updates current stamina based on running state
    private void Update()
    {
        if (playerMovement.isRunning && playerMovement.moveDirection != Vector3.zero)
        {
            UseStamina();

            // Stops the player from running
            if (currentStamina <= 0) playerMovement.StopRunning();
        }
        else
        {
            RecoverStamina();
        }

        PlayerInterface.Instance.UpdateStaminaInterface(currentStamina, maxStamina);
    }

    // Uses stamina when running
    private void UseStamina()
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaUseRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);
        }
    }

    // Recovers stamina when not running
    private void RecoverStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }



}
