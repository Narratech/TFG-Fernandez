using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidSnake.LevelObjects
{
    public class Exit : MonoBehaviour
    {
        [SerializeField]
        private PlayerController playerController;

        [SerializeField]
        private bool used = false;

        [SerializeField]
        private bool door = false;

        [SerializeField]
        private bool end = false;

        public bool Used
        {
            get { return used; }
            set { used = value; }
        }

        public bool Door
        {
            get { return door; }
            set { door = value; }
        }

        public bool End
        {
            get { return end; }
            set { end = value; }
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, playerController.transform.position) <= 1)
            {
                Used = true;
            }
        }
    }
} // namespace LiquidSnake.LevelObjects