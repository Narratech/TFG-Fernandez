using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public FieldOfView fov;

    public bool moving = false;
    public float angle = 90;
    public float moveTime = 3;
    public float waitTime = 1;

    private Quaternion initialRotation;
    private float moveTimer;
    private float waitTimer;

    private void Start()
    {
        initialRotation = transform.rotation;
        moveTimer = 0;
        waitTimer = 0;
    }

    private void Update()
    {
        fov.FieldOfViewCheck();

        if (moving)
        {
            if (moveTimer <= moveTime)
            {
                moveTimer += Time.deltaTime;
                transform.rotation = initialRotation * Quaternion.AngleAxis(moveTimer / moveTime * angle, Vector3.up);
            }
            else
            {
                if (waitTimer <= waitTime)
                {
                    waitTimer += Time.deltaTime;
                }
                else
                {
                    moveTimer = 0;
                    waitTimer = 0;
                    angle = -angle;
                    initialRotation = transform.rotation;
                }
            }
        }
    }
}
