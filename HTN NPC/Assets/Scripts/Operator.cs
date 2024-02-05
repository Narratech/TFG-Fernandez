using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Status
{
    Continue, Success, Failure
}

public class Operator
{
    protected Status status;

    public virtual void Run() { }
    public virtual void Stop() { }

    public virtual Status GetStatus()
    {
        return status;
    }
}
