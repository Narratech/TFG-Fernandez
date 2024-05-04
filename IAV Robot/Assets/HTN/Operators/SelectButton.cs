using LiquidSnake.LevelObjects;
using UnityEngine;
using UnityEngine.AI;

public class SelectButton : Operator
{
    private NavMeshAgent navMeshAgent;
    private string accessKey;
    private string buttonKey;
    private string conditionKey;

    public SelectButton(NavMeshAgent navMeshAgent, string accessKey, string buttonKey, string conditionKey, WorldState worldState) : base(worldState)
    {
        this.navMeshAgent = navMeshAgent;
        this.accessKey = accessKey;
        this.buttonKey = buttonKey;
        this.conditionKey = conditionKey;
    }

    public override void Run()
    {
        Transform variable = worldState.GetValue<Transform>(accessKey);
        if (variable == null)
        {
            status = Status.Failure;
            return;
        }

        Access access = variable.GetComponent<Access>();
        Button button = access.Door.GetButton();
        worldState.ChangeValue(buttonKey, button.GetPosition());

        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(button.GetPosition().position, path);
        worldState.ChangeValue(conditionKey, path.status == NavMeshPathStatus.PathComplete);

        status = Status.Success;
    }

    public override void Reset()
    {
        status = Status.Continue;
    }
}
