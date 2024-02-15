using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : Operator
{
    private Transform currentPosition;
    private Transform targetPosition;
    private Rigidbody rb;

    private float speed;
    private float minDistance;

    private Vector3 direction;

    public MoveTo(Transform currentPosition, Transform targetPosition, Rigidbody rb, float speed, float minDistance)
    {
        this.currentPosition = currentPosition;
        this.targetPosition = targetPosition;
        this.rb = rb;
        this.speed = speed;
        this.minDistance = minDistance;
    }

    public override void Run()
    {
        direction = targetPosition.position - currentPosition.position;
        direction = direction.normalized;

        float distance = Vector3.Distance(targetPosition.position, currentPosition.position);

        if (distance > minDistance)
        {
            if (rb.velocity.magnitude < speed)
            {
                rb.AddForce(direction * speed);
            }
        }
        else
        {
            Stop();
        }
    }

    public override void Stop()
    {
        rb.velocity = Vector3.zero;
    }
}
