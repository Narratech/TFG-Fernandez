using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed = 3;
    public float minDistance = 1;
    public float range = 10;
    public float moveTime = 1;
    public float wanderTime = 3;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        worldState = new WorldState();
        worldState.AddProperty("InRange", false);

        root = new CompoundTask(CompoundType.Selector);

        planner = new Planner(root, worldState);

        PrimitiveTask moveToTask = new PrimitiveTask();
        root.AddTask(moveToTask);

        MoveTo moveTo = new MoveTo(transform, target, GetComponent<Rigidbody>(), speed, minDistance);
        moveToTask.SetOperator(moveTo);

        moveToTask.AddCondition("InRange", true);

        PrimitiveTask wanderTask = new PrimitiveTask();
        root.AddTask(wanderTask);

        Wander wander = new Wander(GetComponent<Rigidbody>(), speed, moveTime, wanderTime);
        wanderTask.SetOperator(wander);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < range)
            worldState.ChangeValue("InRange", true);
        else
            worldState.ChangeValue("InRange", false);

        planner.RunPlan();
    }
}
