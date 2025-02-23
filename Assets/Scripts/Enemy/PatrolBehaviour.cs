using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MonoBehaviour
{

    [SerializeField] private Animator animator;

    private const float minRemainingDistance = 0.5f;

    public float speed = 0.5f;     
    public float waitTime = 2f; 
    public GameObject target;
    public List<Transform> waypoints;

    private NavMeshAgent agent;
    private EnemyVision vision;
    private bool isWaiting = false;

    public bool IsWaiting { get => isWaiting; }

    private void Start()
    {
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
            agent.speed = speed * 4f; // Aumenta su velocidad
            animator.SetFloat("WalkingSpeed", agent.velocity.magnitude);

            // Effects to the player
            PlayerCam.Instance.ChangeFOV(90f);
            GrainEffect.Instance.FrightEffect();

            return;
        }
        else
        {
            // Effects to the player
            PlayerCam.Instance.ChangeFOV(60f);
            GrainEffect.Instance.StopFrightEffect();

            agent.speed = speed;
        }

        if (agent.remainingDistance <= minRemainingDistance && !agent.pathPending && !isWaiting)
        {
            StartCoroutine(WaitBeforeNextDestination());  // Llama la corutina para esperar
        }

        animator.SetFloat("WalkingSpeed", agent.velocity.magnitude);
        agent.isStopped = isWaiting;
    }

    private IEnumerator WaitBeforeNextDestination()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        isWaiting = false;
        ChangeWaypoint();
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
