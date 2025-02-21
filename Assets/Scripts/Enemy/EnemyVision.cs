using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Transform player;  // El objeto jugador
    public float visionRange = 10f;  // Rango de visi�n del enemigo
    public float visionAngle = 90f;  // �ngulo de visi�n del enemigo
    public LayerMask obstacleLayer;  // Capa para detectar obst�culos (muros, etc.)
    private bool playerInSight = false;  // Indica si el jugador est� dentro del campo de visi�n

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
        // Direcci�n desde el enemigo hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;  // Distancia entre el enemigo y el jugador

        // Verifica si el jugador est� dentro del rango de visi�n
        if (distanceToPlayer <= visionRange)
        {
            // Calcula el �ngulo entre la direcci�n de visi�n del enemigo y la direcci�n al jugador
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Si el jugador est� dentro del �ngulo de visi�n
            if (angleToPlayer <= visionAngle / 2f)
            {
                // Raycast para verificar si hay obst�culos entre el enemigo y el jugador
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

                        // El jugador est� en el campo de visi�n y no hay obst�culos en el camino
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
        // Visualiza el campo de visi�n del enemigo en la escena
        Gizmos.color = playerInSight ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Dibuja el �rea de visi�n del enemigo (cono de visi�n)
        Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 2f, 0) * transform.forward * visionRange;
        Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 2f, 0) * transform.forward * visionRange;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // Dibuja el raycast hacia el jugador si est� dentro del campo de visi�n
        if (playerInSight)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}

