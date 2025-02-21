using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Transform player;  // El objeto jugador
    public float visionRange = 10f;  // Rango de visión del enemigo
    public float visionAngle = 90f;  // Ángulo de visión del enemigo
    public LayerMask obstacleLayer;  // Capa para detectar obstáculos (muros, etc.)
    private bool playerInSight = false;  // Indica si el jugador está dentro del campo de visión

    public bool PlayerInSight { get => playerInSight; }

    public Coroutine CoroutineStopFollowing { get; set; }

    void Update()
    {
        DetectPlayer();
    }

    private IEnumerator StopFollowingPlayer()
    {
        Debug.Log("Stop following player");
        yield return new WaitForSeconds(5f);
        playerInSight = false;
    }

    void DetectPlayer()
    {
        // Dirección desde el enemigo hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;  // Distancia entre el enemigo y el jugador

        // Verifica si el jugador está dentro del rango de visión
        if (distanceToPlayer <= visionRange)
        {
            // Calcula el ángulo entre la dirección de visión del enemigo y la dirección al jugador
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Si el jugador está dentro del ángulo de visión
            if (angleToPlayer <= visionAngle / 2f)
            {
                // Raycast para verificar si hay obstáculos entre el enemigo y el jugador
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, visionRange, ~obstacleLayer))
                {
                    if (hit.transform == player)
                    {

                        if (CoroutineStopFollowing != null)
                        {
                            StopCoroutine(CoroutineStopFollowing);
                            CoroutineStopFollowing = null;
                        }

                        // El jugador está en el campo de visión y no hay obstáculos en el camino
                        playerInSight = true;
                    }
                    else
                    {
                        if(playerInSight && CoroutineStopFollowing == null) CoroutineStopFollowing = StartCoroutine(StopFollowingPlayer());
                    }
                }
            }
            else
            {
                if (playerInSight && CoroutineStopFollowing == null) CoroutineStopFollowing = StartCoroutine(StopFollowingPlayer());
            }
        }
        else
        {
            if (playerInSight && CoroutineStopFollowing == null) CoroutineStopFollowing = StartCoroutine(StopFollowingPlayer());
        }
    }

    void OnDrawGizmos()
    {
        // Visualiza el campo de visión del enemigo en la escena
        Gizmos.color = playerInSight ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Dibuja el área de visión del enemigo (cono de visión)
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward * visionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward * visionRange;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // Dibuja el raycast hacia el jugador si está dentro del campo de visión
        if (playerInSight)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}

