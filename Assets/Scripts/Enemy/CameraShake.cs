using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // Duraci�n de la vibraci�n
    public float shakeMagnitude = 0.1f;  // Magnitud de la vibraci�n
    private Vector3 originalPos;  // Posici�n original de la c�mara
    private float shakeTimeRemaining = 0f;

    // Start se ejecuta una vez cuando se inicia el script
    void Start()
    {
        originalPos = transform.position;  // Guarda la posici�n inicial de la c�mara
        TriggerShake(shakeDuration, shakeMagnitude);  // Inicia la vibraci�n al activar la c�mara
    }

    // Update se ejecuta una vez por frame
    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            // Genera un desplazamiento aleatorio para simular la vibraci�n
            transform.position = originalPos + Random.insideUnitSphere * shakeMagnitude;

            // Reduce el tiempo restante de vibraci�n
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Restaura la posici�n de la c�mara a su valor original cuando la vibraci�n termina
            transform.position = originalPos;
        }
    }

    // M�todo para iniciar la vibraci�n de la c�mara
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = duration;
    }
}