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
        // Se teletransporta a un nuevo lugar
        Transform teleport = GetRandomWaypoint(waypoints);
        transform.position = teleport.position;

        //Debug.Log($"TELEPORT: {teleport.name}");

        // Se mueve hacia ese destino
        Transform newDestination = GetRandomWaypoint(waypoints);
        agent.SetDestination(newDestination.position);

        //Debug.Log($"DESTINATION: {newDestination.name}");
    }

    private Transform GetRandomWaypoint(List<Transform> waypoints)
    {
        int index = Random.Range(0, waypoints.Count);
        return waypoints[index];
    }
}
