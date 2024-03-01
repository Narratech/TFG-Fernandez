using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CompoundType
{
    Selector, Sequence
}

public class CompoundTask : Task
{
    private CompoundType compoundType;
    private List<Task> subtasks;

    public CompoundTask(CompoundType compoundType)
    {
        this.compoundType = compoundType;

        taskType = TaskType.Compound;
        parent = null;

        conditions = new List<Condition>();
        subtasks = new List<Task>();
    }

    public void AddTask(Task task)
    {
        if (!subtasks.Contains(task))
        {
            subtasks.Add(task);
            task.SetParent(this);
        }
        else
        {
            Debug.Log("This subtask already exist.");
        }
    }

    public CompoundType GetCompoundType()
    {
        return compoundType;
    }

    public List<Task> GetSubtasks()
    {
        return subtasks;
    }
}
