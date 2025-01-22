using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // Duración de la vibración
    public float shakeMagnitude = 0.1f;  // Magnitud de la vibración
    private Vector3 originalPos;  // Posición original de la cámara
    private float shakeTimeRemaining = 0f;

    void Start()
    {
        originalPos = transform.position;  // Guarda la posición inicial de la cámara
        TriggerShake(shakeDuration, shakeMagnitude);  // Inicia la vibración al activar la cámara
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