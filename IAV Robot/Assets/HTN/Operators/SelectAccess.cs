using LiquidSnake.LevelObjects;
using System.Collections.Generic;
using UnityEngine;

public class SelectAccess : Operator
{
    private string roomKey;
    private string totalKey;
    private string currentKey;
    private string doorKey;
    private string endKey;

    private List<Access> roomAccesses;
    private List<Access> totalAccesses;

    public SelectAccess(string roomKey, string totalKey, string currentKey, string doorKey, string endKey, WorldState worldState) : base(worldState)
    {
        this.roomKey = roomKey;
        this.totalKey = totalKey;
        this.currentKey = currentKey;
        this.doorKey = doorKey;
        this.endKey = endKey;
    }

    public override void Run()
    {
        roomAccesses = worldState.GetValue<List<Access>>(roomKey);
        totalAccesses = worldState.GetValue<List<Access>>(totalKey);

        if (roomAccesses == null || totalAccesses == null)
        {
            status = Status.Failure;
            return;
        }

        Access goal = null;
        List<Access> roomAvailable = new List<Access>();
        List<Access> allAvailable = new List<Access>();

        foreach (Access access in totalAccesses)
        {
            if (access.IsEnd)
            {
                goal = access;
            }

            if (!access.Room.Discovered)
            {

                if (roomAccesses.Contains(access))
                {
                    roomAvailable.Add(access);
                }

                allAvailable.Add(access);
            }
        }

        if (goal != null)
        {
            worldState.ChangeValue(currentKey, goal.transform);
            worldState.ChangeValue(doorKey, goal.IsDoor);
            worldState.ChangeValue(endKey, goal.IsEnd);

            status = Status.Success;
        }
        else if (roomAvailable.Count > 0 || allAvailable.Count > 0)
        {
            Access randomAccess = (roomAvailable.Count > 0) ? roomAvailable[Random.Range(0, roomAvailable.Count)] : allAvailable[Random.Range(0, allAvailable.Count)];
            randomAccess.Room.Discovered = true;

            worldState.ChangeValue(currentKey, randomAccess.transform);
            worldState.ChangeValue(doorKey, randomAccess.IsDoor);
            worldState.ChangeValue(endKey, randomAccess.IsEnd);

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
