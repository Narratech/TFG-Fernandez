using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : Operator
{
    private NavMeshAgent navMeshAgent;
    private Transform point;
    private Transform[] wayPoints;
    private string key;

    private int currentPoint = 0;
    private float threshold = 0.1f;

    public MoveTo(NavMeshAgent navMeshAgent, Transform point) : base(null)
    {
        this.navMeshAgent = navMeshAgent;
        this.point = point;
    }

    public MoveTo(NavMeshAgent navMeshAgent, Transform[] wayPoints) : base(null)
    {
        this.navMeshAgent = navMeshAgent;
        this.wayPoints = wayPoints;
    }

    public MoveTo(NavMeshAgent navMeshAgent, string key, WorldState worldState) : base(worldState)
    {
        this.navMeshAgent = navMeshAgent;
        this.key = key;
    }

    private void OnePoint()
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(point.position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(point.position);

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + threshold)
            {
                navMeshAgent.isStopped = true;
                status = Status.Success;
            }
        }
        else
        {
            navMeshAgent.isStopped = true;
            status = Status.Failure;

            Debug.Log("MoveTo: Unable to make path.");
        }
    }

    private void MultiplePoints()
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

    public override void Run()
    {
        Transform variable = worldState.GetValue<Transform>(key);
        if (variable != null)
        {
            point = variable;
        }

        if (point != null)
        {
            OnePoint();
        }
        else if (wayPoints != null)
        {
            MultiplePoints();
        }
        else
        {
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
