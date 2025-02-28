using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // Duraci�n de la vibraci�n
    public float shakeMagnitude = 0.1f;  // Magnitud de la vibraci�n
    private Vector3 originalPos;  // Posici�n original de la c�mara
    private float shakeTimeRemaining = 0f;

    void Start()
    {
        originalPos = transform.position;  // Guarda la posici�n inicial de la c�mara
        TriggerShake(shakeDuration, shakeMagnitude);  // Inicia la vibraci�n al activar la c�mara
    }

    private void OnEnable()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.position = originalPos;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = duration;
    }
}