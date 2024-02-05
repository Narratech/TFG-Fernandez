using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner
{
    private Task rootTask;
    private Task currentTask;

    private WorldState worldState;
    private WorldState tempState;

    private Stack<Task> tasksToProcess;
    private Stack<Task> finalPlan;

    public Planner(Task rootTask, WorldState worldState)
    {
        this.rootTask = rootTask;
        this.worldState = worldState;

        tasksToProcess = new Stack<Task>();
        finalPlan = new Stack<Task>();
    }

    private void RestoreToLastCompoundTask(Task parent)
    {
        if (parent == null) return;

        while (finalPlan.Peek().GetParent() == parent)
        {
            finalPlan.Pop();
        }
    }

    private void Plan()
    {
        tempState = worldState;
        tasksToProcess.Push(rootTask);

        while (tasksToProcess.Count > 0)
        {
            currentTask = tasksToProcess.Pop();
            if (currentTask.GetTaskType() == TaskType.Primitive)
            {
                if (currentTask.IsValid(tempState))
                {
                    ((PrimitiveTask)currentTask).ApplyEffects(tempState);
                    finalPlan.Push(currentTask);
                }
                else
                {
                    RestoreToLastCompoundTask(currentTask.GetParent());
                }
            }
            else
            {
                if (currentTask.IsValid(tempState))
                {
                    List<Task> subtasks = ((CompoundTask)currentTask).GetSubtasks();
                    foreach (Task task in subtasks)
                    {
                        tasksToProcess.Push(task);
                    }
                }
            }
        }
    }

    public void RunPlan()
    {
        while (finalPlan.Count > 0)
        {
            PrimitiveTask current = (PrimitiveTask)finalPlan.Pop();
            if (current.IsValid(worldState))
            {
                Operator action = current.GetOperator();
                while (action.GetStatus() == Status.Continue)
                {
                    current.GetOperator().Run();
                }

                if (action.GetStatus() == Status.Success)
                {
                    current.ApplyEffects(worldState);
                }
                else
                {
                    Plan();
                }
            }
            else
            {
                Plan();
            }
        }

        Plan();
    }
}
