using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // Duración de la vibración
    public float shakeMagnitude = 0.1f;  // Magnitud de la vibración
    private Vector3 originalPos;  // Posición original de la cámara
    private float shakeTimeRemaining = 0f;

    // Start se ejecuta una vez cuando se inicia el script
    void Start()
    {
        originalPos = transform.position;  // Guarda la posición inicial de la cámara
        TriggerShake(shakeDuration, shakeMagnitude);  // Inicia la vibración al activar la cámara
    }

    // Update se ejecuta una vez por frame
    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            // Genera un desplazamiento aleatorio para simular la vibración
            transform.position = originalPos + Random.insideUnitSphere * shakeMagnitude;

            // Reduce el tiempo restante de vibración
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Restaura la posición de la cámara a su valor original cuando la vibración termina
            transform.position = originalPos;
        }
    }

    // Método para iniciar la vibración de la cámara
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = duration;
    }
}