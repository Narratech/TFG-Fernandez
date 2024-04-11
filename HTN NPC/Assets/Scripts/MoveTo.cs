using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : Operator
{
    private NavMeshAgent navMeshAgent;
    private Transform[] wayPoints;

    private int currentPoint = 0;
    private float threshold = 0.1f;

    public MoveTo(NavMeshAgent navMeshAgent, Transform[] wayPoints) : base(null)
    {
        this.navMeshAgent = navMeshAgent;
        this.wayPoints = wayPoints;
    }

    public override void Run()
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(wayPoints[currentPoint].position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(wayPoints[currentPoint].position);

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + threshold)
            {
                if (currentPoint < wayPoints.Length - 1)
                {
                    currentPoint++;
                }
                else
                {
                    navMeshAgent.isStopped = true;
                    status = Status.Success;
                }
            }
        }
        else
        {
            navMeshAgent.isStopped = true;
            status = Status.Failure;
        }
    }

    public override void Reset()
    {
        currentPoint = 0;
        navMeshAgent.ResetPath();
        status = Status.Continue;
    }
}
