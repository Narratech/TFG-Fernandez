using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    private Dictionary<string, bool> state;

    public WorldState()
    {
        state = new Dictionary<string, bool>();
    }

    public void AddProperty(string key, bool value)
    {
        if (!state.ContainsKey(key))
        {
            state.Add(key, value);
        }
        else
        {
            Debug.Log("This world state property already exist.");
        }
    }

    public void ChangeValue(string key, bool value)
    {
        if (state.ContainsKey(key))
        {
            state[key] = value;
        }
        else
        {
            Debug.Log("This world state property does not exist.");
        }
    }

    public bool GetValue(string key)
    {
        if (state.ContainsKey(key))
        {
            return state[key];
        }
        else
        {
            Debug.Log("This world state property does not exist.");
            return false;
        }
    }
}
