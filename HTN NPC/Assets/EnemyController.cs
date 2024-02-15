using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed = 3;
    public float minDistance = 1;
    public float range = 10;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        worldState = new WorldState();
        worldState.AddProperty("InRange", false);

        root = new CompoundTask();

        planner = new Planner(root, worldState);

        PrimitiveTask primitiveTask = new PrimitiveTask();
        root.AddTask(primitiveTask);

        MoveTo moveTo = new MoveTo(transform, target, GetComponent<Rigidbody>(), speed, minDistance);
        primitiveTask.SetOperator(moveTo);

        primitiveTask.AddCondition("InRange", true);
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
