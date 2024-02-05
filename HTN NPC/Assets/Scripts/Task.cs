using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    Primitive, Compound
}

public class Task
{
    protected TaskType taskType;
    protected CompoundTask parent;
    protected List<Condition> conditions;

    public virtual void SetParent(CompoundTask parent)
    {
        this.parent = parent;
    }

    public virtual void AddCondition(string name, bool value)
    {
        Condition condition = new Condition(name, value);
        if (!conditions.Contains(condition))
        {
            conditions.Add(condition);
        }
        else
        {
            Debug.Log("This task condition already exist.");
        }
    }

    public virtual bool IsValid(WorldState worldState)
    {
        bool result = true;

        int i = 0;
        while (i < conditions.Count && result)
        {
            result = conditions[i].IsValid(worldState);
        }

        return result;
    }

    public virtual TaskType GetTaskType()
    {
        return taskType;
    }

    public virtual CompoundTask GetParent()
    {
        return parent;
    }
}
