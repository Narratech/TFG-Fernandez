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

        worldState.AddProperty("CanHeal", false);
        worldState.AddProperty("CanButton", false);
        worldState.AddProperty("CanHide", false);

        worldState.AddProperty("Healths", null);
        worldState.AddProperty("Button", null);
        worldState.AddProperty("HideSpot", null);
        worldState.AddProperty("Exits", null);

        worldState.AddProperty("SelectedExit", null);
        worldState.AddProperty("SelectedHealth", null);

        root = FinalTree();

        planner = new Planner(root, worldState);
    }

    private CompoundTask MoveToExitTree()
    {
        CompoundTask exitRoot = new CompoundTask(CompoundType.Sequence);

        SelectExit selectExit = new SelectExit("Exits", "SelectedExit", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectExit);

        MoveTo moveToExit = new MoveTo(navMeshAgent, "SelectedExit", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToExit);

        exitRoot.AddTask(selectTask);
        exitRoot.AddTask(moveTask);

        return exitRoot;
    }

    private CompoundTask MoveToButtonTree()
    {
        CompoundTask buttonRoot = new CompoundTask(CompoundType.Selector);

        MoveTo moveToButton = new MoveTo(navMeshAgent, "Button", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.AddCondition("CanButton", true);
        moveTask.AddEffect("CanButton", false);
        moveTask.SetOperator(moveToButton);

        CompoundTask treeTask = MoveToExitTree();
        treeTask.AddCondition("CanButton", false);

        buttonRoot.AddTask(moveTask);
        buttonRoot.AddTask(treeTask);

        return buttonRoot;
    }

    private CompoundTask HidingTree()
    {
        CompoundTask hidingRoot = new CompoundTask(CompoundType.Selector);

        MoveTo moveToHide = new MoveTo(navMeshAgent, "HideSpot", worldState);
        PrimitiveTask hideTask = new PrimitiveTask();
        hideTask.AddCondition("CanHide", true);
        hideTask.SetOperator(moveToHide);

        CompoundTask treeTask = MoveToButtonTree();
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

        CompoundTask buttonTask = MoveToButtonTree();
        buttonTask.AddCondition("Detected", false);

        movementRoot.AddTask(hideTask);
        movementRoot.AddTask(buttonTask);

        return movementRoot;
    }

    private CompoundTask MoveToHealthTree()
    {
        CompoundTask healthRoot = new CompoundTask(CompoundType.Sequence);

        SelectHealth selectHealth = new SelectHealth(navMeshAgent, "Healths", "SelectedHealth", worldState);
        PrimitiveTask healthTask = new PrimitiveTask();
        healthTask.SetOperator(selectHealth);

        MoveTo moveToHealth = new MoveTo(navMeshAgent, "SelectedHealth", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToHealth);

        healthRoot.AddTask(healthTask);
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
}