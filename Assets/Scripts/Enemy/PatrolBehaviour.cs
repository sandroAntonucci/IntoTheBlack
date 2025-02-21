using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MonoBehaviour
{
    private const string ParamRun = "Run";

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

            return;
        }
        else
        {
            agent.speed = speed;
        }

        if (agent.remainingDistance <= minRemainingDistance && !agent.pathPending && !isWaiting)
        {
            StartCoroutine(WaitBeforeNextDestination());  // Llama la corutina para esperar
        }
        agent.isStopped = isWaiting;
        animator.speed = speed / 3;
    }

    private IEnumerator WaitBeforeNextDestination()
    {
        isWaiting = true;
        animator.SetBool(ParamRun, false);

        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        ChangeWaypoint();
        animator.SetBool(ParamRun, true);
    }

    private void ChangeWaypoint()
    {
        // Se mueve hacia un nuevo destino
        Transform newDestination = GetRandomWaypoint(waypoints);
        agent.SetDestination(newDestination.position);
    }



    private Transform GetRandomWaypoint(List<Transform> waypoints)
    {
        int index = Random.Range(0, waypoints.Count);
        return waypoints[index];
    }
}
