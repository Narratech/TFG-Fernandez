using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public bool useHTN = true;

    public Transform[] wayPoints;

    private NavMeshAgent agent;
    private BehaviorExecutor executor;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        executor = GetComponent<BehaviorExecutor>();

        if (useHTN) executor.enabled = false;

        worldState = new WorldState();

        root = new CompoundTask(CompoundType.Selector);

        planner = new Planner(root, worldState);

        PrimitiveTask patrolTask = new PrimitiveTask();
        root.AddTask(patrolTask);

        MoveTo moveTo = new MoveTo(agent, wayPoints);
        patrolTask.SetOperator(moveTo);
    }

    private void Update()
    {
        if (useHTN) planner.RunPlan();
    }
}
