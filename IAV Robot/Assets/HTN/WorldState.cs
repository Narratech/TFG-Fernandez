using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState
{
    private Dictionary<string, object> state;

    public WorldState()
    {
        state = new Dictionary<string, object>();
    }

    public void AddProperty(string key, object value)
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

    public void ChangeValue(string key, object value)
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

    public T GetValue<T>(string key)
    {
        if (state.ContainsKey(key))
        {
            return (T)state[key];
        }
        else
        {
            Debug.Log("This world state property does not exist.");
            return default;
        }
    }
}
