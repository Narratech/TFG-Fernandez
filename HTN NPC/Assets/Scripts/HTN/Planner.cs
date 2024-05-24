using System.Collections.Generic;

public class Planner
{
    private Task rootTask;
    private Task currentTask;

    private WorldState worldState;
    private WorldState tempState;

    private Queue<Task> tasksToProcess;
    private LinkedList<Task> finalPlan;

    public Planner(Task rootTask, WorldState worldState)
    {
        this.rootTask = rootTask;
        this.worldState = worldState;

        tempState = new WorldState();
        tasksToProcess = new Queue<Task>();
        finalPlan = new LinkedList<Task>();
    }

    private void CopyState(WorldState worldState, WorldState tempState)
    {
        Dictionary<string, object> original = worldState.GetDictionary();
        Dictionary<string, object> copy = new Dictionary<string, object>();

        foreach (var item in original)
        {
            copy.Add(item.Key, item.Value);
        }

        this.tempState.SetDictionary(copy);
    }

    private void RestoreToLastCompoundTask(Task parent)
    {
        if (parent == null) return;

        while (finalPlan.Count > 0 && finalPlan.Last.Value.GetParent() == parent)
        {
            finalPlan.RemoveLast();
        }
    }

    private void Plan()
    {
        finalPlan.Clear();

        CopyState(worldState, tempState);
        tasksToProcess.Enqueue(rootTask);

        while (tasksToProcess.Count > 0)
        {
            currentTask = tasksToProcess.Dequeue();
            if (currentTask.GetTaskType() == TaskType.Primitive)
            {
                if (currentTask.IsValid(tempState))
                {
                    ((PrimitiveTask)currentTask).ApplyEffects(tempState);
                    finalPlan.AddLast(currentTask);
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
                    if (((CompoundTask)currentTask).GetCompoundType() == CompoundType.Selector)
                    {
                        int i = 0;
                        bool valid = false;
                        while (i < subtasks.Count && !valid)
                        {
                            valid = subtasks[i].IsValid(tempState);
                            if (!valid) i++;
                        }

                        if (valid)
                        {
                            tasksToProcess.Enqueue(subtasks[i]);
                        }
                    }
                    else
                    {
                        foreach (Task task in subtasks)
                        {
                            tasksToProcess.Enqueue(task);
                        }
                    }
                }
            }
        }
    }

    public void RunPlan()
    {
        if (finalPlan.Count > 0)
        {
            PrimitiveTask current = (PrimitiveTask)finalPlan.First.Value;
            Operator action = current.GetOperator();

            if (current.IsValid(worldState))
            {
                if (action.GetStatus() == Status.Continue)
                {
                    action.Run();
                }
                else if (action.GetStatus() == Status.Success)
                {
                    action.Reset();
                    current.ApplyEffects(worldState);
                    finalPlan.RemoveFirst();
                }
                else
                {
                    action.Reset();
                    Plan();
                }
            }
            else
            {
                action.Reset();
                Plan();
            }
        }
        else
        {
            Plan();
        }
    }
}
