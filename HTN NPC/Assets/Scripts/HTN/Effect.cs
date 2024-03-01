using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    private string id;
    private bool value;

    public Effect(string id, bool value)
    {
        this.id = id;
        this.value = value;
    }

    public void Apply(WorldState state)
    {
        state.ChangeValue(id, value);
    }
}
