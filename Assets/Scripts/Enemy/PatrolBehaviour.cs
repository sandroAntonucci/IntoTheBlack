using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MonoBehaviour
{
    private const string ParamWalk = "Walk", ParamRun = "Run";

    private const int doubleValue = 2;
    private const float minRemainingDistance = 0.5f;

    public float speed = 1;     
    public float waitTime = 2f; 
    public GameObject target;
    public List<Transform> waypoints;

    private Animator animator;
    private NavMeshAgent agent;
    private EnemyVision vision;
    private bool isWaiting = false;

    public bool IsWaiting { get => isWaiting; }

    private void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(agent.transform.position);
        agent.speed = speed;

        vision = GetComponentInChildren<EnemyVision>();
    }

    private void Update()
    {
        if (vision.PlayerInSight)
        {
            agent.SetDestination(target.transform.position);
            agent.speed = speed * doubleValue; // Aumenta su velocidad
            animator.SetBool(ParamRun, true);
            animator.SetBool(ParamWalk, true);
            return;
        }
        else
        {
            agent.speed = speed;
            animator.SetBool(ParamRun, false);
        }

        if (agent.remainingDistance <= minRemainingDistance && !agent.pathPending && !isWaiting)
        {
            StartCoroutine(WaitBeforeNextDestination());  // Llama la corutina para esperar
        }
        agent.isStopped = isWaiting;
    }

    private IEnumerator WaitBeforeNextDestination()
    {
        isWaiting = true;
        animator.SetBool(ParamWalk, false);

        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        TeleportToWaypoint();
        animator.SetBool(ParamWalk, true);
    }

    private void TeleportToWaypoint()
    {
        Transform teleport = GetRandomWaypoint(waypoints);

        // Si el punto es visible por el jugador, cancela el TP
        if (IsPointVisibleByPlayer(teleport.position))
        {
            Debug.Log("El jugador está mirando el punto de teletransporte. Cancelando TP.");
            return;
        }

        transform.position = teleport.position;

        // Se mueve hacia un nuevo destino
        Transform newDestination = GetRandomWaypoint(waypoints);
        agent.SetDestination(newDestination.position);
    }

    private bool IsPointVisibleByPlayer(Vector3 point)
    {
        Camera playerCamera = Camera.main;
        if (playerCamera == null) return false;

        Vector3 viewportPoint = playerCamera.WorldToViewportPoint(point);

        // Verifica si el punto está en la vista de la cámara
        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
            viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
            viewportPoint.z > 0) // Debe estar delante de la cámara
        {
            // Comprobar si realmente es visible con un Raycast
            Vector3 direction = point - playerCamera.transform.position;
            if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hit))
            {
                return hit.point == point; // Si golpea exactamente el punto, está visible
            }
        }
        return false;
    }

    private Transform GetRandomWaypoint(List<Transform> waypoints)
    {
        int index = Random.Range(0, waypoints.Count);
        return waypoints[index];
    }
}
