using LiquidSnake.Character;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Animator animator;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private bool planning;

    private void Start()
    {
        planning = true;

        worldState = new WorldState();

        // Variables
        worldState.AddProperty("LowHealth", false);
        worldState.AddProperty("CanHeal", false);

        worldState.AddProperty("TotalHealths", null);
        worldState.AddProperty("CurrentHealth", null);

        worldState.AddProperty("Detected", false);
        worldState.AddProperty("CanHide", false);

        worldState.AddProperty("TotalHideSpots", null);
        worldState.AddProperty("CurrentHideSpot", null);

        worldState.AddProperty("HasDestination", false);
        worldState.AddProperty("CanButton", false);
        worldState.AddProperty("IsDoor", false);
        worldState.AddProperty("IsEnd", false);

        worldState.AddProperty("CurrentButton", null);

        worldState.AddProperty("TotalExits", null);
        worldState.AddProperty("RoomExits", null);
        worldState.AddProperty("CurrentExit", null);

        root = Tree();

        planner = new Planner(root, worldState);
    }

    private PrimitiveTask SelectDestination()
    {
        SelectAccess selectAccess = new SelectAccess("RoomExits", "TotalExits", "CurrentExit", "IsDoor", "IsEnd", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectAccess);
        selectTask.AddEffect("HasDestination", true);

        return selectTask;
    }

    private PrimitiveTask MoveToDestination()
    {
        MoveTo moveToExit = new MoveTo(navMeshAgent, "CurrentExit", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToExit);
        moveTask.AddEffect("HasDestination", false);

        return moveTask;
    }

    private PrimitiveTask SelectButton()
    {
        SelectButton selectButton = new SelectButton(navMeshAgent, "CurrentExit", "CurrentButton", "CanButton", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectButton);

        return selectTask;
    }

    private PrimitiveTask MoveToButton()
    {
        MoveTo moveToExit = new MoveTo(navMeshAgent, "CurrentButton", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToExit);
        moveTask.AddEffect("IsDoor", false);

        return moveTask;
    }

    private PrimitiveTask Wait(float time)
    {
        Wait wait = new Wait(time);
        PrimitiveTask waitTask = new PrimitiveTask();
        waitTask.SetOperator(wait);

        return waitTask;
    }

    private CompoundTask GoToButton()
    {
        CompoundTask sequence = new CompoundTask(CompoundType.Sequence);

        PrimitiveTask moveTask = MoveToButton();

        PrimitiveTask waitTask = Wait(1);

        sequence.AddTask(moveTask);
        sequence.AddTask(waitTask);

        return sequence;
    }

    private CompoundTask CanButton()
    {
        CompoundTask selector = new CompoundTask(CompoundType.Selector);

        CompoundTask moveTask = GoToButton();
        moveTask.AddCondition("CanButton", true);
        moveTask.AddCondition("LowHealth", false);
        moveTask.AddCondition("Detected", false);

        PrimitiveTask selectTask = SelectDestination();
        selectTask.AddCondition("CanButton", false);

        selector.AddTask(moveTask);
        selector.AddTask(selectTask);

        return selector;
    }

    private CompoundTask OpenDoor()
    {
        CompoundTask sequence = new CompoundTask(CompoundType.Sequence);

        PrimitiveTask selectTask = SelectButton();

        CompoundTask buttonTask = CanButton();

        sequence.AddTask(selectTask);
        sequence.AddTask(buttonTask);

        return sequence;
    }

    private CompoundTask GoToDestination()
    {
        CompoundTask selector = new CompoundTask(CompoundType.Selector);

        CompoundTask doorTask = OpenDoor();
        doorTask.AddCondition("IsDoor", true);

        PrimitiveTask gateTask = MoveToDestination();
        gateTask.AddCondition("IsDoor", false);
        gateTask.AddCondition("LowHealth", false);
        gateTask.AddCondition("Detected", false);

        selector.AddTask(doorTask);
        selector.AddTask(gateTask);

        return selector;
    }

    private CompoundTask SearchNewRoom()
    {
        CompoundTask selector = new CompoundTask(CompoundType.Selector);

        CompoundTask enterTask = GoToDestination();
        enterTask.AddCondition("HasDestination", true);

        PrimitiveTask selectTask = SelectDestination();
        selectTask.AddCondition("HasDestination", false);

        selector.AddTask(enterTask);
        selector.AddTask(selectTask);

        return selector;
    }

    private CompoundTask GoToEnd()
    {
        CompoundTask selector = new CompoundTask(CompoundType.Selector);

        CompoundTask moveTask = GoToDestination();
        moveTask.AddCondition("IsEnd", true);
        moveTask.AddCondition("HasDestination", true);

        CompoundTask searchTask = SearchNewRoom();
        searchTask.AddCondition("IsEnd", false);

        selector.AddTask(moveTask);
        selector.AddTask(searchTask);

        return selector;
    }

    private PrimitiveTask SelectHealth()
    {
        ClosestObject selectObject = new ClosestObject(navMeshAgent, "TotalHealths", "CurrentHealth", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectObject);

        return selectTask;
    }

    private PrimitiveTask MoveToHealth()
    {
        MoveTo moveToHealth = new MoveTo(navMeshAgent, "CurrentHealth", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToHealth);

        return moveTask;
    }

    private CompoundTask GoToHealth()
    {
        CompoundTask sequence = new CompoundTask(CompoundType.Sequence);

        PrimitiveTask selectTask = SelectHealth();

        PrimitiveTask moveTask = MoveToHealth();

        sequence.AddTask(selectTask);
        sequence.AddTask(moveTask);

        return sequence;
    }

    private PrimitiveTask SelectHideSpot()
    {
        ClosestObject selectObject = new ClosestObject(navMeshAgent, "TotalHideSpots", "CurrentHideSpot", worldState);
        PrimitiveTask selectTask = new PrimitiveTask();
        selectTask.SetOperator(selectObject);

        return selectTask;
    }

    private PrimitiveTask MoveToHideSpot()
    {
        MoveTo moveToHealth = new MoveTo(navMeshAgent, "CurrentHideSpot", worldState);
        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.SetOperator(moveToHealth);
        moveTask.AddCondition("Detected", true);

        return moveTask;
    }

    private CompoundTask GoToHideSpot()
    {
        CompoundTask sequence = new CompoundTask(CompoundType.Sequence);

        PrimitiveTask selectTask = SelectHideSpot();

        PrimitiveTask moveTask = MoveToHideSpot();

        PrimitiveTask waitTask = Wait(2);

        sequence.AddTask(selectTask);
        sequence.AddTask(moveTask);
        sequence.AddTask(waitTask);

        return sequence;
    }

    private CompoundTask Tree()
    {
        CompoundTask selector = new CompoundTask(CompoundType.Selector);

        CompoundTask healTask = GoToHealth();
        healTask.AddCondition("CanHeal", true);
        healTask.AddCondition("LowHealth", true);

        CompoundTask hideTask = GoToHideSpot();
        hideTask.AddCondition("CanHide", true);
        hideTask.AddCondition("Detected", true);

        CompoundTask goToEndTask = GoToEnd();

        selector.AddTask(healTask);
        selector.AddTask(hideTask);
        selector.AddTask(goToEndTask);

        return selector;
    }

    private void OnFootstep(AnimationEvent animationEvent) { }

    private void Update()
    {
        Health health = GetComponent<Health>();
        if (health != null)
        {
            worldState.ChangeValue("LowHealth", health.CurrentValue() <= health.MaxValue() / 2);
        }

        worldState.ChangeValue("Detected", GameManager.Instance.TimesDetected > 0);

        if (planning)
        {
            planner.RunPlan();
        }

        animator.SetBool(Animator.StringToHash("Grounded"), true);
        animator.SetFloat(Animator.StringToHash("Speed"), navMeshAgent.velocity.magnitude / 2);
        animator.SetFloat(Animator.StringToHash("MotionSpeed"), navMeshAgent.velocity.magnitude / 2);
    }

    public void Stop()
    {
        planning = false;
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
