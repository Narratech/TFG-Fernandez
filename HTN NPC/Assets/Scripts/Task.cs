using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    private WorldState worldState;

    private List<Condition> conditions;
    private List<Effect> effects;

    public Task(WorldState worldState)
    {
        conditions = new List<Condition>();
        effects = new List<Effect>();
        this.worldState = worldState;
    }

    public void AddCondition(string name, bool value)
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

    public void AddEffect(string name, bool value)
    {
        Effect effect = new Effect(name, value);
        if (!effects.Contains(effect))
        {
            effects.Add(effect);
        }
        else
        {
            Debug.Log("This task effect already exist.");
        }
    }

    public void ApplyEffects()
    {
        foreach (Effect effect in effects)
        {
            effect.Apply(worldState);
        }
    }

    public bool IsValid()
    {
        bool result = true;

        int i = 0;
        while (i < conditions.Count && result)
        {
            result = conditions[i].IsValid(worldState);
        }

        return result;
    }
}
