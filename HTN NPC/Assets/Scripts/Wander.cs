using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : Operator
{
    private NavMeshAgent navMeshAgent;
    private Transform startPosition;
    private Transform endPosition;

    private Vector3 position;

    private float threshold = 0.1f;

    public Wander(NavMeshAgent navMeshAgent, Transform startPosition, Transform endPosition)
    {
        this.navMeshAgent = navMeshAgent;
        this.startPosition = startPosition;
        this.endPosition = endPosition;

        position = endPosition.position;
    }

    public override void Run()
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(position);

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + threshold)
            {
                position = position == startPosition.position ? endPosition.position : startPosition.position;
            }
        }
        else
        {
            status = Status.Failure;
        }
    }

    public override void Reset()
    {
        navMeshAgent.ResetPath();
        status = Status.Continue;
    }
}
