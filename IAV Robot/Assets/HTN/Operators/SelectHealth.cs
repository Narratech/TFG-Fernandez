using System.Collections;
using System.Collections.Generic;
using Unity.Sentis.Layers;
using UnityEngine;
using UnityEngine.AI;

public class SelectHealth : Operator
{
    private NavMeshAgent navMeshAgent;
    private string listKey;
    private string objectKey;

    private List<Transform> healths;

    public SelectHealth(NavMeshAgent navMeshAgent, string listKey, string objectKey, WorldState worldState) : base(worldState)
    {
        this.navMeshAgent = navMeshAgent;
        this.listKey = listKey;
        this.objectKey = objectKey;
    }
    public override void Run()
    {
        healths = worldState.GetValue<List<Transform>>(listKey);
        if (healths == null)
        {
            status = Status.Failure;
            return;
        }

        float currentDistance = 0;
        foreach (Transform health in healths)
        {
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(health.position, path);

            float distance = 0;
            for (int i = 1; i < path.corners.Length; i++)
            {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            if (distance < currentDistance)
            {
                currentDistance = distance;
                worldState.ChangeValue(objectKey, health);
            }
        }
        status = Status.Success;
    }

    public override void Reset()
    {
        status = Status.Continue;
    }
}
