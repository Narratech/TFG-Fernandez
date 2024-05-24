using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public Transform[] wayPoints;

    private NavMeshAgent agent;
    private FieldOfView fov;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();

        worldState = new WorldState();
        worldState.AddProperty("enemyNear", false);
        worldState.AddProperty("target", null);

        root = new CompoundTask(CompoundType.Selector);

        planner = new Planner(root, worldState);

        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.AddCondition("enemyNear", false);
        root.AddTask(moveTask);

        MoveTo moveTo = new MoveTo(agent, wayPoints);
        moveTask.SetOperator(moveTo);

        PrimitiveTask runTask = new PrimitiveTask();
        runTask.AddCondition("enemyNear", true);
        root.AddTask(runTask);

        RunAway runAway = new RunAway(agent, worldState);
        runTask.SetOperator(runAway);
    }

    private void Update()
    {
        planner.RunPlan();

        worldState.ChangeValue("enemyNear", fov.FieldOfViewCheck());
        worldState.ChangeValue("target", fov.GetTarget());
    }
}
