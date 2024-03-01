using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;

    private Rigidbody rb;
    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            direction = Vector3.forward;
        else if (Input.GetKey(KeyCode.S))
            direction = Vector3.back;
        else if (Input.GetKey(KeyCode.D))
            direction = Vector3.right;
        else if (Input.GetKey(KeyCode.A))
            direction = Vector3.left;
        else
            direction = Vector3.zero;

        if (rb.velocity.magnitude < speed)
        {
            rb.AddForce(direction * speed);
        }
    }
}
