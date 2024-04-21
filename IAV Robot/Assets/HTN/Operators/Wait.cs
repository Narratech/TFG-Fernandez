using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Operator
{
    private float time;
    private float resetValue;

    public Wait(float time) : base(null)
    {
        this.time = time;
        resetValue = time;
    }

    public override void Run()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            status = Status.Success;
        }
    }

    public override void Reset()
    {
        time = resetValue;
        status = Status.Continue;
    }
}
