using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    private NavMeshAgent agent;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        worldState = new WorldState();

        root = new CompoundTask(CompoundType.Selector);

        planner = new Planner(root, worldState);

        PrimitiveTask wanderTask = new PrimitiveTask();
        root.AddTask(wanderTask);

        Wander wander = new Wander(agent, startPosition, endPosition);
        wanderTask.SetOperator(wander);
    }

    private void Update()
    {
        planner.RunPlan();
    }
}
