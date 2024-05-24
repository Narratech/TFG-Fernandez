using System.Collections.Generic;
using UnityEngine;

public class PrimitiveTask : Task
{
    private List<Effect> effects;

    private Operator action;

    public PrimitiveTask()
    {
        taskType = TaskType.Primitive;
        parent = null;

        conditions = new List<Condition>();
        effects = new List<Effect>();
    }

    public void AddEffect(string name, object value)
    {
        Effect effect = new Effect(name, value);
        if (!effects.Contains(effect))
        {
            effects.Add(effect);
        }
        else
        {
            Debug.Log(name + " :This task effect already exist.");
        }
    }

    public void SetOperator(Operator action)
    {
        this.action = action;
    }

    public void ApplyEffects(WorldState worldState)
    {
        foreach (Effect effect in effects)
        {
            effect.Apply(worldState);
        }
    }

    public Operator GetOperator()
    {
        return action;
    }
}
