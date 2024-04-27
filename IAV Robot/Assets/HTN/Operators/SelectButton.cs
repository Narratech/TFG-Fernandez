using LiquidSnake.LevelObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : Operator
{
    private string accessKey;
    private string buttonKey;

    public SelectButton(string accessKey, string buttonKey, WorldState worldState) : base(worldState)
    {
        this.accessKey = accessKey;
        this.buttonKey = buttonKey;
    }

    public override void Run()
    {
        Transform variable = worldState.GetValue<Transform>(accessKey);
        if(variable == null )
        {
            status = Status.Failure;
            return;
        }

        Access access = variable.GetComponent<Access>();
        Button button = access.Door.GetButton();
        worldState.ChangeValue(buttonKey, button.GetPosition());

        status = Status.Success;
    }

    public override void Reset()
    {
        status = Status.Continue;
    }
}
