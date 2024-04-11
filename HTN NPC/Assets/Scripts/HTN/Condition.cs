using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    private string id;
    private bool value;

    public Condition(string id, bool value)
    {
        this.id = id;
        this.value = value;
    }

    public bool IsValid(WorldState state)
    {
        return state.GetValue<bool>(id) == value;
    }
}
