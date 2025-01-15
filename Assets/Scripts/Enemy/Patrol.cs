using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    private const string ParamWalk = "Walk", ParamRun = "Run";

    private const int doubleValue = 1;
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
            animator.SetBool(ParamRun, false);
        }

        if (agent.remainingDistance <= minRemainingDistance && !agent.pathPending && !isWaiting)
        {
            StartCoroutine(WaitBeforeNextDestination());  // Llama la corutina para esperar
        }
    }

    private IEnumerator WaitBeforeNextDestination()
    {
        isWaiting = true;
        animator.SetBool(ParamWalk, false);
        yield return new WaitForSeconds(waitTime);

        Transform newDestination = GetNewWaypoint(waypoints);
        agent.SetDestination(newDestination.position);
        agent.speed = agent.speed == speed ? speed : speed / doubleValue;

        while (agent.remainingDistance > minRemainingDistance)
        {
            animator.SetBool(ParamWalk, true);
            yield return null; 
        }

        isWaiting = false;
    }

    private Transform GetNewWaypoint(List<Transform> waypoints)
    {
        int index = GetRandomInt(0, waypoints.Count);
        return waypoints[index];
    }

    private int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }
}
