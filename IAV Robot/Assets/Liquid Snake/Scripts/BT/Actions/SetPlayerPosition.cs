using BBUnity.Actions;
using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Action("LiquidSnake/SetPlayerPosition")]
[Help("Stores the player position")]
public class SetPlayerPosition : GOAction
{
    [InParam("target")]
    [Help("Player target")]
    public GameObject target;

    [OutParam("targetPosition")]
    [Help("Player target position")]
    public Vector3 targetPosition;

    public override void OnStart()
    {
        if (target == null)
        {
            Debug.LogWarningFormat("[SetPlayerPosition] No se especifícón un target para enemigo {0}.", gameObject.name);
            return;
        }
        CharacterController characterController = target.GetComponent<CharacterController>();

        if (characterController != null)
        {
            targetPosition = characterController.center + target.transform.position;
        }
        else
        {
            targetPosition = target.transform.position;
        }
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();
        return TaskStatus.COMPLETED;
    }
}
