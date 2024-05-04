using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClosestObject : Operator
{
    private NavMeshAgent navMeshAgent;
    private string listKey;
    private string objectKey;

    private List<Transform> objects;

    public ClosestObject(NavMeshAgent navMeshAgent, string listKey, string objectKey, WorldState worldState) : base(worldState)
    {
        this.navMeshAgent = navMeshAgent;
        this.listKey = listKey;
        this.objectKey = objectKey;
    }
    public override void Run()
    {
        objects = worldState.GetValue<List<Transform>>(listKey);
        if (objects == null || objects.Count <= 0)
        {
            status = Status.Failure;
            return;
        }

        float currentDistance = Mathf.Infinity;
        foreach (Transform item in objects)
        {
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(item.position, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                float distance = 0;
                for (int i = 1; i < path.corners.Length; i++)
                {
                    distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                if (distance < currentDistance)
                {
                    currentDistance = distance;
                    worldState.ChangeValue(objectKey, item);
                }
            }
            else
            {
                Debug.Log("SelectObject: Unable to make path to " + item.gameObject + " with parent " + item.parent.gameObject);
            }
        }

        if (currentDistance < Mathf.Infinity)
        {
            status = Status.Success;
        }
        else
        {
            status = Status.Failure;
        }
    }

    public override void Reset()
    {
        status = Status.Continue;
    }
}
