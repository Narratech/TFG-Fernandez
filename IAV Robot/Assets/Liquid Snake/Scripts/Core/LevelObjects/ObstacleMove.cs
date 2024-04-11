using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidSnake.LevelObjects
{
    public class ObstacleMove : MonoBehaviour
    {
        [SerializeField]
        private Transform startPosition;

        [SerializeField]
        private Transform endPosition;

        [SerializeField]
        private float moveSpeed;

        private Transform currentTarget;

        void Start()
        {
            currentTarget = startPosition;
        }

        void Update()
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, step);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.001f)
            {
                if (currentTarget == startPosition)
                {
                    currentTarget = endPosition;
                }
                else
                {
                    currentTarget = startPosition;
                }
            }
        }
    }

}
