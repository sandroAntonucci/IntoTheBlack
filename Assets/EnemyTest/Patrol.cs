using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    private const float minRemainingDistance = 0.5f;

    public GameObject target;
    public List<Transform> waypoints;

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_agent.transform.position);
    }

    private void Update()
    {
        if (_agent.remainingDistance <= minRemainingDistance && !_agent.pathPending)
        {
            Debug.Log("¡El agente ha llegado a su destino!");

            Transform newDestination = GetNewWaypoint(waypoints);
            _agent.SetDestination(newDestination.position);
        }
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
