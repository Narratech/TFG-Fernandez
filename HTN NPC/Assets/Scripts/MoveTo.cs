using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : Operator
{
    private NavMeshAgent navMeshAgent;
    private Transform targetPosition;

    private float threshold = 0.1f;

    public MoveTo(NavMeshAgent navMeshAgent, Transform targetPosition)
    {
        this.navMeshAgent = navMeshAgent;
        this.targetPosition = targetPosition;
    }

    public override void Run()
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(targetPosition.position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(targetPosition.position);

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + threshold)
            {
                Stop();
            }
        }
        else
        {
            status = Status.Failure;
        }
    }

    public override void Stop()
    {
        navMeshAgent.isStopped = true;
        status = Status.Success;
    }

    public override void Reset()
    {
        navMeshAgent.ResetPath();
        status = Status.Continue;
    }
}
