using LiquidSnake.LevelObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectExit : Operator
{
    private string listKey;
    private string objectKey;

    private List<Exit> exits;

    public SelectExit(string listKey, string objectKey, WorldState worldState) : base(worldState)
    {
        this.listKey = listKey;
        this.objectKey = objectKey;
    }

    public override void Run()
    {
        exits = worldState.GetValue<List<Exit>>(listKey);
        if (exits == null)
        {
            status = Status.Failure;
            return;
        }

        List<Exit> list = new List<Exit>();
        Exit endExit = null;

        foreach (Exit exit in exits)
        {
            if (!exit.Door && !exit.Used)
            {
                list.Add(exit);
                if (exit.End) endExit = exit;
            }
        }

        if (list.Count > 0)
        {
            if (endExit != null)
            {
                worldState.ChangeValue(objectKey, endExit.transform);
            }
            else
            {
                Exit randomExit = list[Random.Range(0, list.Count)];
                worldState.ChangeValue(objectKey, randomExit.transform);
            }
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
