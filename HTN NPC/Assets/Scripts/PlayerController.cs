using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Transform target;

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

        PrimitiveTask moveToTask = new PrimitiveTask();
        root.AddTask(moveToTask);

        MoveTo moveTo = new MoveTo(agent, target);
        moveToTask.SetOperator(moveTo);
    }

    private void Update()
    {
        planner.RunPlan();
    }
}
