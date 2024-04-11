using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public bool useHTN = true;

    public Transform[] wayPoints;

    private NavMeshAgent agent;
    private BehaviorExecutor executor;
    private FieldOfView fov;

    private Animator animator;
    private AudioSource audioSource;

    private WorldState worldState;
    private Planner planner;
    private CompoundTask root;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        executor = GetComponent<BehaviorExecutor>();
        fov = GetComponent<FieldOfView>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (useHTN) executor.enabled = false;

        worldState = new WorldState();
        worldState.AddProperty("enemyNear", false);
        worldState.AddProperty("target", null);

        root = new CompoundTask(CompoundType.Selector);

        planner = new Planner(root, worldState);

        PrimitiveTask moveTask = new PrimitiveTask();
        moveTask.AddCondition("enemyNear", false);
        root.AddTask(moveTask);

        MoveTo moveTo = new MoveTo(agent, wayPoints);
        moveTask.SetOperator(moveTo);

        PrimitiveTask runTask = new PrimitiveTask();
        runTask.AddCondition("enemyNear", true);
        root.AddTask(runTask);

        RunAway runAway = new RunAway(agent, worldState);
        runTask.SetOperator(runAway);
    }

    private void Update()
    {
        if (useHTN) planner.RunPlan();

        worldState.ChangeValue("enemyNear", fov.FieldOfViewCheck());
        worldState.ChangeValue("target", fov.GetTarget());

        bool isWalking = agent.velocity.magnitude > 0.1;
        animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
