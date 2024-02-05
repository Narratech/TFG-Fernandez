using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundTask : Task
{
    private List<Task> subtasks;

    public CompoundTask()
    {
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

    public List<Task> GetSubtasks()
    {
        return subtasks;
    }
}
