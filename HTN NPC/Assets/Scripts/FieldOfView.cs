using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public float radius;
    public float angle;

    public bool FieldOfViewCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (colliders.Length > 0)
        {
            Transform target = colliders[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                return !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
