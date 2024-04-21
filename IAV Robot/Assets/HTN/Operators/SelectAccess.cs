using LiquidSnake.LevelObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAccess : Operator
{
    private string roomKey;
    private string totalKey;
    private string currentKey;
    private string conditionKey;

    private List<Access> roomAccesses;
    private List<Access> totalAccesses;

    public SelectAccess(string roomKey, string totalKey, string currentKey, WorldState worldState, string conditionKey) : base(worldState)
    {
        this.roomKey = roomKey;
        this.totalKey = totalKey;
        this.currentKey = currentKey;
        this.conditionKey = conditionKey;
    }

    public override void Run()
    {
        roomAccesses = worldState.GetValue<List<Access>>(roomKey);
        totalAccesses = worldState.GetValue<List<Access>>(totalKey);

        if (roomAccesses == null || totalAccesses == null)
        {
            worldState.ChangeValue(conditionKey, false);
            status = Status.Failure;
            return;
        }

        Access accessEnd = null;
        List<Access> roomAvailable = new List<Access>();
        List<Access> allAvailable = new List<Access>();

        foreach (Access access in totalAccesses)
        {
            if (!access.Door && !access.Used)
            {
                if (access.End)
                {
                    accessEnd = access;
                }

                if (roomAccesses.Contains(access))
                {
                    roomAvailable.Add(access);
                }

                allAvailable.Add(access);
            }
        }

        if (accessEnd != null)
        {
            worldState.ChangeValue(currentKey, accessEnd.transform);
            status = Status.Success;
        }
        else if (roomAvailable.Count > 0 || allAvailable.Count > 0)
        {
            Access randomAccess = (roomAvailable.Count > 0) ? roomAvailable[Random.Range(0, roomAvailable.Count)] : allAvailable[Random.Range(0, allAvailable.Count)];
            randomAccess.Used = true;

            worldState.ChangeValue(currentKey, randomAccess.transform);
            status = Status.Success;
        }
        else
        {
            worldState.ChangeValue(conditionKey, false);
            status = Status.Failure;
        }
    }

    public override void Reset()
    {
        status = Status.Continue;
    }
}
