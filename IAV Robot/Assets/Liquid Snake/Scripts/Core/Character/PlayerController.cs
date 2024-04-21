using LiquidSnake.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        worldState = new WorldState();

        // Variables
        worldState.AddProperty("LowHealth", false);
        worldState.AddProperty("Detected", false);
        worldState.AddProperty("FindAccess", true);

        worldState.AddProperty("CanHeal", false);
        worldState.AddProperty("CanButton", false);
        worldState.AddProperty("CanHide", false);

        worldState.AddProperty("HideSpot", null);
        worldState.AddProperty("TotalHealths", null);
        worldState.AddProperty("TotalButtons", null);
        worldState.AddProperty("TotalExits", null);
        worldState.AddProperty("RoomExits", null);

        worldState.AddProperty("CurrentHealth", null);
        worldState.AddProperty("CurrentButton", null);
        worldState.AddProperty("CurrentExit", null);
        worldState.AddProperty("PreviousExit", null);

        root = FinalTree();

        planner = new Planner(root, worldState);
    }

    private CompoundTask MoveToExitTree()
    {
        CompoundTask exitRoot = new CompoundTask(CompoundType.Sequence);

        SelectAccess selectAccess = new SelectAccess("RoomExits", "TotalExits", "CurrentExit", worldState, "FindAccess");
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectAccess);

        MoveTo moveToExit = new MoveTo(navMeshAgent, "CurrentExit", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToExit);

        exitRoot.AddTask(selectTask);
        exitRoot.AddTask(moveTask);

        return exitRoot;
    }

    private CompoundTask MoveToRemainingButtonTree()
    {
        CompoundTask buttonRoot = new CompoundTask(CompoundType.Sequence);

        SelectObject selectButton = new SelectObject(navMeshAgent, "TotalButtons", "CurrentButton", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectButton);

        MoveTo moveToButton = new MoveTo(navMeshAgent, "CurrentButton", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.AddEffect("FindAccess", true);
        moveTask.SetOperator(moveToButton);

        Wait wait = new Wait(1);
        PrimitiveTask waitTask = new PrimitiveTask();
        waitTask.SetOperator(wait);

        buttonRoot.AddTask(selectTask);
        buttonRoot.AddTask(moveTask);
        buttonRoot.AddTask(waitTask);

        return buttonRoot;
    }

    private CompoundTask MoveToExitOrButtonTree()
    {
        CompoundTask movementRoot = new CompoundTask(CompoundType.Selector);

        CompoundTask exitTask = MoveToExitTree();
        exitTask.AddCondition("FindAccess", true);

        CompoundTask buttonTask = MoveToRemainingButtonTree();
        buttonTask.AddCondition("FindAccess", false);

        movementRoot.AddTask(exitTask);
        movementRoot.AddTask(buttonTask);

        return movementRoot;
    }

    private CompoundTask MoveToSelectedButtonTree()
    {
        CompoundTask buttonRoot = new CompoundTask(CompoundType.Sequence);

        MoveTo moveToButton = new MoveTo(navMeshAgent, "CurrentButton", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.AddEffect("CanButton", false);
        moveTask.SetOperator(moveToButton);

        Wait wait = new Wait(1);
        PrimitiveTask waitTask = new PrimitiveTask();
        waitTask.SetOperator(wait);

        buttonRoot.AddTask(moveTask);
        buttonRoot.AddTask(waitTask);

        return buttonRoot;
    }

    CompoundTask MoveUndetectedTree()
    {
        CompoundTask undetectedRoot = new CompoundTask(CompoundType.Selector);

        CompoundTask selectedButtonTask = MoveToSelectedButtonTree();
        selectedButtonTask.AddCondition("CanButton", true);

        CompoundTask buttonOrExitTask = MoveToExitOrButtonTree();
        buttonOrExitTask.AddCondition("CanButton", false);

        undetectedRoot.AddTask(selectedButtonTask);
        undetectedRoot.AddTask(buttonOrExitTask);

        return undetectedRoot;
    }

    private CompoundTask HidingTree()
    {
        CompoundTask hidingRoot = new CompoundTask(CompoundType.Selector);

        MoveTo moveToHide = new MoveTo(navMeshAgent, "HideSpot", worldState);
        PrimitiveTask hideTask = new PrimitiveTask();
        hideTask.AddCondition("CanHide", true);
        hideTask.AddEffect("Detected", false);
        hideTask.SetOperator(moveToHide);

        CompoundTask treeTask = MoveToExitOrButtonTree();
        treeTask.AddCondition("CanHide", false);

        hidingRoot.AddTask(hideTask);
        hidingRoot.AddTask(treeTask);

        return hidingRoot;
    }

    private CompoundTask MovementTree()
    {
        CompoundTask movementRoot = new CompoundTask(CompoundType.Selector);

        CompoundTask hideTask = HidingTree();
        hideTask.AddCondition("Detected", true);

        CompoundTask buttonTask = MoveUndetectedTree();
        buttonTask.AddCondition("Detected", false);

        movementRoot.AddTask(hideTask);
        movementRoot.AddTask(buttonTask);

        return movementRoot;
    }

    private CompoundTask MoveToHealthTree()
    {
        CompoundTask healthRoot = new CompoundTask(CompoundType.Sequence);

        SelectObject selectHealth = new SelectObject(navMeshAgent, "TotalHealths", "CurrentHealth", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectHealth);

        MoveTo moveToHealth = new MoveTo(navMeshAgent, "CurrentHealth", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToHealth);

        healthRoot.AddTask(selectTask);
        healthRoot.AddTask(moveTask);

        return healthRoot;
    }

    private CompoundTask FinalTree()
    {
        CompoundTask root = new CompoundTask(CompoundType.Selector);

        CompoundTask healthTask = MoveToHealthTree();
        healthTask.AddCondition("LowHealth", true);
        healthTask.AddCondition("CanHeal", true);

        CompoundTask moveTask = MovementTree();
        moveTask.AddCondition("LowHealth", false);

        root.AddTask(healthTask);
        root.AddTask(moveTask);

        return root;
    }

    private void Update()
    {
        Health health = GetComponent<Health>();
        if (health != null && health.CurrentValue() <= health.MaxValue() / 2)
        {
            worldState.ChangeValue("LowHealth", true);
        }
        else
        {
            worldState.ChangeValue("LowHealth", false);
        }

        planner.RunPlan();
    }

    public WorldState GetWorldState()
    {
        return worldState;
    }

    public void SetWorldState(WorldState worldState)
    {
        this.worldState = worldState;
    }

    public void SetDetected(bool detected)
    {
        worldState.ChangeValue("Detected", detected);
    }
}