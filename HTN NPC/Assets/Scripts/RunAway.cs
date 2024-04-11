using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : Operator
{
    private NavMeshAgent agent;
    private Transform target;

    public RunAway(NavMeshAgent agent, WorldState worldState) : base(worldState)
    {
        this.agent = agent;
    }

    public override void Run()
    {
        target = worldState.GetValue<Transform>("target");

        if (target != null)
        {
            Vector3 direction = agent.transform.position - target.transform.position;
            Vector3 position = agent.transform.position + direction;
            agent.SetDestination(position);
        }
    }
}
