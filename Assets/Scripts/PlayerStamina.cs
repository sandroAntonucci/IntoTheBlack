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

    public bool isRunning = false;

    private void Start()
    {
        currentStamina = maxStamina;
    }

    // Called when the player starts running
    public void StartConsumingStamina()
    {
        isRunning = true;
    }

    // Called when the player stops running
    public void StopConsumingStamina()
    {
        isRunning = false;
    }

    // Updates current stamina based on running state
    private void Update()
    {
        Debug.Log("Current Stamina: " + currentStamina);

        if (isRunning)
        {
            UseStamina();

            // Stops the player from running
            if (currentStamina <= 0) playerMovement.StopRunning();
        }
        else
        {
            RecoverStamina();
        }
    }

    // Uses stamina when running
    private void UseStamina()
    {
        if (currentStamina > 0)
        {
            currentStamina -= staminaUseRate * Time.deltaTime;
        }
    }

    // Recovers stamina when not running
    private void RecoverStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }
    }



}
