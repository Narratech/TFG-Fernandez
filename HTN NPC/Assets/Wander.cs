using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Operator
{
    private Rigidbody rb;

    private float speed;
    private float moveTime;
    private float wanderTime;

    private float mTimer;
    private float wTimer;
    private Vector3 direction;

    public Wander(Rigidbody rb, float speed, float moveTime, float wanderTime)
    {
        this.rb = rb;
        this.speed = speed;
        this.moveTime = moveTime;
        this.wanderTime = wanderTime;

        mTimer = moveTime;
        wTimer = wanderTime;
    }

    public override void Run()
    {
        mTimer -= Time.deltaTime;
        if (mTimer < 0)
        {
            mTimer = moveTime;
            direction = Random.insideUnitSphere;
            direction.y = 0;
            direction = direction.normalized;
        }

        wTimer -= Time.deltaTime;
        if (wTimer < 0)
        {
            Stop();
        }

        if (rb.velocity.magnitude < speed)
        {
            rb.AddForce(direction * speed);
        }
    }

    public override void Stop()
    {
        status = Status.Success;
    }

    public override void Reset()
    {
        status = Status.Continue;

        mTimer = moveTime;
        wTimer = wanderTime;
    }
}
